using JourneyMate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.IServeces
{
    public interface IPasswordService
    {
        Task<bool> SendResetPasswordUrlAsync(string model, string resetPasswordUrl);
        Task<bool> ResetPasswordAsync(ResetPasswordDto model);


    }
}
