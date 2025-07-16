namespace JourneyMate.Core.Models;
public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsFirstLogin { get; set; }

    public ICollection<FavoriteTravel> FavoriteTravels = new List<FavoriteTravel>();
}