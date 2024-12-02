namespace Tildom.Services.Abstractions.Blazor;

public class AppStateService {
  private string _message = string.Empty;

  public string Message {
    get => _message;
    set {
      if (_message != value) {
        _message = value;
        OnMessageChanged?.Invoke(_message);
      }
    }
  }

  public event Action<string>? OnMessageChanged;
}
