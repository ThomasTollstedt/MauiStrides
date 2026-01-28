using MauiStrides.Client;
using MauiStrides.Interfaces;
using MauiStrides.Models;
using MauiStrides.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Repositories
{
    public class AthleteRepository : IAthleteRepository
    {
        private readonly StravaApiClient _stravaApiClient;

        public AthleteRepository(StravaApiClient stravaApiClient)
        {
            _stravaApiClient = stravaApiClient;
        }

        public async Task<AthleteProfile> GetAthleteProfileAsync()
        {
            var profileDetail = await _stravaApiClient.GetAthleteProfileAsync();
            var profile = MapToDomainProfile(profileDetail);

            return profile;
        }

        //Hjälpmetod för att mappa från DTO till domänmodell
        private AthleteProfile MapToDomainProfile(AthleteProfileDTO dto)
        {
            return new AthleteProfile
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Id = dto.Id,
                Bio = dto.Bio,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                Sex = dto.Sex,
                Weight = dto.Weight,
                ProfileMedium = dto.ProfileMedium,
                Profile = dto.Profile,
                
            };
        }
    }
}
