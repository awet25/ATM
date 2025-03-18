


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
        Console.WriteLine("Enter Id for the Account you want to delete");
        Id=Int32.Parse(Console.ReadLine());
        
        
        return await _adminServices.DeleteUserAndAccount(Id);
    }

public async Task<bool> UpdateAccount(){
   UpdateUserDto updateduser=HandleInputToUpudate();
   return await _adminServices.UpdateUser(updateduser);
}
public async Task SearchForAccount(){
    string stringId="";
    int id;
    Console.WriteLine("Enter Account ID to look for account ");
    stringId=Console.ReadLine();
    while(!int.TryParse(stringId, out id)||id<=0){
        Console.WriteLine("Invalid Input please enter a valid Id");
       stringId=Console.ReadLine(); 

    }
    var account= await _adminServices.GetAccount(id);
    if(account==null){
     Console.WriteLine("sorry account not found");
     return ;
    }
    Console.WriteLine("The Account information is :");
    Console.WriteLine($"Account # {account.Id}");
    Console.WriteLine($"Holder:  {account.User.HolderName}");
    Console.WriteLine($"Balance: {account.IntialBalance}");
    Console.WriteLine($"Status: {account.status}");
    Console.WriteLine($"Login: {account.User.Login}");
     Console.WriteLine($"Pin Code: {account.User.PinCode}");



}

public void Exit(){
_authService.Exit();
}
public static CreateUserDto HandleCreateUserInput(){

string login, pinCode, holderName;
    UserRole userRole;
    AccountStatus accountStatus;
    decimal balance=0;

   
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

    
 Console.WriteLine("Select Status of Account :");
    foreach (var stats in Enum.GetNames(typeof(AccountStatus)))
    {
        Console.WriteLine($"- {stats}");
    }

Console.Write("Enter status: ");
    string stringStatus = Console.ReadLine();
    while (!Enum.TryParse(stringStatus, out AccountStatus parseStatus) || !Enum.IsDefined(typeof(AccountStatus), parseStatus))
    {
        Console.WriteLine("Invalid choice. Please select a valid role:");
        stringStatus = Console.ReadLine();
    }
    accountStatus = (AccountStatus)Enum.Parse(typeof(AccountStatus), stringStatus);



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

      Console.WriteLine("Enter balance");
      string stringBalance= Console.ReadLine();
      while(!decimal.TryParse(stringBalance, out balance)||balance< 0)
      {
        Console.WriteLine("Enter balance");
        stringBalance = Console.ReadLine();
      }



    userRole = (UserRole)Enum.Parse(typeof(UserRole), stringRole);

    return new CreateUserDto
    {
        HolderName = holderName,
        Login = login,
        PinCode = pinCode,
        Role = userRole,
        status=accountStatus,
    };
   

}


public static UpdateUserDto HandleInputToUpudate()
{
    string login=null,pinCode=null,holderName=null;
    AccountStatus? accountStatus=null;
    int accountId=0;
     Console.Write("Enter Account ID to update: ");
    while (!int.TryParse(Console.ReadLine(), out accountId) || accountId <= 0)
    {
        Console.WriteLine("Invalid ID. Please enter a valid Account ID:");
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

    if (AskUserToEdit("Status"))
    {
        Console.WriteLine("Select Account status (Leave blank to keep current):");
        foreach (var status in Enum.GetNames(typeof(AccountStatus)))
        {
            Console.WriteLine($"- {status}");
        }

        Console.Write("Enter status: ");
        string stringStatus = Console.ReadLine();
        if (!string.IsNullOrEmpty(stringStatus))
        {
            while (!Enum.TryParse(stringStatus, out AccountStatus parsedStatus) || !Enum.IsDefined(typeof(AccountStatus), parsedStatus))
            {
                Console.WriteLine("Invalid choice. Please select a valid role:");
                stringStatus = Console.ReadLine();
            }
            accountStatus = (AccountStatus)Enum.Parse(typeof(AccountStatus), stringStatus);
        }
    }

    return new UpdateUserDto
    {
        Id = accountId,
        HolderName = string.IsNullOrEmpty(holderName) ? null : holderName,
        Login = string.IsNullOrEmpty(login) ? null : login,
        PinCode = string.IsNullOrEmpty(pinCode) ? null : pinCode,
        status = accountStatus ?? default
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