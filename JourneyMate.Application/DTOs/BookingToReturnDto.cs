using JourneyMate.Core.Models.Booking_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class BookingToReturnDto
    {
        public int id {  get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public string Status { get; set; }
        public BookingItemDto BookingItem {get;set;}
        public decimal TotalCost { get; set; }
        public string PaymentIntentId { get; set; } = " ";
    }
}
