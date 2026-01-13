namespace IpService.Contracts;

public class UserIpEventMessage
{
    public long UserId { get; set; }
    public string IpAddress { get; set; }
    public DateTimeOffset LastConnectedAt { get; set; }
}