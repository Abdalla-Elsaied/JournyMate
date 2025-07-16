using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class BookingItemDto
    {
        public int TravelId { get; set; }
        public string Title { get; set; }
        public string TravelProfileUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
