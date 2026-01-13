namespace IpService.Domain.Entities;

public abstract class HasIdEntity<T> : IHasIdEntity<T>
{
    public T Id { get; set; }
}