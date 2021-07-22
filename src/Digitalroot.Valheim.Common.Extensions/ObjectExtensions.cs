using System.Linq;
using System.Reflection;

namespace Digitalroot.Valheim.Common.Extensions
{
  /// <summary>
  /// Source: https://stackoverflow.com/a/5114514
  /// </summary>
  public static class ObjectExtensions
  {
    public static bool HasMethod(this object objectToCheck, string methodName)
    {
      return objectToCheck.GetType().GetMethods().Any(m => m.Name == methodName);
      // return objectToCheck.GetType().GetMethod(methodName) != null;
    }

    public static MethodInfo GetMethod(this object objectToCheck, string methodName)
    {
      return !objectToCheck.HasMethod(methodName) ? null : objectToCheck.GetType().GetMethods().FirstOrDefault(m => m.Name == methodName);
    }

    public static bool HasProperty(this object objectToCheck, string property)
    {
      return objectToCheck.GetType().GetProperty(property) != null;
    }

    public static bool HasProperty<T>(this object objectToCheck, T property) where T : System.Enum 
    {
      return objectToCheck.GetType().GetProperty(property.ToString()) != null;
    }

    public static PropertyInfo GetProperty(this object objectToCheck, string property)
    {
      return !objectToCheck.HasProperty(property) ? null : objectToCheck.GetType().GetProperty(property);
    }

    public static PropertyInfo GetProperty<T>(this object objectToCheck, T property) where T : System.Enum
    {
      return !objectToCheck.HasProperty(property) ? null : objectToCheck.GetType().GetProperty(property.ToString());
    }
  }
}
