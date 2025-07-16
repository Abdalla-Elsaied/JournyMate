using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models.Booking_Aggregation
{
    public class BookingItem
    {
        public BookingItem() { }
        public BookingItem(TravelItemBooked travelItemBooked, decimal price, int quantity)
        {
            TravelItemBooked = travelItemBooked;
            Price = price;
            Quantity = quantity;
        }
        public TravelItemBooked TravelItemBooked { get; set; }   //just for clean code 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
