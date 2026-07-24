using System.ComponentModel.DataAnnotations;

namespace BudgetForecastingAPI.Models
{
    public class DepartmentBudget
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; } = string.Empty;

        public int Year { get; set; }

        // O yıl departmana ayrılan hedef bütçe
        public decimal AllocatedBudget { get; set; }

        // O yıl departmanın gerçekten harcadığı miktar
        public decimal ActualSpent { get; set; }
    }
}