// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularEfCore.Context;
using ModularEfCore.Example.Database.Map;
using ModularEfCore.Example.Database.Model;
using ModularEfCore.Example.Test.Base;
using ModularEfCore.Factory;
using ModularEfCore.Map;
using ModularEfCore.Seed;
using NUnit.Framework;

namespace ModularEfCore.Example.Test
{
    public class DbContextSeed : IDbContextSeed
    {
        public void Seed(ModelBuilder modelBuilder)
        {
            // Nothing to do...
        }
    }

    [TestFixture]
    public class CustomerTest : TransactionalTestBase
    {
        protected override void OnSetupBeforeTransaction()
        {
            // Now first resolve the Factory for creating the ApplicationDbContext:
            var factory = services.GetService<IApplicationDbContextFactory>();

            // Make sure the Test Database is created:
            using (var context = factory.Create())
            {
                context.Database.EnsureCreated();
            }
        }

        [Test]
        public void TestInsertUsers()
        {
            // Generate some Test Data:
            var customer1 = new Customer() {FirstName = "A", LastName = "Wagner"};
            var customer2 = new Customer() {FirstName = "B", LastName = "Mustermann"};

            // Now first resolve the Factory for creating the ApplicationDbContext:
            var factory = services.GetService<IApplicationDbContextFactory>();

            // Insert the Two Customers:
            using (var context = factory.Create())
            {
                context.DbSet<Customer>().Add(customer1);
                context.DbSet<Customer>().Add(customer2);

                context.SaveChanges();
            }

            // Use the Factory to create the ApplicationDbContext:
            using (var context = factory.Create())
            {
                var result = context.DbSet<Customer>()
                    .OrderBy(x => x.FirstName)
                    .ToList();

                // Check the two Customers have been successfully inserted into the DB:
                Assert.AreEqual(result[0].FirstName, customer1.FirstName);
                Assert.AreEqual(result[0].LastName, customer1.LastName);

                Assert.AreEqual(result[1].FirstName, customer2.FirstName);
                Assert.AreEqual(result[1].LastName, customer2.LastName);
            }
        }

        protected override void OnTeardownAfterTransaction()
        {
            // Now first resolve the Factory for creating the ApplicationDbContext:
            var factory = services.GetService<IApplicationDbContextFactory>();

            // Insert the Two Customers:
            using (var context = factory.Create())
            {
                var countOfCustomers = context.DbSet<Customer>().Count();

                Assert.AreEqual(0, countOfCustomers);
            }
        }

        protected override void RegisterDependencies(ServiceCollection services)
        {
            // Register Database Entity Maps:
            services.AddSingleton<IEntityTypeMap, CustomerMap>();

            // Register an Empty Context Seed:
            services.AddSingleton<IDbContextSeed, DbContextSeed>();

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
