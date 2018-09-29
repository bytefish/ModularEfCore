// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using ModularEfCore.Context;
using ModularEfCore.Factory;
using NUnit.Framework;

namespace ModularEfCore.Example.Test.Base
{
    public abstract class AsyncTransactionalTestBase
    {
        protected ServiceProvider services;
        protected ApplicationDbContext context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var serviceCollection = new ServiceCollection();

            RegisterDependencies(serviceCollection);

            services = serviceCollection.BuildServiceProvider();
        }

        [SetUp]
        public async Task Setup()
        {
            context = services.GetService<IApplicationDbContextFactory>().Create();

            await OnSetupBeforeTransactionAsync();
            
            await context.Database.BeginTransactionAsync();

            await OnSetupInTransactionAsync();
        }

        protected virtual Task OnSetupBeforeTransactionAsync()
        {
            return Task.FromResult(true);
        }

        protected virtual Task OnSetupInTransactionAsync()
        {
            return Task.FromResult(true);
        }

        [TearDown]
        public async Task Teardown()
        {
            await OnTeardownInTransactionAsync();

            context.Database.RollbackTransaction();

            await OnTeardownAfterTransactionAsync();

            context.Dispose();
        }

        protected virtual Task OnTeardownInTransactionAsync()
        {
            return Task.FromResult(true);
        }

        protected virtual Task OnTeardownAfterTransactionAsync()
        {
            return Task.FromResult(true);
        }

        protected abstract void RegisterDependencies(ServiceCollection services);
    }
}
