using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Tildom.Services.Abstractions.BitWarden;

public class BitwardenServiceOptions {
  public required string IdentityUrl { get; init; }
  public required string ApiUrl      { get; init; }
  public required string AccessToken { get; init; }
  public required Guid   OrgId       { get; init; }
  public required Guid   ProjectId   { get; init; } = Guid.Empty;

  /// <summary>
  /// Validates the BitwardenServiceOptions properties.
  /// </summary>
  /// <exception cref="ArgumentException">Thrown when a validation check fails.</exception>
  public void Validate() {
    // Validate IdentityUrl
    if (!Uri.TryCreate(IdentityUrl, UriKind.Absolute, out var identityUri) || (identityUri.Scheme != Uri.UriSchemeHttp && identityUri.Scheme != Uri.UriSchemeHttps)) {
      throw new ArgumentException("IdentityUrl must be a valid HTTP or HTTPS URL.", nameof(IdentityUrl));
    }

    // Validate ApiUrl
    if (!Uri.TryCreate(ApiUrl, UriKind.Absolute, out var apiUri) || (apiUri.Scheme != Uri.UriSchemeHttp && apiUri.Scheme != Uri.UriSchemeHttps)) {
      throw new ArgumentException("ApiUrl must be a valid HTTP or HTTPS URL.", nameof(ApiUrl));
    }

    // Validate AccessToken
    if (string.IsNullOrWhiteSpace(AccessToken)) {
      throw new ArgumentException("AccessToken cannot be null, empty, or whitespace.", nameof(AccessToken));
    }

    // Validate OrgId
    if (OrgId == Guid.Empty) {
      throw new ArgumentException("OrgId cannot be empty", nameof(OrgId));
    }
  }
}
