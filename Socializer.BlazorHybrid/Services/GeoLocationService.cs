using Common.Utils;
namespace Socializer.BlazorHybrid.Services;

internal class GeoLocationService : IGeoLocationService
{
    public async Task<OperationResult<Location>> GetGeoLocation()
    {
        try
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (permissionStatus != PermissionStatus.Granted)
                permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (permissionStatus != PermissionStatus.Granted)
                throw new UnauthorizedAccessException($"Location permission is not granted. PermissionStatus: {permissionStatus}.");

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
                return OperationResult<Location>.Success(location);

            location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return OperationResult<Location>.Success(location);

            return OperationResult<Location>.Failure("Unable to retrieve location. No last known location available.");

        }
        catch (Exception ex)
        {
            return OperationResult<Location>.Failure(ex);
        }
    }
}
