using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IpService.Service.Consumers;

public class KafkaConsumer<TKey, TMessage> : IKafkaConsumer<TKey, TMessage> where TMessage : class
{
    private readonly KafkaSection _kafkaConfig;
    private readonly ILogger<KafkaConsumer<TKey, TMessage>> _logger;
    private readonly IDeserializer<TMessage> _deserializer;
    private readonly IMessageConsumer<TMessage> _messageConsumer;

    public KafkaConsumer(
        IOptions<KafkaSection> kafkaConfig,
        ILogger<KafkaConsumer<TKey, TMessage>> logger,
        IDeserializer<TMessage> deserializer,
        IMessageConsumer<TMessage> messageConsumer)
    {
        _kafkaConfig = kafkaConfig.Value ?? throw new ArgumentNullException(nameof(kafkaConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
    }

    public async Task SubscribeAsync(string topic, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting consumer");

        using var consumer = BuildConsumer(_logger);
        consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);

                if (consumeResult == null)
                {
                    continue;
                }

                try
                {
                    _logger.LogDebug("Message received {val} with {key}, TopicPartition:{tp}, TopicPartitionOffset:{tpo}, Topic:{t}, Offset:{o}",
                        typeof(TMessage).Name,
                        consumeResult.Message.Key,
                        consumeResult.TopicPartition,
                        consumeResult.TopicPartitionOffset,
                        consumeResult.Topic,
                        consumeResult.Offset);

                    var message = consumeResult.Message.Value;

                    await _messageConsumer.ConsumeAsync(message, cancellationToken);

                    try
                    {
                        consumer.Commit(consumeResult);
                    }
                    catch (KafkaException e)
                    {
                        _logger.LogError(e, "Commit Offset error: {reason}", e.Error.Reason);
                        throw;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Message process error {msg}", e.Message);
                    consumer.Assign(consumeResult.TopicPartitionOffset);
                }
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, "Consume error: {reason}", e.Error.Reason);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation canceled");
            }
        }

        consumer.Close();
    }

    private IConsumer<TKey, TMessage> BuildConsumer(ILogger logger) =>
        new ConsumerBuilder<TKey, TMessage>(_kafkaConfig.ToConsumerConfig())
            .SetErrorHandler((_, eb) => logger.LogError("Error: {reason}, fatality: {}", eb.Reason.Length, eb.IsFatal))
            .SetStatisticsHandler((_, json) => logger.LogDebug("Statistics: {stat}", json))
            .SetPartitionsAssignedHandler((_, partitions) =>
                logger.LogDebug("Assigned Partitions: [{part}]", string.Join(", ", partitions)))
            .SetPartitionsRevokedHandler((_, partitions) =>
                logger.LogDebug("Revoking Assignment: [{part}]", string.Join(", ", partitions)))
            .SetPartitionsLostHandler((_, partitions) =>
                logger.LogDebug("Lost Partitions: [{part}]", string.Join(", ", partitions)))
            .SetValueDeserializer(_deserializer)
            .Build();
}