using Common.Utils;

namespace Socializer.BlazorHybrid.Services;

internal interface IGeoLocationService
{
    Task<OperationResult<Location>> GetGeoLocation();
}
