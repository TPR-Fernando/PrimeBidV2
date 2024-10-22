using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public interface IProfileService
    {
        Task<Profile?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, Profile profile); 
    }
}
