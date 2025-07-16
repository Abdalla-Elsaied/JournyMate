using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class PaymentCallbackDto
    {
        public bool Success { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string RawData { get; set; }
    }
}
