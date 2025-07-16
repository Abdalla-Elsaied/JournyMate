using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class PaymentRequestDto
    {
        [Required]
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
     
    }
}
