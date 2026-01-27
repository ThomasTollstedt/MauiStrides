using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services
{
   public interface IStravaAuthService
    {
        Task HandleAuthCallbackAsync(string uriString);
        Task LoginServiceAsync();
        Task LogoutAsync();
    }

}
