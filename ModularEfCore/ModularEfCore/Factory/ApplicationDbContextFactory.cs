// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ModularEfCore.Context;

namespace ModularEfCore.Factory
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly ApplicationDbContextOptions options;

        public ApplicationDbContextFactory(ApplicationDbContextOptions options)
        {
            this.options = options;
        }

        public ApplicationDbContext Create()
        {
            return new ApplicationDbContext(options);
        }
    }
}
