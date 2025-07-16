using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.Application.ServicesApi.Interfaces;

public interface IUserAccountService :IPasswordService
{
    Task<AuthModel> CreateUser(UserSignUpVm userCreate);

    Task<AuthModel> UserLoger(UserLogInVm logIn);
    Task<AuthModel> GoogleLogin(string idToken);
}
