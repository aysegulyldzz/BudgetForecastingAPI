using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BudgetForecastingAPI.Data;
using BudgetForecastingAPI.Enums;
using BudgetForecastingAPI.Models;
using Microsoft.VisualBasic;


namespace BudgetForecastingAPI.Services.Providers
{
    public class DbBudgetDataProvider : IBudgetDataProvider
    {
        private readonly AppDbContext _context;
        public DataSourceType SourceType => DataSourceType.Database;
        
        public DbBudgetDataProvider(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HistoricalBudgetData>> GetHistoricalBudgetsAsync(string departmentName)
        {
            var data = await _context.DepartmentBudgets
                .Where(d => d.DepartmentName == departmentName)
                .OrderBy(d => d.Year)
                .Select(d => new HistoricalBudgetData { Year = d.Year, ActualSpent = d.ActualSpent }).ToListAsync();
            if(data == null || !data.Any())
            {
                throw new KeyNotFoundException($"Secilen departman ({departmentName}) icin veritabaninda gecmis veri bulunamadi.");

            }
            return data;
        }
    }
}
