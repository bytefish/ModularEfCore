// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;

namespace ModularEfCore.Map
{
    public interface IEntityTypeMap
    {
        void Map(ModelBuilder builder);
    }
}
