using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class PaymentMethodDto
    {
        public string? Type { get; set; }
        public string ?Provider { get; set; }
    }
}
