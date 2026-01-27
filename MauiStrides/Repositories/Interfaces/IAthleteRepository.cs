using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Repositories.Interfaces
{
    public interface IAthleteRepository
    {
        Task<AthleteProfile> GetAthleteProfileAsync();

    }
}
