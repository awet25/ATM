

using System.Threading.Tasks;
using ATMApp.DTOs;
using ATMApp.Models;
using ATMApp.Services;

namespace ATMApp.Views
{

public class AdminView
{
    private readonly AdminServices _adminServices;
    private readonly AuthService _authService;

    public AdminView(AdminServices adminServices,AuthService authService){
        _adminServices = adminServices;
        _authService=authService;
    }
    public async Task show()
    {
        while (true)
        {
              {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("1 - Create New  user ");
                Console.WriteLine("2 - Delete Existing Account");
                Console.WriteLine("3 - Update Account info ");
                Console.WriteLine("4 - Search for Account ");
                Console.Write("\n 5 - Exit");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("you have choosen to create new account");
                       await CreateUser();
                        break;
                    case 2:
                        Console.WriteLine("follow instructions to delete Account");
                        await DeleteAccount();
                        break;
                    case 3:
                        Console.WriteLine("Follow instruction to updade info");
                        await UpdateAccount();
                        break;
                    case 4:
                        Console.WriteLine("Follow instruction to search for an account");
                        await SearchForAccount();
                        break;
                    case 5:
                        Console.WriteLine("Exiting Admin menu...");
                        Exit();
                        
                        return;
                    
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
        }
    }
}

    public  async Task<bool> CreateUser()
    {
   CreateUserDto newuser=HandleInputFromUser<CreateUserDto>();
    return await _adminServices.AddUser(newuser);
    
    }

    public async Task<bool> DeleteAccount(){
        int Id;
        Console.WriteLine("Enter Id for the client you want to delete");
        Id=Int32.Parse(Console.ReadLine());
        
        
        return await _adminServices.DeleteUserAndAccount(Id);
    }

public async Task<bool> UpdateAccount(){
   UpdateUserDto updateduser=HandleInputFromUser<UpdateUserDto>();
   return await _adminServices.UpdateUser(updateduser);
}
public async Task SearchForAccount(){
    string login="";
    Console.WriteLine("Enter Login to look for user and account useing login");
    login=Console.ReadLine();
    var user= await _adminServices.GetUserByLogin(login);
    if(user==null){
     Console.WriteLine("sorry account not found");
     
    }
    Console.WriteLine("+----------------+----------------+---------+---------------+----------------+------------+");
    Console.WriteLine("| Holder Name    | Login          | Role    | Account No    | Balance        | Status     |");
    Console.WriteLine("+----------------+----------------+---------+---------------+----------------+------------+");

  
    Console.WriteLine($"| {user.HolderName,-14} | {user.Login,-14} | {user.Role,-7} | " +
                      $"{(user.Account != null ? user.Account.Id : "N/A"),-13} | " +
                      $"{(user.Account != null ? user.Account.IntialBalance.ToString("C") : "N/A"),-14} | " +
                      $"{(user.Account != null ? user.Account.status.ToString() : "N/A"),-10} |");

    Console.WriteLine("+----------------+----------------+---------+---------------+----------------+------------+");

}

public void Exit(){
_authService.Exit();
}
public static T HandleInputFromUser<T>(bool isUpdate=false)where T:BaseDto, new()  {

    string login = null, pinCode = null, holderName = null;
    UserRole? userRole = null; 
    int clientId=0;

    Console.WriteLine("Enter account Holder Name" + (isUpdate ? " (Leave blank to skip):" : ":"));
    holderName = Console.ReadLine();
    while (!isUpdate && string.IsNullOrEmpty(holderName)) // Required for CreateUserDto
    {
        Console.WriteLine("Holder name is required. Try again:");
        holderName = Console.ReadLine();
    }

    Console.WriteLine("Enter login for account" + (isUpdate ? " (Leave blank to skip):" : ":"));
    login = Console.ReadLine();
    while (!isUpdate && string.IsNullOrEmpty(login)) // Required for CreateUserDto
    {
        Console.WriteLine("Login is required. Try again:");
        login = Console.ReadLine();
    }

    Console.WriteLine("Enter PinCode for account (Must be 5 characters" + (isUpdate ? ", leave blank to skip" : "") + "):");
    pinCode = Console.ReadLine();
    while (!isUpdate && (string.IsNullOrEmpty(pinCode) || pinCode.Length != 5)) // Required for CreateUserDto
    {
        Console.WriteLine("PinCode is required and must be exactly 5 characters. Try again:");
        pinCode = Console.ReadLine();
    }
    if (isUpdate && !string.IsNullOrEmpty(pinCode) && pinCode.Length != 5)
    {
        Console.WriteLine("Invalid PinCode! It must be exactly 5 characters.");
        pinCode = null;
    }

    Console.WriteLine("Select Account Role" + (isUpdate ? " (Leave blank to skip)" : ""));
    foreach (var role in Enum.GetNames(typeof(UserRole)))
    {
        Console.WriteLine($"- {role}");
    }

    Console.Write("Enter role: ");
    string stringRole = Console.ReadLine();
    if (!string.IsNullOrEmpty(stringRole))
    {
        while (!Enum.TryParse(stringRole, out UserRole parsedRole) || !Enum.IsDefined(typeof(UserRole), parsedRole))
        {
            Console.WriteLine("Invalid choice. Please select a valid role.");
            stringRole = Console.ReadLine();
        }
        userRole = (UserRole)Enum.Parse(typeof(UserRole), stringRole);
    }

    var userDto = new T
    {
        HolderName = isUpdate && string.IsNullOrEmpty(holderName) ? null : holderName,
        Login = isUpdate && string.IsNullOrEmpty(login) ? null : login,
        PinCode = isUpdate && string.IsNullOrEmpty(pinCode) ? null : pinCode,
        Role =userRole ??default(UserRole) 
    };

    if (isUpdate && userDto is UpdateUserDto updateDto)
    {
        Console.WriteLine("Enter Client ID to update:");
        while (!int.TryParse(Console.ReadLine(), out  clientId) || clientId <= 0)
        {
            Console.WriteLine("Invalid ID. Please enter a valid Client ID:");
        }
        updateDto.ClientId = clientId;
    }

    return userDto;

}
    }
    }