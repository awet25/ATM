using ATMApp.Models;
using Microsoft.EntityFrameworkCore;




namespace ATMApp.Data{
public class ATMContext:DbContext
{



        public ATMContext(DbContextOptions<ATMContext> options) : base(options) { }

       public DbSet<User> User{get;set;}
       public DbSet<Account>Account{get;set;}
       public DbSet<Transaction>Transactions{get;set;}

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
         modelBuilder.Entity<Account>()
         .HasOne(a=>a.User)
         .WithOne(u=>u.Account)
         .HasForeignKey<Account>(a=>a.ClientID)
         .OnDelete(DeleteBehavior.Cascade);

         modelBuilder.Entity<Transaction>()
         .HasOne(t=>t.Account)
         .WithMany(a=>a.Transactions)
         .HasForeignKey(t=>t.AccountId)
         .OnDelete(DeleteBehavior.Cascade);
       }
        


    }
}