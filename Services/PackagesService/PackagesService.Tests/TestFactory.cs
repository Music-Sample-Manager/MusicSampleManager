using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using PackagesService.DAL;
using System.Collections.Generic;

namespace PackagesService.Tests
{
    internal class TestFactory
    {
        internal static DefaultHttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    { queryStringKey, queryStringValue }
                })
            };
        }

        internal static MSMDbContext GetInMemoryDbContext()
        {
            return new MSMDbContext(true);
        }
    }
}