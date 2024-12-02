using Bitwarden.Sdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.DataProtection;
using Tildom.Services.Abstractions.BitWarden;

namespace Tildom.Services.Configuration.Bitwarden;

public class BitwardenConfigurationProvider : ConfigurationProvider {
  private readonly BitwardenServiceOptions _options;
  private readonly BitwardenClient         _client;

  public BitwardenConfigurationProvider(BitwardenServiceOptions options) {
    options.Validate();
    _options = options;
    _client  = new BitwardenClient(new BitwardenSettings { ApiUrl = options.ApiUrl, IdentityUrl = options.IdentityUrl });
    _client.Auth.LoginAccessToken(options.AccessToken);
  }

  public override void Load() {
    var secrets = FetchSecretsFromBitwarden();
    Data = secrets;
  }

  private Dictionary<string, string?> FetchSecretsFromBitwarden() {
    var secrets = new Dictionary<string, string?>();
    try {
      var project = _client.Projects.Get(_options.ProjectId);
      if (project is null) {
        throw new Exception($"Project with ID {_options.ProjectId} not found.");
      }

      var allSecretsResponse = _client.Secrets.List(_options.OrgId);
      if (allSecretsResponse?.Data is null || allSecretsResponse.Data.Length == 0) {
        throw new Exception("No secrets found for Organisation.");
      }

      foreach (var secret in allSecretsResponse.Data) {
        var secretResponse = _client.Secrets.Get(secret.Id);
        if (secretResponse is null) {
          throw new Exception($"Secret with ID {secret.Id} not found.");
        }

        if (secretResponse.ProjectId == _options.ProjectId) {
          secrets.Add(secretResponse.Key, secretResponse.Value);
        }
      }
    } catch (Exception ex) {
      throw new Exception("Error fetching secrets from Bitwarden", ex);
    }

    return secrets;
  }
}