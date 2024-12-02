namespace Tildom.Services.Abstractions.BitWarden;

public class SecretNameAttribute : Attribute {
  public string Name { get; }
  public SecretNameAttribute(string name) {
    Name = name;
  }
}
