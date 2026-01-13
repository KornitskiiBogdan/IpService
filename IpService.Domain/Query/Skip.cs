using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.Query;

public sealed class Skip<TModel, TQuery> : IPaginate<TModel, TQuery> where TModel : class
{
    private readonly int _size;
    private readonly Func<TQuery, int, TQuery> _paginate;

    private Skip(int size, Func<TQuery, int, TQuery> paginate)
    {
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        _paginate = paginate ?? throw new ArgumentNullException(nameof(paginate));
        _size = size;
    }

    public static Skip<TModel, TQuery> Create(int size, Func<TQuery, int, TQuery> paginate)
    {
        return new(size, paginate);
    }

    public TQuery Apply(TQuery provider)
    {
        return _paginate.Invoke(provider, _size);
    }
}