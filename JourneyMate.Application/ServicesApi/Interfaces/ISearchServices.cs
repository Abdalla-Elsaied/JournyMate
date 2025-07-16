using System;

namespace JourneyMate.Application.ServicesApi.Interfaces;

public interface ISearchServices
{

    Task<object> SearchAsync(string? type, string? query, int pageIndex, int pageSize);
    
    


}
