using ATMApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using ATMApp.Data;
using ATMApp.Models;
using Org.BouncyCastle.Security;
namespace  ATMApp.Repositories
{

 public class UserRepository:IUserRepository
 {
   private readonly ATMContext _context;
   public UserRepository(ATMContext context)
   {
      _context=context;
   }
   public async Task<User>GetUserBylogin(string login)
   {
    return await _context.User.FirstOrDefaultAsync(x=>x.Login == login);
   }
 
 public async Task AddUser(User user)
 {
    await _context.User.AddAsync(user);
    await _context.SaveChangesAsync();
 }
}
}