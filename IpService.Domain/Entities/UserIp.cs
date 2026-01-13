namespace IpService.Domain.Entities
{
    public class UserIp : HasIdEntity<Guid>
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTimeOffset LastConnectedAt { get; set; }
    }
}
