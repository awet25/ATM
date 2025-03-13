
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ATMApp.Data;
using Microsoft.Extensions.Hosting;
using ATMApp.Interfaces;
using ATMApp.Services;
using ATMApp.Repositories;
using System.Threading.Tasks;


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
        }).Build();
     





        var UserRepository = builder.Services.GetService<IUserRepository>();
        var authService=builder.Services.GetService<IAuthService>();

        Console.WriteLine("welcome to our ATM system please Login to use our ATM");
        Console.Write("enter your login: ");

         //add this to check for extra characters
       

        string login=Console.ReadLine();

        Console.Write("enter your pincode: ");

      //add this to check for extra characters
       

       string pinCode=Console.ReadLine();

       Console.WriteLine($"{login},{pinCode}");
        bool isAuthenticated= await authService.Login(login, pinCode);
        if(isAuthenticated)
        {
            Console.WriteLine("welcome to our atm");
        }
else{
    Console.WriteLine("Exiting the system....");
}

       Console.ReadLine();
    }
}
