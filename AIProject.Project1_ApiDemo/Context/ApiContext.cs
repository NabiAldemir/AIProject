using AIProject.Project1_ApiDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIProject.Project1_ApiDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Nabi ;initial catalog = ApiAIDb ; integrated security = true; trustservercertificate=true;");
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
