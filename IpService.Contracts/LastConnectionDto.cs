namespace IpService.Contracts
{
    public class LastConnectionDto
    {
        public DateTimeOffset LastConnectionDate { get; set; }
        public string Ip { get; set; }
    }
}
