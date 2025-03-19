using ATMApp.Interfaces;
using ATMApp.Models;
using ATMApp.Services;

namespace ATMApp.Views{
    public class ClientView
    {
        private readonly IClientService _clientService;
        private readonly IAuthService _authService;
        public ClientView(IClientService clientService, IAuthService authService)
        {
            _clientService=clientService;
            _authService=authService;
        }
           public async Task Show(User user)
    {
        while (true)
        {
              {
                Console.WriteLine("Client Menu:");
                Console.WriteLine("1 - Withdraw cash ");
                Console.WriteLine("2 - Deposit Cash");
                Console.WriteLine("3 - Display Balance");
                Console.WriteLine("4 - Exit");
                Console.WriteLine("Enter one of the above options");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("you have choosen to Withdraw Money");
                        await WithdrawMoney(user);
                        break;
                    case 2:
                        Console.WriteLine("you have choose to Deposite Money");
                        await Deposite(user);
                        break;
                    case 3:
                        Console.WriteLine("You have choose to View you money");
                        await DisplayAccount(user);
                        break;
                    
                    case 4:
                        Console.WriteLine("Exiting Client menu...");
                        Exit();
                        
                        return;
                    
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
        }
    }
        }


       public async Task DisplayAccount(User user)
       {  
     
        await _clientService.GetBalance(user.Id);
       }

      public async Task WithdrawMoney(User user)

      {

        

        decimal amount;
        Console.WriteLine("Enter the amount you want to withdraw");
        string input=Console.ReadLine();

       while (!decimal.TryParse(input, out amount)|| amount<=0)
    {
        Console.WriteLine("Invalid input please enter a valid amount greater than 0.");
        input=Console.ReadLine();
    }

         await _clientService.Withdraw(user.Id,amount);
      }

     
       public async Task Deposite(User user)
       {
        Console.WriteLine("Enter amount you want to deposite");
        decimal amount=decimal.Parse(Console.ReadLine());

        await _clientService.Deposit(user.Id, amount);
       }

       public void Exit()
       {
        _authService.Exit();
       }


    }
}