
namespace JourneyMate.Core.Models.Company_Aggregation
{
    public class PaymentMethod : ModelBase
    {
        public int CompanyId { get; set; } // FK
        public Company? Company { get; set; } // Navigation property
        public string? Type { get; set; } // e.g., "Credit Card"
        public string? Provider { get; set; } // e.g., "Visa" //link gat way
    }
}