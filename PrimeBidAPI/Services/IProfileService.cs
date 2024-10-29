using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public interface IProfileService
    {
        Task<Profile?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, Profile profile); 
    }

    /*
    //Validating the Session Key
    public async Task<bool> ValidateSessionKeyAsync(int userId, string sessionKey)
    {
        // Check if the session key is valid for the given userId
        var session = await _sessionRepository.GetSessionAsync(userId, sessionKey);
        return session != null && session.IsActive;
    }
    */
}
