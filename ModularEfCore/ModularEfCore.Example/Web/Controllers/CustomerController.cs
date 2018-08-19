// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModularEfCore.Context;
using ModularEfCore.Example.Database.Model;
using ModularEfCore.Example.Web.Converters;
using ModularEfCore.Factory;

namespace ModularEfCore.Example.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IApplicationDbContextFactory dbContextFactory;

        public CustomerController(IApplicationDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id, CancellationToken cancellationToken)
        {
            using (var context = dbContextFactory.Create())
            {
                var customer = await context.DbSet<Customer>()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (customer == null)
                {
                    return NotFound();
                }

                var dtoCustomer = Converter.Convert(customer);

                return Ok(dtoCustomer);
            }
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers(CancellationToken cancellationToken)
        {
            using (var context = dbContextFactory.Create())
            {
                var customers = await context.DbSet<Customer>()
                    .ToListAsync(cancellationToken);

                var dtoCustomers = Converter.Convert(customers);

                return Ok(dtoCustomers);
            }
        }
    }
}
