
#if NETSTANDARD

namespace System.Runtime.InteropServices
{
  [AttributeUsage(AttributeTargets.Method, Inherited = false)]
  public sealed class UnmanagedCallersOnlyAttribute : Attribute
  {
    public UnmanagedCallersOnlyAttribute() { }
    public Type[] CallConvs;
    public string EntryPoint;
  }
}

#endif

namespace Microsoft.JavaScript.NodeApi
{

// The Unity linker will recognize this by name
public sealed class PreserveAttribute : System.Attribute
{
}

}
