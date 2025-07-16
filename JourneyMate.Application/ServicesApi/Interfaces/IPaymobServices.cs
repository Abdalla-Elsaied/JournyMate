using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Interfaces
{
    public interface IPaymobServices
    {
       
        Task<PaymentResponseDto> InitiatePaymentAsync(PaymentRequestDto request);
        Task<PaymentCallbackDto?> ProcessPaymentCallbackAsync(string payload,string HmacResevied);
    }
}
