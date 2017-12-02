using System;
using AutoMapper;
using Employees.Data;
using Employees.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.App
{
    internal class StartUp
    {
        private static void Main(string[] args)
        {

            //ResetDb();
            var serviceProvider = ConfigureServices();
            var engine = new Engine(serviceProvider);

            engine.Run();
        }

        private static void ResetDb()
        {
            using (var context = new EmployeeContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeeContext>(
                options => options.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<AutomapperProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}