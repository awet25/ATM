﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ATMApp.Data;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using ATMApp.Interfaces;
using ATMApp.Services;
using ATMApp.Repositories;
using System.Threading.Tasks;
using ATMApp.DTOs;
using ATMApp.Validators;
using FluentValidation;
using ATMApp.Models;
using ATMApp.Views;


class Program
{
    static async Task Main(string[] args)
    {
        var builder=Host.CreateDefaultBuilder().ConfigureServices((context,service)=>
        {
      


         service.AddDbContext<ATMContext>(options=>
         options.UseMySql("server=localhost;database=atmdb;user=root;password=9361;",
         new MySqlServerVersion(new Version(8,0,41))));
         service.AddScoped<IAuthService,AuthService>();
         service.AddScoped<IUserRepository,UserRepository>();
         service.AddScoped<IAccountRepository,AccountRepository>();
         service.AddScoped<ITransactionRepository,TransactionRepository>();
         service.AddScoped<ITransactionService, TransactionService>();
         service.AddValidatorsFromAssemblyContaining<UserLoginValidator>();
         service.AddScoped<IAdminservices,AdminServices>();
         service.AddScoped<AdminView>();
        }).Build();
     



            using(var scope=builder.Services.CreateScope()){
            var serviceProvider=scope.ServiceProvider;
            var  authService=serviceProvider.GetRequiredService<IAuthService>();
            var adminView=serviceProvider.GetRequiredService<AdminView>();
            Console.WriteLine("welcome to our ATM system please Login to use our ATM");
            Console.Write("enter your login: ");

         
       

        string login=Console.ReadLine();

        Console.Write("enter your pincode: ");
       

       string pinCode=Console.ReadLine();


        var userLoginDto= new UserLoginDTO{
            Login=login,
            PinCode=pinCode
        };         

        User isAuthenticated= await authService.Login(userLoginDto);
        if(isAuthenticated.Role==UserRole.Admin)
        {
            Console.WriteLine($"welcome to our ATM Admin {isAuthenticated.HolderName}");
            await adminView.show();
        }
else if(isAuthenticated.Role==UserRole.Client){
    Console.WriteLine($"Welcome client{isAuthenticated.HolderName}");
}
else{
    Console.WriteLine("exiting .....");
}
        }

       

       

       Console.ReadLine();
    }
}
