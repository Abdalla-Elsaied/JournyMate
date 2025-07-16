using JourneyMate.Infrastracture.Specefications.TravelSpecifications;
using JourneyMate.Infrastracture.Specefications;

public class NewestTravelSpecifications : BaseSpecification<Travel>
{
    // Constructor for company-specific travels with optional pagination control
    public NewestTravelSpecifications(TravelSpecPrams travelParams, int companyId, bool enablePagination = true) : base(
        x => x.CreationDate >= DateTime.UtcNow.AddDays(-3) && // Changed from -2 to -3 for "3 days"
        x.CreationDate <= DateTime.UtcNow.AddDays(1) && // Allow slight future buffer
        x.AvailableSeats > 0 &&
        x.CompanyId == companyId)
    {
        Includes.Add(x => x.Company!);
        Includes.Add(x => x.Images);
        AddOrderBy(x => x.CreationDate); // Order by creation date for "newest"

        if (enablePagination)
        {
            ApplyPagination(((travelParams.PageIndex - 1) * travelParams.PageSize), travelParams.PageSize);
        }
    }

    // Constructor for all travels (no company filter)
    public NewestTravelSpecifications(TravelSpecPrams travelParams) : base(
        x => x.CreationDate >= DateTime.UtcNow.AddDays(-3) && // Changed from -2 to -3
        x.CreationDate <= DateTime.UtcNow.AddDays(1) && // Allow slight future buffer
        x.AvailableSeats > 0)
    {
        Includes.Add(x => x.Company!);
        Includes.Add(x => x.Images);
        AddOrderBy(x => x.CreationDate); // Order by creation date for "newest"
        ApplyPagination(((travelParams.PageIndex - 1) * travelParams.PageSize), travelParams.PageSize);
    }
}