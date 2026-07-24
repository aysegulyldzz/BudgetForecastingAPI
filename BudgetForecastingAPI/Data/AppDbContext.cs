using Microsoft.EntityFrameworkCore;
using BudgetForecastingAPI.Models;

namespace BudgetForecastingAPI.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor: Veritabanı bağlantı ayarlarını (örneğin SQLite yolunu) Program.cs'den alıp temel sınıfa (base) iletir.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // C# sınıflarımızı veritabanındaki tablolara dönüştüren DbSet tanımlamaları
        public DbSet<DepartmentBudget> DepartmentBudgets { get; set; }
        public DbSet<EconomicIndicator> EconomicIndicators { get; set; }
    }
}