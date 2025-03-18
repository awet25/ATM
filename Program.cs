
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
        try
        {
            var builder = Host.CreateDefaultBuilder().ConfigureServices((context, service) =>
            {
                service.AddDbContext<ATMContext>(options =>
                    options.UseMySql("server=localhost;database=atmdb;user=root;password=9361;",
                    new MySqlServerVersion(new Version(8, 0, 41))));

                service.AddScoped<IAuthService, AuthService>();
                service.AddScoped<IUserRepository, UserRepository>();
                service.AddScoped<IAccountRepository, AccountRepository>();
                service.AddScoped<ITransactionRepository, TransactionRepository>();
                service.AddScoped<IClientService, ClientService>();
                service.AddValidatorsFromAssemblyContaining<AddNewuserValidator>();
                service.AddScoped<IValidator<CreateUserDto>, AddNewuserValidator>();
                service.AddScoped<IAdminservices, AdminServices>();
                service.AddScoped<AdminView>();
                service.AddScoped<ClientView>();
            }).Build();

            using (var scope = builder.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var authService = serviceProvider.GetRequiredService<IAuthService>();
                var adminView = serviceProvider.GetRequiredService<AdminView>();
                var clientView = serviceProvider.GetRequiredService<ClientView>();

                Console.WriteLine("Welcome to our ATM system!");

                
                while (true)
                {
                    Console.Write("Enter your login: ");
                    string login = Console.ReadLine();

                    Console.Write("Enter your pincode: ");
                    string pinCode = Console.ReadLine();

                    var userLoginDto = new UserLoginDTO
                    {
                        Login = login,
                        PinCode = pinCode
                    };

                    User isAuthenticated = await authService.Login(userLoginDto);

                    if (isAuthenticated == null)
                    {
                        Console.WriteLine("Invalid credentials. Please try again.");
                        continue;
                    }

                    if (isAuthenticated.Role == UserRole.Admin)
                    {
                        Console.WriteLine($"Welcome Admin {isAuthenticated.HolderName}!");
                        await adminView.Show();
                    }
                    else if (isAuthenticated.Role == UserRole.Client)
                    {
                        Console.WriteLine($"Welcome Client {isAuthenticated.HolderName}!");
                        await clientView.Show(isAuthenticated);
                    }

                    // 🛑 If user logs out, restart login loop
                    Console.WriteLine("Returning to login screen...\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
