using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeBidAPI.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AuctionDbContext _dbContext;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(AuctionDbContext dbContext, ILogger<ProfileService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Profile?> GetProfileAsync(int userId)
        {
            return await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
        }

        public async Task<bool> UpdateProfileAsync(int userId, Profile profile)
        {
            var existingProfile = await _dbContext.Profiles.FindAsync(userId);
            if (existingProfile != null)
            {
                existingProfile.FullName = profile.FullName;
                existingProfile.Email = profile.Email;
                existingProfile.Address = profile.Address;
                existingProfile.PhoneNumber = profile.PhoneNumber;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Profile>> GetAllProfilesAsync()
        {
            _logger.LogInformation("Fetching all user profiles.");
            var profiles = await _dbContext.Profiles.ToListAsync();

            if (!profiles.Any())
            {
                _logger.LogWarning("No user profiles found.");
            }

            return profiles;
        }

        public async Task DeleteProfileAsync(int userId)
        {
            var profile = await _dbContext.Profiles.FindAsync(userId);
            if (profile == null)
            {
                throw new KeyNotFoundException($"Profile not found for userId: {userId}");
            }

            _dbContext.Profiles.Remove(profile);
            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<Profile> GetAllProfiles()
        {
            return _dbContext.Profiles.ToList();
        }
    }
}
