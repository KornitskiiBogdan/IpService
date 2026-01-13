namespace IpService.Domain.Entities
{
    public interface IHasIdEntity<out T>
    {
        T Id { get; }
    }
}
