// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ModularEfCore.Example.Test.Base
{
    public abstract class TransactionalTestBase
    {
        protected ServiceProvider services;
        protected TransactionScope transactionScope;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var serviceCollection = new ServiceCollection();

            RegisterDependencies(serviceCollection);

            services = serviceCollection.BuildServiceProvider();
        }

        [SetUp]
        public void Setup()
        {
            OnSetupBeforeTransaction();
            transactionScope = new TransactionScope();
            OnSetupInTransaction();
        }

        protected virtual void OnSetupBeforeTransaction() { }

        protected virtual void OnSetupInTransaction() { }

        [TearDown]
        public void Teardown()
        {
            OnTeardownInTransaction();
            transactionScope.Dispose();
            OnTeardownAfterTransaction();
        }

        protected virtual void OnTeardownInTransaction() { }

        protected virtual void OnTeardownAfterTransaction() { }

        protected abstract void RegisterDependencies(ServiceCollection services);
    }
}
