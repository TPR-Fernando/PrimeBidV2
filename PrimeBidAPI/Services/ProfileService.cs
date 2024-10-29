﻿using Microsoft.EntityFrameworkCore;
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
            // Use FirstOrDefaultAsync to return null if not found
            return await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
        }

        // Update profile and return success status
        public async Task<bool> UpdateProfileAsync(int userId, Profile profile)
        {
            var existingProfile = await _dbContext.Profiles.FindAsync(userId);
            if (existingProfile != null)
            {
                // Update properties
                existingProfile.FullName = profile.FullName;
                existingProfile.Email = profile.Email;
                existingProfile.Address = profile.Address;
                existingProfile.PhoneNumber = profile.PhoneNumber;

                // No need to call Update since you are tracking existingProfile
                await _dbContext.SaveChangesAsync();
                return true; // Indicate success
            }
            return false; // Indicate failure (profile not found)
        }

        // Delete profile by user ID
        public async Task DeleteProfileAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid userId");
            }

            var profile = await _dbContext.Profiles.FindAsync(userId);
            if (profile == null)
            {
                throw new KeyNotFoundException($"Profile not found for userId: {userId}");
            }

            _dbContext.Profiles.Remove(profile);
            await _dbContext.SaveChangesAsync();
        }

    }
}
