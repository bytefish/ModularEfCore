// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ModularEfCore.Map;
using ModularEfCore.Seed;

namespace ModularEfCore.Context
{
    public class ApplicationDbContextOptions
    {
        public readonly DbContextOptions<ApplicationDbContext> Options;
        public readonly IDbContextSeed DbContextSeed;
        public readonly IEnumerable<IEntityTypeMap> Mappings;

        public ApplicationDbContextOptions(DbContextOptions<ApplicationDbContext> options, IDbContextSeed dbContextSeed, IEnumerable<IEntityTypeMap> mappings)
        {
            DbContextSeed = dbContextSeed;
            Options = options;
            Mappings = mappings;
        }
    }

    public class ApplicationDbContext : DbContext
    {
        private readonly ApplicationDbContextOptions options;
        
        public ApplicationDbContext(ApplicationDbContextOptions options)
            : base(options.Options)
        {
            this.options = options;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var mapping in options.Mappings)
            {
                mapping.Map(builder);
            }

            options.DbContextSeed?.Seed(builder);
        }
    }

    public static class ApplicationDbContextExtensions
    {
        public static DbSet<TEntityType> DbSet<TEntityType>(this ApplicationDbContext context)
            where TEntityType : class
        {
            return context.Set<TEntityType>();
        }
    }
}
