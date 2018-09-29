// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using ModularEfCore.Seed;

namespace ModularEfCore.Example.Test.Mocks
{
    public class MockDbContextSeed : IDbContextSeed
    {
        public void Seed(ModelBuilder modelBuilder)
        {
            // Nothing to do...
        }
    }
}
