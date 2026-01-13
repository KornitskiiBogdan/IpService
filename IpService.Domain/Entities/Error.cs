namespace IpService.Domain.Entities
{
    public record Error(string Description)
    {
        public static Error Create(string description) => new(description);
    }
}
