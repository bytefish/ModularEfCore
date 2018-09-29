// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularEfCore.Context;
using ModularEfCore.Example.Database.Map;
using ModularEfCore.Example.Database.Model;
using ModularEfCore.Example.Test.Base;
using ModularEfCore.Example.Test.Mocks;
using ModularEfCore.Factory;
using ModularEfCore.Map;
using ModularEfCore.Seed;
using NUnit.Framework;

namespace ModularEfCore.Example.Test
{
    [TestFixture]
    public class AsyncCustomerTest : AsyncTransactionalTestBase
    {
        protected override async Task OnSetupBeforeTransactionAsync()
        {
            await context.Database.EnsureCreatedAsync();
        }

        [Test]
        public async Task TestInsertUsersAsync()
        {
            // Generate some Test Data:
            var customer1 = new Customer() {FirstName = "A", LastName = "Wagner"};
            var customer2 = new Customer() {FirstName = "B", LastName = "Mustermann"};

            // Insert the Two Customers:
            await context.DbSet<Customer>().AddRangeAsync(customer1, customer2);

            await context.SaveChangesAsync();

            var result = await context.DbSet<Customer>()
                .OrderBy(x => x.FirstName)
                .ToListAsync();

            // Check the two Customers have been successfully inserted into the DB:
            Assert.AreEqual(result[0].FirstName, customer1.FirstName);
            Assert.AreEqual(result[0].LastName, customer1.LastName);

            Assert.AreEqual(result[1].FirstName, customer2.FirstName);
            Assert.AreEqual(result[1].LastName, customer2.LastName);
        }

        protected override async Task OnTeardownAfterTransactionAsync()
        {
            // Insert the Two Customers:
            var countOfCustomers = await context.DbSet<Customer>()
                .CountAsync()
                .ConfigureAwait(false);

            Assert.AreEqual(0, countOfCustomers);
        }

        protected override void RegisterDependencies(ServiceCollection services)
        {
            // Register Database Entity Maps:
            services.AddSingleton<IEntityTypeMap, CustomerMap>();

            // Register an Empty Context Seed:
            services.AddSingleton<IDbContextSeed, MockDbContextSeed>();

            // Add the DbContextOptions:
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(@"Server=.\MSSQLSERVER2017;Database=TestDB;Trusted_Connection=True;")
                .Options;

            services.AddSingleton(dbContextOptions);

            // Finally register the DbContextOptions:
            services.AddSingleton<ApplicationDbContextOptions>();

            // This Factory is used to create the DbContext from the custom DbContextOptions:
            services.AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory>();

            // Finally Add the Applications DbContext:
            services.AddDbContext<ApplicationDbContext>();
        }
    }
}
