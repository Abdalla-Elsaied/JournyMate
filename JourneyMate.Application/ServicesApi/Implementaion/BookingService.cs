using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications.BookingSepcifications;
using JourneyMate.Core.Interfaces;
using JourneyMate.Core.Models.Booking_Aggregation;
using Microsoft.AspNetCore.Identity;
using JourneyMate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public BookingService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Booking?> CreateBookingAsync(string buyerEmail, BookingDto bookingDto)
        {
            // Validate the booking DTO
            if (!ValidateBookingDto(bookingDto))
                return null;

            var travel = await _unitOfWork.Repository<Travel>().GetByIdAsync(bookingDto.TravelId);

            if (travel == null || travel.AvailableSeats < bookingDto.TotalQuantity)
                return null;  // or throw business exception

            // Update user's phone number if provided
            await UpdateUserPhoneNumberAsync(buyerEmail, bookingDto.PhoneNumber);

            // Create TravelItemBooked (immutable snapshot of travel info)
            var travelItem = new TravelItemBooked(travel.Id, travel.Title, travel.CoverImageUrl);

            // Calculate total cost: TotalQuantity pays full price, ChildrenUnderFive pay half price
            var totalCost = CalculateTotalCost(travel.Price, bookingDto.TotalQuantity, bookingDto.ChildrenUnderFiveNum);

            var bookingItem = new BookingItem(travelItem, travel.Price, bookingDto.TotalQuantity);

            var booking = new Booking(buyerEmail, bookingItem, totalCost);

            await _unitOfWork.Repository<Booking>().AddAsync(booking);

            var result = await _unitOfWork.CompleteAsync();

            return result <= 0 ? null : booking;
        }

        public async Task<IReadOnlyCollection<Booking>?> GetBookingForUserById(string buyerEmail)
        {
            var bookingSpec = new BookingSpec(buyerEmail);
            var bookforUser = await _unitOfWork.Repository<Booking>().GetAllWithSpecificaiton(bookingSpec);
            if (bookforUser == null) return null;
            return bookforUser;
        }

        public async Task<bool> ConfirmBookingPaymentAsync(int bookingId)
        {
            var spec = new BookingSpec(bookingId);
            var booking = await _unitOfWork.Repository<Booking>().GetByIdWithSpecifications(spec);
            if (booking == null || booking.Status != BookingStatus.Pending)
                return false;

            var travel = await _unitOfWork.Repository<Travel>().GetByIdAsync(booking.Item.TravelItemBooked.TravelId);

            if (travel == null || travel.AvailableSeats < booking.Item.Quantity)
            {
                booking.Status = BookingStatus.PaymentFailed;
                _unitOfWork.Repository<Booking>().Update(booking);
                await _unitOfWork.CompleteAsync();
                return false;
            }

            travel.AvailableSeats -= booking.Item.Quantity;
            _unitOfWork.Repository<Travel>().Update(travel);

            booking.Status = BookingStatus.PaymentSuccessed;
            _unitOfWork.Repository<Booking>().Update(booking);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<Booking?> UpdateBookingAsync(int id, string buyerEmail, BookingDto bookingDto)
        {
            // Validate the booking DTO
            if (!ValidateBookingDto(bookingDto))
                return null;

            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);

            if (booking == null || booking.BuyerEmail != buyerEmail)
                return null;

            var travel = await _unitOfWork.Repository<Travel>().GetByIdAsync(bookingDto.TravelId);
            if (travel == null || travel.AvailableSeats < bookingDto.TotalQuantity)
                return null;

            // Update user's phone number if provided
            await UpdateUserPhoneNumberAsync(buyerEmail, bookingDto.PhoneNumber);

            // Create new snapshot of the travel item
            var travelItem = new TravelItemBooked(travel.Id, travel.Title, travel.CoverImageUrl);

            // Calculate total cost with new pricing logic
            var totalCost = CalculateTotalCost(travel.Price, bookingDto.TotalQuantity, bookingDto.ChildrenUnderFiveNum);

            var bookingItem = new BookingItem(travelItem, travel.Price, bookingDto.TotalQuantity);

            // Update booking
            booking.Item = bookingItem;
            booking.TotalCost = totalCost;
            booking.BookingDate = DateTimeOffset.UtcNow; // Update date if needed
            booking.Status = BookingStatus.Pending;

            _unitOfWork.Repository<Booking>().Update(booking);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? booking : null;
        }

        public async Task<bool> DeleteBookingAsync(int id, string buyerEmail)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);

            if (booking == null || booking.BuyerEmail != buyerEmail)
                return false;

            _unitOfWork.Repository<Booking>().Delete(booking);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        #region Private Helper Methods

        private bool ValidateBookingDto(BookingDto bookingDto)
        {
            // Validate that TotalQuantity is at least 1
            if (bookingDto.TotalQuantity < 1)
                return false;

            // Validate that ChildrenUnderFiveNum is not greater than TotalQuantity
            if (bookingDto.ChildrenUnderFiveNum > bookingDto.TotalQuantity)
                return false;

            // Validate that National ID is not empty
            if (string.IsNullOrWhiteSpace(bookingDto.NationalId))
                return false;

            // Validate that Phone Number is not empty
            if (string.IsNullOrWhiteSpace(bookingDto.PhoneNumber))
                return false;

            return true;
        }

        private decimal CalculateTotalCost(decimal pricePerPerson, int totalQuantity, int childrenUnderFiveNum)
        {
            // TotalQuantity pays full price
            var fullPricePassengers = totalQuantity;

            // Children under 5 pay half price
            var halfPricePassengers = childrenUnderFiveNum;

            // Calculate total cost
            var fullPriceCost = pricePerPerson * fullPricePassengers;
            var halfPriceCost = (pricePerPerson * halfPricePassengers) / 2;

            return fullPriceCost + halfPriceCost;
        }

        private async Task UpdateUserPhoneNumberAsync(string userEmail, string phoneNumber)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user != null && user.PhoneNumber != phoneNumber)
                {
                    user.PhoneNumber = phoneNumber;
                    await _userManager.UpdateAsync(user);
                }
            }
            catch (Exception)
            {
                // Log the exception but don't fail the booking process
                // You might want to add proper logging here
            }
        }

        #endregion
    }
}