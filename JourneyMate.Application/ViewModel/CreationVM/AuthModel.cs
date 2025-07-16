using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ViewModel.CreationVM
{
    public class AuthModel
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        public string? EmailorNumber { get; set; }
        public string? Token { get; set; }

    }
}
