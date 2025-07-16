using JourneyMate.Application.DTOs;
using JourneyMate.Application.Setting;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Text.Encodings.Web;


namespace JourneyMate.Application.Servieces;

public class PasswordService : IPasswordService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly EmailSetting _emailSetting;
    public PasswordService(
                            UserManager<AppUser> userManger
                            ,EmailSetting emailSetting
)
    {
        _userManager = userManger;

        _emailSetting = emailSetting;
    }

    public async Task<bool> SendResetPasswordUrlAsync( string userEmail , string resetPasswordUrl)
    {
        
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = Uri.EscapeDataString(token);
        //https://your-frontend.com/reset-password?email=
        var resetUrl = $"{resetPasswordUrl}{userEmail}&token={encodedToken}";

        // send email

        var email = new Email
        {
            Subject = "Reset Your Password",
            To = userEmail,
            Body = $@"<p>Click the button below to reset your password:</p><a href='{HtmlEncoder.Default.Encode(resetUrl)}'style='display:inline-block;
          padding:10px 20px;
          font-size:16px;
          color:white;
          background-color:#007bff;
          text-decoration:none;
          border-radius:5px;
          margin-top:10px;'>Reset Password</a>"
        };
        try
        {
            _emailSetting.SendEmail(email);

        }
        catch (Exception ex) { return false; }
        return await Task.FromResult(true);

    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email ?? "");

        if (user != null)
        {
            var result = await _userManager.ResetPasswordAsync(user, model.Token ?? "", model.NewPassword);
            return result.Succeeded;
        }
        return false;
    }
}
