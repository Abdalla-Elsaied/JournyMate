using JourneyMate.Application.Helper;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;

namespace JourneyMate.Application.Servieces;

public class CompanyDashService : ICompanyDashService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly DocumentSetting _documentSetting;
    private readonly ILogger<CompanyDashService> _logger;
    private readonly IMapper _mapper;

    public CompanyDashService(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        DocumentSetting documentSetting,
        ILogger<CompanyDashService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _documentSetting = documentSetting;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<CompanyVm?> GetCompanyByUserIdAsync(string userId)
    {
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null or empty.");
                    return null;
                }
                var spec =new CompanyUserIdSpec(userId);
                var company = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(spec);
                var user = await _userManager.FindByIdAsync(userId);

                if (company == null || user == null )
                {
                    _logger.LogError($"Company or User not found for ID: {userId}");
                    return null;
                }

                var companyModel = _mapper.Map<CompanyVm>(company);
                companyModel.Travels = _mapper.Map<List<Travel>, List<TravelVm>>(company.Travels);
                companyModel.Email = user.Email;
                companyModel.PhoneNumber = user.PhoneNumber;
                companyModel.CompanyName = user.UserName;

                return companyModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching company data.");
                return null;
            }
        }


    }

    public async Task<bool> UpdateCompanyAsync(CompanyVm companyVm, string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || companyVm == null)
            {
                _logger.LogWarning("Invalid data received for company update.");
                return false;
            }

            var spec = new CompanyUserIdSpec(userId);
            var company = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(spec);
            var user = await _userManager.FindByIdAsync(userId);

            if (company == null)
            {
                _logger.LogError($"Company not found for ID: {userId}");
                return false;
            }

            companyVm.SocialMediaLinks = companyVm.SocialMediaLinks
                ?.Where(link => !string.IsNullOrEmpty(link.Url))
                .ToList();

            companyVm.PaymentMethods = companyVm.PaymentMethods
                ?.Where(link => !string.IsNullOrEmpty(link.MethodName) && !string.IsNullOrEmpty(link.Details))
                .ToList();

            // Handle Profile Image Upload - PRESERVE EXISTING IF NO NEW FILE
            if (companyVm.ProfileImageFile != null)
            {
                bool isTheSame = companyVm.ProfileImageUrl != null && company.ProfileImageUrl != null &&
                               _documentSetting.check(companyVm.ProfileImageFile, company.ProfileImageUrl);

                if (!isTheSame)
                {
                    if (company.ProfileImageUrl != null)
                    {
                        _documentSetting.DeleteFile(company.ProfileImageUrl);
                    }
                    companyVm.ProfileImageUrl = _documentSetting.UploadFile(companyVm.ProfileImageFile, "CompanyImages");
                }
            }
            else
            {
                // PRESERVE existing profile image if no new file is uploaded
                companyVm.ProfileImageUrl = company.ProfileImageUrl;
            }

            // Handle Cover Image Upload - PRESERVE EXISTING IF NO NEW FILE
            if (companyVm.CoverImageFile != null)
            {
                bool isTheSame = companyVm.CoverImageUrl != null && company.CoverImageUrl != null &&
                               _documentSetting.check(companyVm.CoverImageFile, company.CoverImageUrl);

                if (!isTheSame)
                {
                    if (company.CoverImageUrl != null)
                    {
                        _documentSetting.DeleteFile(company.CoverImageUrl);
                    }
                    companyVm.CoverImageUrl = _documentSetting.UploadFile(companyVm.CoverImageFile, "CompanyImages");
                }
            }
            else
            {
                // PRESERVE existing cover image if no new file is uploaded
                companyVm.CoverImageUrl = company.CoverImageUrl;
            }

            _mapper.Map(companyVm, company);

            if (user is null) return false;
            user.PhoneNumber = companyVm.PhoneNumber;
            user.UserName = companyVm.CompanyName;
            user.FullName = companyVm.CompanyName;
            await _userManager.UpdateAsync(user);
            company.Address = companyVm.Address;

            if (companyVm.PaymentMethods != null)
            {
                var existingPaymentMethods = company.PaymentMethods.ToList();
                var submittedPaymentMethods = companyVm.PaymentMethods;

                foreach (var submittedMethod in submittedPaymentMethods)
                {
                    if (submittedMethod.Id > 0)
                    {
                        var existingMethod = existingPaymentMethods.FirstOrDefault(pm => pm.Id == submittedMethod.Id);
                        if (existingMethod != null)
                        {
                            existingMethod.Type = submittedMethod.MethodName;
                            existingMethod.Provider = submittedMethod.Details;
                        }
                    }
                    else
                    {
                        var newPaymentMethod = new PaymentMethod
                        {
                            Type = submittedMethod.MethodName,
                            Provider = submittedMethod.Details,
                            CompanyId = company.Id
                        };
                        company.PaymentMethods.Add(newPaymentMethod);
                    }
                }

                var methodsToRemove = existingPaymentMethods.Where(pm => !submittedPaymentMethods.Any(sm => sm.Id == pm.Id)).ToList();
                foreach (var methodToRemove in methodsToRemove)
                {
                    company.PaymentMethods.Remove(methodToRemove);
                }
            }

            if (companyVm.SocialMediaLinks != null)
            {
                var existingSocialMediaLinks = company.SocialMediaLinks.ToList();
                var submittedSocialMediaLinks = companyVm.SocialMediaLinks;

                foreach (var submittedSocialMediaLink in submittedSocialMediaLinks)
                {
                    if (submittedSocialMediaLink.Id > 0)
                    {
                        var existingSocialMediaLink = existingSocialMediaLinks.FirstOrDefault(pm => pm.Id == submittedSocialMediaLink.Id);
                        if (existingSocialMediaLink != null)
                        {
                            existingSocialMediaLink.Url = submittedSocialMediaLink.Url;
                            existingSocialMediaLink.Platform = submittedSocialMediaLink.Platform;
                        }
                    }
                    else
                    {
                        var newSocialMediaLink = new SocialMediaLink
                        {
                            Url = submittedSocialMediaLink.Url,
                            Platform = submittedSocialMediaLink.Platform,
                            CompanyId = company.Id
                        };
                        company.SocialMediaLinks.Add(newSocialMediaLink);
                    }
                }

                var socialMediaLinksToRemove = existingSocialMediaLinks.Where(pm => !submittedSocialMediaLinks.Any(sm => sm.Id == pm.Id)).ToList();
                foreach (var socialMediaLinkToRemove in socialMediaLinksToRemove)
                {
                    company.SocialMediaLinks.Remove(socialMediaLinkToRemove);
                }
            }

            _unitOfWork.Repository<Company>().Update(company);
            _unitOfWork.Complete();

            _logger.LogInformation($"Company {userId} updated successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating company {userId}");
            return false;
        }
    }
}

