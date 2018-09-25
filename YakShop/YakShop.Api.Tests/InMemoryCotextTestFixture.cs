using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.Infrastructure;

namespace YakShop.Api.Tests
{ 
    public class InMemoryContextTestFixture : IDisposable
    {
        public YakShopContext Context => InMemoryContext();
        public YakShopContext FreshContext => InMemoryFreshContext();

        private static YakShopContext _context = null;
        public void Dispose()
        {
            Context?.Dispose();
        }

        private static YakShopContext InMemoryContext()
        {
            if (_context == null)
            {
                var options = new DbContextOptionsBuilder<YakShopContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging()
                    .Options;
                _context = new YakShopContext(options);
            }
            return _context;
        }

        private static YakShopContext InMemoryFreshContext()
        {
            var options = new DbContextOptionsBuilder<YakShopContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .EnableSensitiveDataLogging()
                   .Options;
            return  new YakShopContext(options);
        }
    }

    [CollectionDefinition("YakShopContext")]
    public class YakShopContextCollection : ICollectionFixture<InMemoryContextTestFixture>
    {
    }
}
