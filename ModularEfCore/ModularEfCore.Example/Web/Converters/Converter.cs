// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using ModularEfCore.Example.Database.Model;
using ModularEfCore.Example.Web.DTO;

namespace ModularEfCore.Example.Web.Converters
{
    public static class Converter
    {
        public static CustomerDto Convert(Customer source)
        {
            if (source == null)
            {
                return null;
            }

            return new CustomerDto
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName
            };
        }

        public static IEnumerable<CustomerDto> Convert(IEnumerable<Customer> source)
        {
            if (source == null)
            {
                return null;
            }

            return source
                .Select(x => Convert(x));
        }
    }
}
