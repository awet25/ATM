using ATMApp.Models;
using Microsoft.EntityFrameworkCore;




namespace ATMApp.Data{
public class ATMContext:DbContext
{



        public ATMContext(DbContextOptions<ATMContext> options) : base(options) { }

       public DbSet<User> User{get;set;}
        


    }
}