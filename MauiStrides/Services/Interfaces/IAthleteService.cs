using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services.Interfaces
{
    public interface IAthleteService
    {
        Task<AthleteProfile> GetAthleteProfileAsync();
    }
}
