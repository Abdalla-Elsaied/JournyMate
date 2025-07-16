using System.ComponentModel.DataAnnotations.Schema;
using JourneyMate.Core.Models.Booking_Aggregation;
using JourneyMate.Core.Models.Company_Aggregation;

namespace JourneyMate.Core.Models;

public class Booking : ModelBase
{
    public Booking()
    {
        
    }
    public Booking(string buyerEmail, BookingItem item, decimal totalCost)
    {
        BuyerEmail = buyerEmail;
        Item = item;
        TotalCost = totalCost;
    }

    public string BuyerEmail { get; set; }
    public DateTimeOffset BookingDate { get; set; } = DateTimeOffset.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public BookingItem Item { get; set; }   //will  be 1 - 1 relationship  own   >> map in the same table 
    public decimal TotalCost { get; set; }
    public string PaymentIntentId { get; set; } = " ";

}
