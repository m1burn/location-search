using LocationSearch.Core;
using LocationSearch.Infrastructure.Cache;
using LocationSearch.Infrastructure.Repositories;

namespace LocationSearch.WebApi;

public static class ApplicationCache
{
    public static void Initialize(IApplicationBuilder app)
    {
        InitializeAddressCache(app);
    }

    private static void InitializeAddressCache(IApplicationBuilder app)
    {
        var addressRepository = app.ApplicationServices.GetService<IAddressRepository>();
        var addressCache = app.ApplicationServices.GetService<IAddressCache>();

        Guard.NotNull(addressRepository, nameof(addressRepository));
        Guard.NotNull(addressCache, nameof(addressCache));

        var locationsTask = addressRepository.GetAll();
        // Intentionally make both operations synchronous to avoid deadlocks
        locationsTask.Wait();
        addressCache.Set(locationsTask.Result).Wait();
    }
}