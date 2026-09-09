using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace challenge.IntegrationTests.FactoryFixture
{
    public class ApiFactoryFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(Services =>
            {
                
            });
        }
    }

    [CollectionDefinition("ApiCollection")]
    public class ApiColletion : ICollectionFixture<ApiFactoryFixture>
    {

    }
}
