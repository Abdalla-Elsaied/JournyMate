using JourneyMate.Application.Helper;
using JourneyMate.Application.IServeces;
using JourneyMate.Infrastracture.Specefications;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;
using JourneyMate.Infrastracture.Specefications.TravelSpecifications;
using JourneyMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;

namespace JourneyMate.Application.Servieces
{
    public class TravelService : ITravelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DocumentSetting _documentSetting;
        private readonly IWebHostEnvironment _webHostEnvironment; // Inject IWebHostEnvironment
        private readonly ILogger<TravelService> _logger;
        private readonly IAiTravelService _aiTravelServices;

        public TravelService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            DocumentSetting documentSetting,
            IWebHostEnvironment webHostEnvironment, // Add this dependency
            ILogger<TravelService> logger,
            IAiTravelService aiTravelServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _documentSetting = documentSetting;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _aiTravelServices = aiTravelServices;
        }

        public async Task<List<Travel>> GetTravelsByCompanyAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID is null or empty.");
                return null;
            }
            var spec = new CompanyUserIdSpec(userId);
            var company = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(spec);
            return company?.Travels?.ToList() ?? new List<Travel>();
        }

        public async Task<TravelVm> GetTravelByIdAsync(int id)
        {
            var spec = new TravelImagesSpec(id);
            var travelDB = await _unitOfWork.Repository<Travel>().GetByIdWithSpecifications(spec);
            if (travelDB == null)
            {
                _logger.LogError($"Travel not found for ID: {id}");
                return null;
            }

            return _mapper.Map<TravelVm>(travelDB);
        }

        public async Task<bool> CreateTravelAsync(CreateTravelVmParamter TravelVmParamter)
        {
            try
            {
                if (string.IsNullOrEmpty(TravelVmParamter.userId) || TravelVmParamter.travelVm == null)
                {
                    _logger.LogWarning("Invalid data received for travel creation.");
                    return false;
                }
                var spec = new CompanyUserIdSpec(TravelVmParamter.userId);
                var company = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(spec);
                if (company == null)
                {
                    _logger.LogError($"Company not found for user ID: {TravelVmParamter.userId}");
                    return false;
                }


                //  travelVm.Price = travelVm.BaseCost * (1 - travelVm.SaleDiscount);


                var travelDB = _mapper.Map<Travel>(TravelVmParamter.travelVm);
                travelDB.CompanyId = company.Id;
                travelDB.CreationDate = DateTime.Now;
                if (TravelVmParamter.ProfileImage != null && TravelVmParamter.CoverImage != null)
                {

                    string ProfileimageUrl = _documentSetting.UploadFile(TravelVmParamter.ProfileImage, $"TravelImages/ProfileAndCover/{travelDB.Id}");
                    travelDB.ProfileImageUrl = ProfileimageUrl;
                    string CoverimageUrl = _documentSetting.UploadFile(TravelVmParamter.ProfileImage, $"TravelImages/ProfileAndCover/{travelDB.Id}");
                    travelDB.CoverImageUrl = CoverimageUrl;
                }
                _unitOfWork.Repository<Travel>().Add(travelDB);
                _unitOfWork.Complete();
                if (TravelVmParamter.images != null && TravelVmParamter.images.Count > 0)
                {
                    foreach (var image in TravelVmParamter.images)
                    {
                        if (image != null && image.Length > 0)
                        {
                            string imageUrl = _documentSetting.UploadFile(image, $"TravelImages/{travelDB.Id}");
                            travelDB.Images.Add(new Image { ImageUrl = imageUrl });
                        }
                    }
                    _unitOfWork.Complete();
                }
                // here is where to send the same obj of travel to the Ai
                bool aiSynced = await _aiTravelServices.SyncTravelAsync(travelDB);
                if (!aiSynced)
                {
                    _logger.LogWarning($"AI sync failed after updating travel ");
                }
             

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating travel.");
                return false;
            }
        }

        public async Task<bool> UpdateTravelAsync(int id, TravelVm travelVm, List<int> imagesToRemove, List<IFormFile> newImages)
        {
            try
            {
                var spec = new TravelImagesSpec(id);
                var travelDB = await _unitOfWork.Repository<Travel>().GetByIdWithSpecifications(spec);
                if (travelDB == null)
                {
                    _logger.LogError($"Travel not found for ID: {id}");
                    return false;
                }
                var coveurl = travelDB.CoverImageUrl;
                var profileurl = travelDB.ProfileImageUrl;
                // Update scalar values
                _mapper.Map(travelVm, travelDB);

                travelDB.CoverImageUrl = coveurl;
                travelDB.ProfileImageUrl = profileurl;

                // 🔻 Remove gallery images
                if (imagesToRemove != null && imagesToRemove.Any())
                {
                    var imagesToDelete = travelDB.Images.Where(img => imagesToRemove.Contains(img.Id)).ToList();
                    foreach (var image in imagesToDelete)
                    {
                        _documentSetting.DeleteFile(image.ImageUrl); // ✅ Old gallery image from wwwroot
                        travelDB.Images.Remove(image);
                        _unitOfWork.Repository<Image>().Delete(image);
                    }
                }

                // 🔺 Add new gallery images
                if (newImages != null && newImages.Any())
                {
                    foreach (var image in newImages)
                    {
                        if (image != null && image.Length > 0)
                        {
                            string imageUrl = _documentSetting.UploadFile(image, $"TravelImages/{travelDB.Id}");
                            travelDB.Images.Add(new Image
                            {
                                ImageUrl = imageUrl,
                                TravelId = travelDB.Id
                            });
                        }
                    }
                }

                // ✅ Handle Cover Image replacement
                if (travelVm.IFormFileCoverImage != null && travelVm.IFormFileCoverImage.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(travelDB.CoverImageUrl))
                    {
                        _documentSetting.DeleteFile(travelDB.CoverImageUrl); // ✅ Deletes old image from wwwroot
                    }

                    string coverImageUrl = _documentSetting.UploadFile(travelVm.IFormFileCoverImage, $"Travel/CoverImages/{travelDB.Id}");
                    travelDB.CoverImageUrl = coverImageUrl;
                }

                // ✅ Handle Profile Image replacement
                if (travelVm.IFormFileProfileImage != null && travelVm.IFormFileProfileImage.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(travelDB.ProfileImageUrl))
                    {
                        _documentSetting.DeleteFile(travelDB.ProfileImageUrl); // ✅ Deletes old image from wwwroot
                    }

                    string profileImageUrl = _documentSetting.UploadFile(travelVm.IFormFileProfileImage, $"Travel/ProfileImages/{travelDB.Id}");
                    travelDB.ProfileImageUrl = profileImageUrl;
                }

                await _unitOfWork.CompleteAsync();

                // Optional: sync with AI
                bool aiSynced = await _aiTravelServices.UpdateTravelAsync(travelDB);
                if (!aiSynced)
                {
                    _logger.LogWarning($"AI sync failed after updating travel {id}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating travel {id}");
                return false;
            }
        }



        public async Task<bool> DeleteTravelAsync(int id)
        {
            try
            {
                var spec = new TravelImagesSpec(id);
                var travel = await _unitOfWork.Repository<Travel>().GetByIdWithSpecifications(spec);
                if (travel == null)
                {
                    _logger.LogError($"Travel not found for ID: {id}");
                    return false;
                }

                var travelFolder = Path.Combine(_webHostEnvironment.WebRootPath, "TravelImages", id.ToString());
                if (Directory.Exists(travelFolder))
                {
                    Directory.Delete(travelFolder, true);
                }

                _unitOfWork.Repository<Travel>().Delete(travel);
                _unitOfWork.Complete();
                var aiSynced = await _aiTravelServices.DeleteTravelFromAIServiceAsync(id.ToString());
                 if (!aiSynced)
                {
                    _logger.LogWarning($"AI sync failed after Deleting the  travel ");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting travel {id}");
                return false;
            }
        }

        public async Task<IEnumerable<Category>?> GetCategories()
        {
            return await _unitOfWork.Repository<Category>().GetAllAsync();
        }
    }
}

