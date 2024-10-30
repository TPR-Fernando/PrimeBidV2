using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public interface IProfileService
    {
        Task<Profile?> GetProfileAsync(int userId);
     
        Task<bool> UpdateProfileAsync(int userId, Profile profile);
        IEnumerable<Profile> GetAllProfiles();

        //Chanuka: Session Validation
        Task<bool> ValidateSessionKeyAsync(int userId, string sessionKey);
        //I also added this method just in case...cus its not here? but in the ProfileService class
        //Task<bool> DeleteProfileAsync(int userId);
    }
}
