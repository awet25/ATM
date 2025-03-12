using ATMApp.Models;

namespace ATMApp.Interfaces
{
    
 public   interface Iuser{
    void AddUser(User user);
    List<User>GetAllPeople();
 }
}