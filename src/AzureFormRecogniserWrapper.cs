using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;

namespace Tildom.Services;
public interface IAzureFormRecognizerWrapper {
  AzureFormRecognizerSettings Settings { get; init; }
  DocumentAnalysisClient Client { get; init; }
}

public class AzureFormRecognizerWrapper(AzureFormRecognizerSettings settings) : IAzureFormRecognizerWrapper {
  public AzureFormRecognizerSettings Settings { get; init; } = settings;
  public DocumentAnalysisClient Client { get; init; } = new DocumentAnalysisClient(new Uri(settings.Endpoint), new AzureKeyCredential(settings.ApiKey));
}

public class AzureFormRecognizerSettings {
  public string Endpoint { get; init; } = string.Empty;
  public string ApiKey { get; init; } = string.Empty;
  public string ModelId { get; init; } = string.Empty;
}