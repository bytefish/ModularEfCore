// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ModularEfCore.Context;

namespace ModularEfCore.Factory
{
    public interface IApplicationDbContextFactory
    {
        ApplicationDbContext Create();
    }
}
