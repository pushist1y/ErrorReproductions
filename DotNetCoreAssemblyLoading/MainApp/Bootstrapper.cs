using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using AbstractionsLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MainApp
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
        }

        private readonly List<IDynamicModule> _modules = new List<IDynamicModule>();



        public void Initialize(IServiceCollection services, IConfigurationRoot config)
        {
            try
            {
                var i = 0;
                var paths = new List<string>();
                while (!string.IsNullOrEmpty(config[$"UnreferencedAssemblies:{i}"]))
                {
                    paths.Add(config[$"UnreferencedAssemblies:{i}"]);
                    i++;
                }

                var moduleTypes = new List<Type>();
                //moduleTypes.Add(typeof(Consanta.Data.Ef.Module));

                foreach (var path in paths)
                {
                    var fullPath = Path.Combine(System.AppContext.BaseDirectory, path);
                    if (!File.Exists(fullPath)) continue;

                    var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                    moduleTypes.AddRange(
                        asm.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IDynamicModule))));
                }


                //var loadedAsms =
                //    this.GetType()
                //        .GetTypeInfo()
                //        .Assembly.GetReferencedAssemblies()
                //        .Select(a => a.FullName)
                //        .Distinct()
                //        .OrderBy(s => s);

                //var refAsms = moduleTypes.Select(t => t.GetTypeInfo().Assembly)
                //    .Distinct()
                //    .SelectMany(a => a.GetReferencedAssemblies())
                //    .Select(a => a.FullName)
                //    .Distinct()
                //    .OrderBy(s => s);

                //var loadingAsms = refAsms.Where(s => !loadedAsms.Contains(s)).ToList();

                //AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Npgsql"));
                //foreach (var refAsm in loadingAsms)
                //{
                //    var n = new AssemblyName(refAsm);
                //    AssemblyLoadContext.Default.LoadFromAssemblyName(n);
                //}

                foreach (var moduleType in moduleTypes)
                {
                    var module = Activator.CreateInstance(moduleType) as IDynamicModule;
                    if (module == null) continue;
                    _modules.Add(module);
                    module.Register(services, config);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = e as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }
                throw;
            }
        }

        public void Startup(IServiceProvider serviceProvider, IConfigurationRoot config)
        {
            foreach (var module in _modules)
                module.Startup(serviceProvider, config);

        }
    }
}
