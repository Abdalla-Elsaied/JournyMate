using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CateroryDto>> GetCaterories()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            var outputCategory= _mapper.Map<IEnumerable<CateroryDto>>(categories);
            return outputCategory;

        }
    }
}
