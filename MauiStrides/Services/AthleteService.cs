using MauiStrides.Models;
using MauiStrides.Repositories.Interfaces;
using MauiStrides.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services
{
    public class AthleteService : IAthleteService
    {
        private readonly IAthleteRepository _athleteRepository;

        public AthleteService(IAthleteRepository athleteRepository)
        {
            _athleteRepository = athleteRepository;
        }

        public async Task<AthleteProfile> GetAthleteProfileAsync()
        {
            return await _athleteRepository.GetAthleteProfileAsync();
        }

    }
}
