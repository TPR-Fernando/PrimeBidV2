using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AuctionDbContext _dbContext; // Assuming AuctionDbContext is your EF Core DbContext

        // Constructor to inject DbContext
        public ProfileService(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Fetch profile by user ID
        public async Task<Profile?> GetProfileAsync(int userId)
        {
            return await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
        }

        // Update profile and return success status
        public async Task<bool> UpdateProfileAsync(int userId, Profile profile)
        {
            var existingProfile = await _dbContext.Profiles.FindAsync(userId);
            if (existingProfile != null)
            {
                existingProfile.FullName = profile.FullName;
                existingProfile.Email = profile.Email;
                existingProfile.Address = profile.Address;
                existingProfile.PhoneNumber = profile.PhoneNumber;

                _dbContext.Profiles.Update(existingProfile);
                await _dbContext.SaveChangesAsync();
                return true; // Indicate success
            }
            return false; // Indicate failure (profile not found)
        }
    }
}
