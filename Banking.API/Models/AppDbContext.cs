using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
                options.UseSqlServer(@"data source = .\SQLEXPRESS;initial catalog = Project3DB;integrated security =True;MultipleActiveResultSets=True;");
         
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
