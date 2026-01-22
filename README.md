# MauiStrides

A .NET MAUI application for tracking Strava activities.

## Prerequisites

- .NET 10.0 SDK
- Visual Studio 2022 (or VS Code with MAUI workload)
- Strava API credentials

## Setup

### 1. Clone the Repository

```bash
git clone https://github.com/ThomasTollstedt/MauiStrides.git
cd MauiStrides
```

### 2. Configure Strava API Credentials

1. Copy `appsettings.json.template` to `appsettings.json`:
   ```bash
   copy appsettings.json.template appsettings.json
   ```

2. Get your Strava API credentials:
   - Go to https://www.strava.com/settings/api
   - Create a new application or use existing one
   - Copy your **Client ID** and **Client Secret**

3. Edit `appsettings.json` and replace the placeholders:
   ```json
   {
     "Strava": {
       "ClientId": "YOUR_CLIENT_ID_HERE",
       "ClientSecret": "YOUR_CLIENT_SECRET_HERE"
     }
   }
   ```

4. **?? IMPORTANT: Never commit `appsettings.json` to Git!**
   - It's already in `.gitignore` to prevent accidental commits
   - Only commit `appsettings.json.template`

### 3. Build and Run

```bash
dotnet restore
dotnet build
```

Run on your preferred platform:
- **Android**: Select Android emulator or device
- **iOS**: Requires Mac with Xcode
- **Windows**: Run as Windows App

## Features

- ? OAuth authentication with Strava
- ? Automatic token refresh (tokens expire after 6 hours)
- ? Fetch athlete profile
- ? List all activities
- ? Filter activities by type
- ? View activity details
- ? Secure token storage using MAUI SecureStorage

## Project Structure

```
MauiStrides/
??? Client/
?   ??? StravaApiClient.cs       # HTTP client for Strava API
??? Models/
?   ??? Activity.cs              # Activity data model
?   ??? AthleteProfile.cs        # Athlete profile model
?   ??? StravaTokenResponse.cs   # OAuth token response model
??? Services/
?   ??? IStravaService.cs        # Service interface
?   ??? StravaService.cs         # Business logic layer
?   ??? ITokenService.cs         # Token management interface
?   ??? TokenService.cs          # Token refresh & storage
?   ??? StravaConfiguration.cs   # Configuration model
??? ViewModels/
?   ??? ActivitiesViewModel.cs   # MVVM ViewModel
??? Views/
?   ??? ActivitiesPage.xaml      # Activities UI
??? appsettings.json.template    # Configuration template
```

## Security Notes

- ? API credentials stored in `appsettings.json` (excluded from Git)
- ? Access tokens stored securely using MAUI `SecureStorage`
- ? Automatic token refresh before expiration
- ? Refresh tokens rotated per Strava OAuth spec

## Token Management

The app automatically handles Strava's OAuth token lifecycle:

1. **Initial Authentication**: User logs in via Strava OAuth
2. **Token Storage**: Access token, refresh token, and expiry stored securely
3. **Auto Refresh**: When token expires (6h), automatically refreshes using refresh token
4. **Token Rotation**: New refresh token stored after each refresh (Strava rotates them)

## Troubleshooting

### "appsettings.json not found" Error
- Ensure you've created `appsettings.json` from the template
- Verify it's set as `EmbeddedResource` in `.csproj`

### Authentication Fails
- Verify your Client ID and Secret are correct
- Check Strava API application settings
- Ensure redirect URIs match your app configuration

### Build Errors
- Restore NuGet packages: `dotnet restore`
- Clean solution: `dotnet clean`
- Rebuild: `dotnet build`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. **Never commit `appsettings.json`**
5. Submit a pull request

## License

[Add your license here]

## Author

Thomas Tollstedt
