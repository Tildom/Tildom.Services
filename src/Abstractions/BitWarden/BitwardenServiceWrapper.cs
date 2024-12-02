using System.Reflection;
using Bitwarden.Sdk;
using Tildom.Core.DataOperations;
namespace Tildom.Services.Abstractions.BitWarden;

public class BitwardenServiceWrapper : IBitwardenServiceWrapper, IDisposable {
  public BitwardenServiceOptions Options { get; init; }
  public BitwardenClient         Client  { get; init; }

  public BitwardenServiceWrapper(BitwardenServiceOptions options) {
    options.Validate();
    Options = options;
    Client  = new BitwardenClient(new BitwardenSettings() { ApiUrl = options.ApiUrl, IdentityUrl = options.IdentityUrl });
    //Client.AccessTokenLogin(Options.AccessToken);
    Client.Auth.LoginAccessToken(accessToken: Options.AccessToken);
  }

  public IOperationResult<IEnumerable<SecretResponse>> GetProjectSecrets(Guid projectId) {
    // Check if the project exists
    try {
      var project = Client.Projects.Get(projectId);
      // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
      if (project == null) {
        return new ErrorResult<IEnumerable<SecretResponse>>(new Exception("Project not found."));
      }
    } catch (Exception e) {
      // Project does not exist
      return new ErrorResult<IEnumerable<SecretResponse>>(e);
    }
    // Get all secrets for the machine account's access token
    var secrets = new List<SecretResponse>();
    try {
      var allSecrets = Client.Secrets.List(Options.OrgId);
      if (allSecrets?.Data == null || allSecrets.Data.Length == 0) {
        return new ErrorResult<IEnumerable<SecretResponse>>(new Exception("No secrets found."));
      }
      foreach (var secret in allSecrets.Data) {
        // Get the secret's full details
        var secretDetails = Client.Secrets.Get(secret.Id);
        if (secretDetails?.ProjectId == null || secretDetails.ProjectId.Value == Guid.Empty) {
          continue;
        }
        if (secretDetails.ProjectId == projectId) {
          secrets.Add(secretDetails);
        }
      }
    } catch (Exception e) {
      // Organisation does not exist
      return new ErrorResult<IEnumerable<SecretResponse>>(e);
    }
    if (!secrets.Any()) {
      return new ErrorResult<IEnumerable<SecretResponse>>(new Exception("No secrets found."));
    }
    return new SuccessResult<IEnumerable<SecretResponse>>(secrets);
  }

  // A generic overload that allows for strongly typed secrets to be returned
  public IOperationResult<T> GetProjectSecrets<T>(Guid projectId) where T : new() {
    IOperationResult<T> operationResult        = null!;
    var                 originalProjSecrets    = new List<SecretResponse>();
    var                 projSecretsQueryResult = GetProjectSecrets(projectId);
    projSecretsQueryResult.Match(
                                 onSuccess: results => { originalProjSecrets = results.ToList(); },
                                 onError: (ex, _) => { operationResult = new ErrorResult<T>(ex); });
    // If an error occurred, return the error result
    if (operationResult != null) {
      return operationResult;
    }
    T   customType = new T();
    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    var secretDict = originalProjSecrets.ToDictionary(s => s.Key, s => s.Value);

    foreach (var prop in properties) {
      var secretNameAttr = prop.GetCustomAttribute<SecretNameAttribute>();
      var secretName     = secretNameAttr?.Name ?? prop.Name;

      if (secretDict.TryGetValue(secretName, out var value)) {
        try {
          var convertedValue = Convert.ChangeType(value, prop.PropertyType);
          prop.SetValue(customType, convertedValue);
        } catch (Exception ex) {
          return new ErrorResult<T>(new Exception($"Failed to set property '{prop.Name}' with value '{value}'.", ex));
        }
      }
    }
    return new SuccessResult<T>(customType);
  }

  // Cache the secrets locally
  // Cache should expire after a certain amount of time and removed

  public void Dispose() { Client.Dispose(); }
}
