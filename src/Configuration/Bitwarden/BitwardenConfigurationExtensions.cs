using Microsoft.Extensions.Configuration;
using Tildom.Services.Abstractions.BitWarden;

namespace Tildom.Services.Configuration.Bitwarden;

public static class BitwardenConfigurationExtensions {
  public static IConfigurationBuilder AddBitwarden(this IConfigurationBuilder builder, BitwardenServiceOptions options) {
    builder.Add(new BitwardenConfigurationSource(options));
    return builder;
  }
}