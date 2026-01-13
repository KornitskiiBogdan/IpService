namespace IpService.Service
{
    public sealed class KafkaSection
    {
        public string Uri { get; set; }

        public string GroupId { get; set; } = "139CDBDE-DF4D-4A6D-8C83-E524524DEACD";

        public int MessageMaxBytes { get; set; } = 1000000;

        public int MaxPollIntervalMs { get; set; } = 300000;

        public Dictionary<string, string>? Other { get; set; }
    }
}
