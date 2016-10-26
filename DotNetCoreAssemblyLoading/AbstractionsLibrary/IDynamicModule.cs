using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbstractionsLibrary
{
    public interface IDynamicModule
    {
        void Register(IServiceCollection services, IConfigurationRoot config);
        void Startup(IServiceProvider serviceProvider, IConfigurationRoot config);
    }
}
