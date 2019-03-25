using AutoMapper;
using StARKS.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StARKS.Application.Test.Services
{
    [CollectionDefinition("ServiceCollection")]
    public class ServiceCollectionFixture : ICollectionFixture<ServiceCollectionFixture>, IDisposable
    {
        public AutoMapperFixture AutoMapperFixture { get; }

        public ServiceCollectionFixture()
        {
            this.AutoMapperFixture = new AutoMapperFixture();
        }

        public void Dispose()
        {
            this.AutoMapperFixture.Instance = null;
        }
    }
}
