using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ATMApp.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        var builder=Host.CreateDefaultBuilder().ConfigureServices((context,service)=>
        {
         var configuration=new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json").Build();


         service.AddDbContext<ATMContext>(options=>
         options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
         new MySqlServerVersion(new Version(8,0,41))));
        }).Build();

        Console.WriteLine("Database configuration is set up");
    }
}
