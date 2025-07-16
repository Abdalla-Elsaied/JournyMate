using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Application.Servieces;
using JourneyMate.Application.Setting;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class UserAccountService : PasswordService, IUserAccountService
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserAccountService> logger;
        private readonly IRecommendationServices _recommendationService;

        public UserAccountService(UserManager<AppUser> userManager, IMapper mapper, IJwtService jwtService, EmailSetting emailSetting, IRecommendationServices recommendationService, ILogger<UserAccountService> logger)
             : base(userManager, emailSetting)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtService = jwtService;
            _recommendationService = recommendationService;
            this.logger = logger;
        }

        public async Task<AuthModel> CreateUser(UserSignUpVm userCreate)
        {
            string? input = userCreate.Email;

            if (string.IsNullOrWhiteSpace(input))
            {
                return new AuthModel { Message = "Email is required!" };
            }

            var existingUser = await _userManager.FindByEmailAsync(userCreate.Email ?? "");

            if (existingUser is not null)
            {
                return new AuthModel { Message = "Email is already registered!" };
            }

            var userMap = new AppUser
            {
                UserName = userCreate.UserName,
                Email = userCreate.Email
            };

            IdentityResult? userCreated = await _userManager.CreateAsync(userMap, userCreate.Password ?? "");

            if (!userCreated.Succeeded)
            {
                string errors = string.Join(",", userCreated.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(userMap, "User");
            
            // ✅ Add AI sync: wrap ID as string & interactions = null
            var aiDto = new UserInteractionDto
            {
                Id = userMap.Id.ToString(),
                UserInteraction = new List<InteractionDto>()
            };
            await _recommendationService.SetInteractionsAsync(aiDto);

            var authModel =await CreateToken(userMap);
            return authModel;
        }

        public async Task<AuthModel> GoogleLogin(string idToken)
        {
            try
            {
                Payload? payload = await ValidateAsync(idToken);
                if (payload == null)
                {
                    return new AuthModel { IsAuthenticated = false, Message = "Invalid token" };
                }
                var email = payload.Email;
                var name = payload.Name;
                var googleUser = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = email,
                };
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    IdentityResult? userCreated = await _userManager.CreateAsync(googleUser);

                    if (!userCreated.Succeeded)
                    {
                        string errors = string.Join(", ", userCreated.Errors.Select(e => e.Description));
                        return new AuthModel { IsAuthenticated = false, Message = errors };
                    }

                    await _userManager.AddToRoleAsync(googleUser, "User");

                
                     user = await _userManager.FindByEmailAsync(email);

                }

                var authModel = await CreateToken(user);
                return authModel;

            }
            catch (Exception ex)
            {
                return new AuthModel { IsAuthenticated = false, Message = "Invalid token" };

            }
        }

        public async Task<AuthModel> UserLoger(UserLogInVm logIn)
        {
            var user = await _userManager.FindByNameAsync(logIn.UserName?.Trim() ?? "");

            if (user is null || !await _userManager.CheckPasswordAsync(user!, logIn.Password ?? ""))
            {
                return new AuthModel { IsAuthenticated = false, Message = "UserName or password  not found" };
            }

            var authModel = await CreateToken(user);
            return authModel;

           
        }
        public async Task<AuthModel> CreateToken(AppUser? user)
        {
            if (user is null)
            {
                return new AuthModel { IsAuthenticated = false, Message = "User not found" };
            }
            var jwtSecurityToken = await _jwtService.CreatTokenAsync(user, _userManager);

            return new AuthModel
            {
                EmailorNumber = user.Email ?? user.PhoneNumber?.ToString(),
                IsAuthenticated = true,
                Token = jwtSecurityToken,
                UserName = user.UserName
            };
        }


    }
}
