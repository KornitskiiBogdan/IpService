using Confluent.Kafka;

namespace IpService.Service;

public static class KafkaSectionExtensions
{
    public static ConsumerConfig ToConsumerConfig(this KafkaSection kafkaSection)
    {
        var conf = new ConsumerConfig()
        {
            GroupId = kafkaSection.GroupId,
            BootstrapServers = kafkaSection.Uri,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            MessageMaxBytes = kafkaSection.MessageMaxBytes,
            MaxPollIntervalMs = kafkaSection.MaxPollIntervalMs,
            EnablePartitionEof = false,
        };

        if (kafkaSection.Other == null)
        {
            return conf;
        }

        foreach (var other in kafkaSection.Other)
        {
            conf.Set(other.Key, other.Value);
        }

        return conf;
    }
}