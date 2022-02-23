using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MockEfDbSet.Test.TestUtils
{
    public class TestDbAsyncEnumerableNSub<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerableNSub(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerableNSub(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestDbAsyncEnumeratorNSub<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProviderNSub<T>(this); }
        }
    }
}