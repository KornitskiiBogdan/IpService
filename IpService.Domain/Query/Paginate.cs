using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.Query;

public sealed class Paginate<TModel, TQuery> : IPaginate<TModel, TQuery> where TModel : class
{
    private readonly int _pageNumber;
    private readonly int _pageSize;
    private readonly Func<TQuery, int, int, TQuery> _paginate;

    private Paginate(int pageNumber, int pageSize, Func<TQuery, int, int, TQuery>? paginate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pageNumber);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        _paginate = paginate ?? throw new ArgumentNullException(nameof(paginate));
        _pageNumber = pageNumber;
        _pageSize = pageSize;
    }

    public static Paginate<TModel, TQuery>? Create(
        int? pageNumber, 
        int? pageSize,
        Func<TQuery, int, int, TQuery>? paginate = null)
    {
        if (!pageNumber.HasValue || !pageSize.HasValue)
        {
            return null;
        }

        return new Paginate<TModel, TQuery>(pageNumber.Value, pageSize.Value, paginate);
    }

    public TQuery Apply(TQuery provider)
    {
        return _paginate.Invoke(provider, _pageNumber, _pageSize);
    }
}