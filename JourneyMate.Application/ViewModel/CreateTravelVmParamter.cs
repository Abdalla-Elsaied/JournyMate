namespace JourneyMate.Application.ViewModel
{
    public class CreateTravelVmParamter
    {
        public TravelVm travelVm { get; set; }

        public string userId { get; set; }

        public List<IFormFile> images { get; set; }

        public IFormFile ProfileImage { get; set; }
        public IFormFile CoverImage { get; set; }

    }
}
