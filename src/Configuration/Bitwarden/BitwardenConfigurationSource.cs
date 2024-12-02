using Microsoft.Extensions.Configuration;

using Tildom.Services.Abstractions.BitWarden;

namespace Tildom.Services.Configuration.Bitwarden;

public class BitwardenConfigurationSource : IConfigurationSource {
  private readonly BitwardenServiceOptions _options;

  public BitwardenConfigurationSource(BitwardenServiceOptions options) {
    options.Validate();
    _options = options;
  }

  public IConfigurationProvider Build(IConfigurationBuilder builder) {
    return new BitwardenConfigurationProvider(_options);
  }
}