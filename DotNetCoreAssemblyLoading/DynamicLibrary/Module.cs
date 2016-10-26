using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbstractionsLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DynamicLibrary
{
    public class Module: IDynamicModule
    {
        public void Register(IServiceCollection services, IConfigurationRoot config)
        {
            var str = config.GetConnectionString("Main");
            services.AddDbContext<CommonContext>(options => options.UseNpgsql(str));
        }

        public void Startup(IServiceProvider serviceProvider, IConfigurationRoot config)
        {
            var context = serviceProvider.GetService<CommonContext>();
            var user = new User {Name = "admin", Password = "admin"};
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
