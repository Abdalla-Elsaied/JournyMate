using System;
using JourneyMate.Application.Common;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;
using JourneyMate.Infrastracture.Specefications.TravelSpecifications;

namespace JourneyMate.Application.ServicesApi.Implementaion;

public class SearchServices(ITravelServices _travelService, ICompanyServices _companyServices,IIpiEnventService _eventSerives) : ISearchServices

{
    public async Task<object> SearchAsync(string? type, string? query,int pageIndex, int pageSize)
    {
         if (string.IsNullOrWhiteSpace(type))
        {
            // Perform search across all types
            var travelParams = new TravelSpecPrams
            {
                Search = query,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var travels = await _travelService.GetAllTravles(travelParams);
            var travelsCount = await _travelService.GetCountAsync(travelParams, new TravelSpecifications(travelParams));

            var eventParams = new BaseSpecPrams
            {
                Search = query,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var events = await _eventSerives.GetIpiEvenstWithSpecAsync(eventParams);

            var companyParams = new CompanySpecParams
            {
                Search = query,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var companies = await _companyServices.GetAllCompaniessBySpec(companyParams);
            var companiesCount = await _companyServices.GetCountAsync(companyParams);

            return new
            {
                Travels = new PagedResult<TravelDetailsDto>(travels, travelsCount, pageIndex, pageSize),
                Events = new PagedResult<APIEventItem>(events, travelsCount, pageIndex, pageSize),
                Companies = new PagedResult<CompanyDetailsDto>(companies, companiesCount, pageIndex, pageSize)
            };
        }
         switch (type.ToLower())
        {
            case "travels":
                var travelParams = new TravelSpecPrams
                {
                    Search = query,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                var travels = await _travelService.GetAllTravles(travelParams);
                var travelsCount = await _travelService.GetCountAsync(travelParams, new TravelSpecifications(travelParams));
                return new PagedResult<TravelDetailsDto>(travels, travelsCount, pageIndex, pageSize);

            case "events":
                var eventPrams = new BaseSpecPrams
                {
                    Search = query,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                var events = await _eventSerives.GetIpiEvenstWithSpecAsync(eventPrams);
                return events;

            case "companies":
                var companyParams = new CompanySpecParams
                {
                    Search = query,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                var companies = await _companyServices.GetAllCompaniessBySpec(companyParams);
                var companiesCount = await _companyServices.GetCountAsync(companyParams);
                return new PagedResult<CompanyDetailsDto>(companies, companiesCount, pageIndex, pageSize);

            default:
                throw new ArgumentException("Invalid type specified.");
        }
       
    }

}
