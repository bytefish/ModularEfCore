// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModularEfCore.Example.Database.Model;
using ModularEfCore.Seed;

namespace ModularEfCore.Example.Web.Database
{
    public class DbContextSeed : IDbContextSeed
    {
        private readonly IConfiguration configuration;

        public DbContextSeed(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            // Add Customers:
            var customer1 = new Customer {Id = 1, FirstName = "Philipp", LastName = "Wagner"};
            var customer2 = new Customer {Id = 2, FirstName = "Max", LastName = "Mustermann"};
            

            modelBuilder.Entity<Customer>()
                .HasData(customer1, customer2);
        }
    }
}
