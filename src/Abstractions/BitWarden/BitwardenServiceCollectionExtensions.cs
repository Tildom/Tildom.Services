using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tildom.Services.Abstractions.BitWarden;

public static class BitwardenServiceCollectionExtensions {
  public static IServiceCollection AddBitwardenService(this IServiceCollection services, BitwardenServiceOptions options) {
    ArgumentNullException.ThrowIfNull(options);
    options.Validate();
    services.AddSingleton(options);
    services.AddSingleton<IBitwardenServiceWrapper, BitwardenServiceWrapper>();
    return services;
  }
}
