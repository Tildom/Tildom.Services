using System.ComponentModel;

namespace Tildom.Services;

public class AppState : INotifyPropertyChanged {

  private string message = string.Empty;

  /// <summary>
  /// Gets or sets a message to consuming components.
  /// Example in Blazor:
  /// <CascadingValue Value="AppState" IsFixed="true">
  ///   @Body
  ///   <StatusBar />
  /// </CascadingValue>
  /// Here the StatusBar component can consume the AppState service and display a message that is set somewhere else such as globally.
  /// </summary>
  public string Message {
    get => message;
    set {
      if (value == null) {
        message = string.Empty;
      }
      if (message != value) {
        message = value ?? string.Empty;
        OnPropertyChanged(nameof(Message));
      }
    }
  }

  /// <summary>
  /// Occurs when a property value changes.
  /// </summary>
  public event PropertyChangedEventHandler? PropertyChanged;

  /// <summary>
  /// Raises the <see cref="PropertyChanged"/> event for the specified property.
  /// </summary>
  /// <param name="propertyName">Name of the property that changed.</param>
  protected void OnPropertyChanged(string propertyName) {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}

