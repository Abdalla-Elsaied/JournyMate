using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models.Booking_Aggregation
{
    public enum BookingStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Successed")]
        PaymentSuccessed,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,
    }
}
