using System.Collections.ObjectModel;

namespace DotNetPoC.Functions.Shared;

public class ValueFromTokenResult(bool invalidSignature = false, bool isAltered = false, IEnumerable<string>? values = null)
{
  public bool InvalidSignature { get { return invalidSignature; } }
  public bool IsAltered { get { return isAltered; } }

  public IReadOnlyList<string> Values
  {
    get
    {
      values ??= [];
      return new ReadOnlyCollection<string>([.. values]);
    }
  }

}

