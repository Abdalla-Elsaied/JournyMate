using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> CreateBookingAsync(string buyerEmail, BookingDto bookingDto);

        Task<IReadOnlyCollection<Booking>?> GetBookingForUserById(string buyerEmail);

        Task<Booking?> UpdateBookingAsync(int id, string buyerEmail, BookingDto bookingDto);
        Task<bool> DeleteBookingAsync(int id, string buyerEmail);

        Task<bool> ConfirmBookingPaymentAsync(int bookingId);
    }
}
