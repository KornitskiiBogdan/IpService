using System.Net;
using IpService.Cqrs;
using IpService.Dal.Ef.Store;
using IpService.Domain.Entities;
using IpService.Service.Commands;
using Mapster;
using Microsoft.Extensions.Logging;

namespace IpService.Service.Handlers;

public class AddedUserIpToDbCommandHandler(IEfStore<UserIp> store, ILogger<AddedUserIpToDbCommandHandler> logger)
    : ICommandHandler<AddedUserIpToDbCommand, Result>
{
    private readonly IEfStore<UserIp> _store = store ?? throw new ArgumentNullException(nameof(store));
    private readonly ILogger<AddedUserIpToDbCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> Handle(AddedUserIpToDbCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;

        if (string.IsNullOrEmpty(message.IpAddress))
        {
            throw new ArgumentException($"{nameof(message.IpAddress)} cannot be empty");
        }

        if (!IPAddress.TryParse(message.IpAddress, out _))
        {
            throw new ArgumentException($"Invalid {message.IpAddress}");
        }

        var entity = request.Message.Adapt<UserIp>();

        var result = await _store.AddAsync(entity, cancellationToken);

        if (result.IsSuccess)
        {
            return Result.Success();
        }

        _logger.LogError($"Entity added failed due to unknown error {result.Error?.Description}");

        throw new InvalidOperationException(result.Error?.Description);

    }
}