# Tildom.Services

**Tildom.Services** is a .NET package that provides services and wrappers for working with application state and Azure services, specifically Blob Storage and Azure Form Recognizer. The package includes utility classes to manage state in Blazor applications and interact with Azure Blob Storage and Azure Form Recognizer.

## Installation

To install Tildom.Services, run the following command in the NuGet Package Manager Console:

```bash
Install-Package Tildom.Services
```

## Features

- **AppState**: A class to manage and notify changes in application state, especially useful in Blazor applications.
- **AzureBlobStorageWrapper**: A class to simplify interaction with Azure Blob Storage.
- **AzureFormRecognizerWrapper**: A class to simplify interaction with Azure Form Recognizer services.

## Usage

### 1. AppState

The `AppState` class implements `INotifyPropertyChanged` and is designed to manage global state in Blazor applications. It can be consumed using the `CascadingValue` component.

#### Example in Blazor:

```razor
@page "/"

@using Tildom.Services
@inject AppState AppState

<CascadingValue Value="AppState" IsFixed="true">
  <StatusBar />
</CascadingValue>

@code {
    protected override void OnInitialized() {
        AppState.Message = "Hello from the AppState!";
    }
}
```

In the `StatusBar` component:

```razor
@code {
    [CascadingParameter] public AppState? AppState { get; set; }

    protected override void OnInitialized() {
        if (AppState != null) {
            AppState.PropertyChanged += OnAppStateChanged;
        }
    }

    private void OnAppStateChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(AppState.Message)) {
            StateHasChanged();
        }
    }
}
```

### 2. AzureBlobStorageWrapper

The `AzureBlobStorageWrapper` class wraps around the `BlobServiceClient` provided by the Azure SDK, making it easier to initialize and use Blob Storage in your application.

#### Example usage:

```csharp
using Tildom.Services;
using Azure.Storage.Blobs;

var blobStorageWrapper = new AzureBlobStorageWrapper("<your_connection_string>");
BlobContainerClient containerClient = blobStorageWrapper.ServiceClient.GetBlobContainerClient("<container_name>");
```

### 3. AzureFormRecognizerWrapper

The `AzureFormRecognizerWrapper` class simplifies interaction with the Azure Form Recognizer API. It wraps the `DocumentAnalysisClient` provided by the Azure SDK and manages settings like the API key, endpoint, and model ID.

#### Example usage:

```csharp
using Tildom.Services;
using Azure.AI.FormRecognizer.DocumentAnalysis;

var settings = new AzureFormRecognizerSettings {
    Endpoint = "<your_endpoint>",
    ApiKey = "<your_api_key>",
    ModelId = "<your_model_id>"
};

var formRecognizerWrapper = new AzureFormRecognizerWrapper(settings);
var client = formRecognizerWrapper.Client;
```

### Settings for AzureFormRecognizerWrapper

```csharp
public class AzureFormRecognizerSettings {
  public string Endpoint { get; init; } = string.Empty;
  public string ApiKey { get; init; } = string.Empty;
  public string ModelId { get; init; } = string.Empty;
}
```

## Contributing

Feel free to open issues or submit pull requests for improvements and features.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
