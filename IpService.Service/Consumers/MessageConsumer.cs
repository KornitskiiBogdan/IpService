using IpService.Contracts;
using IpService.Service.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IpService.Service.Consumers
{
    public class MessageConsumer(
        IMediator mediator,
        ILogger<MessageConsumer> logger)
        : IMessageConsumer<UserIpEventMessage>
    {
        private readonly ILogger<MessageConsumer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task ConsumeAsync(UserIpEventMessage message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Received message {message}");

            try
            {
                ArgumentNullException.ThrowIfNull(message);

                await _mediator.Send(AddedUserIpToDbCommand.Create(message), cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Error during processing: {Exception}", e);
            }
        }
    }
}
