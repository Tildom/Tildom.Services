using Bitwarden.Sdk;
using Tildom.Core.DataOperations;

namespace Tildom.Services.Abstractions.BitWarden;

public interface IBitwardenServiceWrapper {
  BitwardenServiceOptions Options { get; init; }
  BitwardenClient         Client  { get; init; }

  IOperationResult<IEnumerable<SecretResponse>> GetProjectSecrets(Guid projectId);
}
