using JourneyMate.Core.Models.Booking_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.BookingSepcifications
{
    public class BookingSpec:BaseSpecification<Booking>
    {
        public BookingSpec(string BuyerEmail)
            :base(B=>B.BuyerEmail== BuyerEmail && B.Status!= BookingStatus.PaymentFailed)
        {
            AddOrderBy(B=>B.BookingDate);
            Includes.Add(B => B.Item);
        }
        public BookingSpec(int BookingId)
               : base(B=> B.Id == BookingId)
        {
            Includes.Add(B => B.Item);
            ThenIncludeStrings.Add("Item.TravelItemBooked");
        }
    }
}
