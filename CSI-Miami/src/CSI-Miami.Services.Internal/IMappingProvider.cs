using System.Collections.Generic;
using System.Linq;

namespace CSI_Miami.Services.Internal
{
    public interface IMappingProvider
    {
        TDestination MapTo<TDestination>(object source);

        TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination);

        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable<object> source);

        IEnumerable<TDestination> EnumerableProjectTo<TSource, TDestination>(IEnumerable<TSource> source);
    }
}