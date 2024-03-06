using FetchFromPublicApis_FunctionApp.Services.BlobService;
using FetchFromPublicApis_FunctionApp.Services.TableService;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FetchFromPublicApis_FunctionApp.Startup))]

namespace FetchFromPublicApis_FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddSingleton<IBlobService, BlobService>();
            builder.Services.AddSingleton<ITableService, TableService>();
        }
    }
}
