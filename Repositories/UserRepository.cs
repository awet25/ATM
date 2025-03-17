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
 public async Task<User> GetUserById(int id)
 {
   return await _context.User.FindAsync(id);
 }
 public async Task<User> AddUser(User user)
 {
    var createdUser = await _context.User.AddAsync(user);
    await _context.SaveChangesAsync();
    return createdUser.Entity;
 }
 public async Task<bool>DeleteUserbyId (int id)
 {
   var user= await GetUserById(id);
   if (user == null) return false;
   _context.User.Remove(user);
   await _context.SaveChangesAsync();
   return true;
 }

public async  Task<bool> UpdateUser(User user)
{
  var existingUser =await  _context.User.FindAsync(user.Id);
  if (existingUser == null) return false;
  _context.Entry(existingUser).CurrentValues.SetValues(user);
  await _context.SaveChangesAsync();
  return true;
}

}

}