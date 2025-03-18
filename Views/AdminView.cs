


using ATMApp.DTOs;
using ATMApp.Interfaces;
using ATMApp.Models;


namespace ATMApp.Views
{

public class AdminView
{
    private readonly IAdminservices _adminServices;
    private readonly IAuthService _authService;

    public AdminView(IAdminservices adminServices,IAuthService authService){
        _adminServices = adminServices;
        _authService=authService;
    }
    public async Task Show()
    {
        while (true)
        {
              {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("1 - Create New  user ");
                Console.WriteLine("2 - Delete Existing Account");
                Console.WriteLine("3 - Update Account info ");
                Console.WriteLine("4 - Search for Account ");
                Console.WriteLine("5 - Exit");
                Console.WriteLine("Enter your choice");
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
                        Console.WriteLine("Follow instruction to updateUser info");
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
   CreateUserDto newuser=HandleCreateUserInput();
    return await _adminServices.AddUser(newuser);
    
    }

    public async Task<bool> DeleteAccount(){
        int Id;
        Console.WriteLine("Enter Id for the client you want to delete");
        Id=Int32.Parse(Console.ReadLine());
        
        
        return await _adminServices.DeleteUserAndAccount(Id);
    }

public async Task<bool> UpdateAccount(){
   UpdateUserDto updateduser=HandleInputToUpudate();
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
public static CreateUserDto HandleCreateUserInput(){

string login, pinCode, holderName;
    UserRole userRole;
   
 Console.Write("Enter account Holder Name: ");
    holderName = Console.ReadLine();
    while (string.IsNullOrEmpty(holderName))
    {
        Console.WriteLine("Holder name is required. Try again:");
        holderName = Console.ReadLine();
    }

    Console.Write("Enter login for account: ");
    login = Console.ReadLine();
    while (string.IsNullOrEmpty(login))
    {
        Console.WriteLine("Login is required. Try again:");
        login = Console.ReadLine();
    }

    Console.Write("Enter PinCode for account (Must be 5 digits): ");
    pinCode = Console.ReadLine();
    while (string.IsNullOrEmpty(pinCode) || pinCode.Length != 5)
    {
        Console.WriteLine("PinCode must be exactly 5 digits. Try again:");
        pinCode = Console.ReadLine();
    }

    Console.WriteLine("Select Account Role:");
    foreach (var role in Enum.GetNames(typeof(UserRole)))
    {
        Console.WriteLine($"- {role}");
    }

    Console.Write("Enter role: ");
    string stringRole = Console.ReadLine();
    while (!Enum.TryParse(stringRole, out UserRole parsedRole) || !Enum.IsDefined(typeof(UserRole), parsedRole))
    {
        Console.WriteLine("Invalid choice. Please select a valid role:");
        stringRole = Console.ReadLine();
    }
    userRole = (UserRole)Enum.Parse(typeof(UserRole), stringRole);

    return new CreateUserDto
    {
        HolderName = holderName,
        Login = login,
        PinCode = pinCode,
        Role = userRole
    };
   

}


public static UpdateUserDto HandleInputToUpudate()
{
    string login=null,pinCode=null,holderName=null;
    UserRole? userRole=null;
    int clientId=0;
     Console.Write("Enter Client ID to update: ");
    while (!int.TryParse(Console.ReadLine(), out clientId) || clientId <= 0)
    {
        Console.WriteLine("Invalid ID. Please enter a valid Client ID:");
    }

    if (AskUserToEdit("Holder Name"))
    {
        Console.Write("Enter account Holder Name (Leave blank to keep current): ");
        holderName = Console.ReadLine();
    }

    if (AskUserToEdit("Login"))
    {
        Console.Write("Enter login for account (Leave blank to keep current): ");
        login = Console.ReadLine();
    }

    if (AskUserToEdit("PinCode"))
    {
        Console.Write("Enter PinCode for account (Must be 5 characters, leave blank to keep current): ");
        pinCode = Console.ReadLine();

        while (!string.IsNullOrEmpty(pinCode) && pinCode.Length != 5)
        {
            Console.WriteLine("Invalid PinCode! It must be exactly 5 characters. Try again or leave blank:");
            pinCode = Console.ReadLine();
        }
    }

    if (AskUserToEdit("Role"))
    {
        Console.WriteLine("Select Account Role (Leave blank to keep current):");
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
                Console.WriteLine("Invalid choice. Please select a valid role:");
                stringRole = Console.ReadLine();
            }
            userRole = (UserRole)Enum.Parse(typeof(UserRole), stringRole);
        }
    }

    return new UpdateUserDto
    {
        ClientId = clientId,
        HolderName = string.IsNullOrEmpty(holderName) ? null : holderName,
        Login = string.IsNullOrEmpty(login) ? null : login,
        PinCode = string.IsNullOrEmpty(pinCode) ? null : pinCode,
        Role = userRole ?? default(UserRole)
    };
}






private static bool AskUserToEdit(string field)
{
    Console.Write($"Do you want to update {field}? (yes/no): ");
    string response = Console.ReadLine()?.Trim().ToLower();
    return response == "yes" || response == "y";
}
    }
    }