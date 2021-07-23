using UnityEngine;

namespace Digitalroot.Valheim.Common.Extensions
{
  public static class GameObjectExtensions
  {
    public static GameObject AddMonoBehaviour(this GameObject gameObject, string name)
    {
      Type type = Assembly.GetExecutingAssembly().GetType(name) ?? MonoBehaviourRepositoryAssembly.GetType(name);

      if (type == null)
      {
        throw new ArgumentException($"Unable to find MonoBehaviour: {name}", nameof(name));
      }

      gameObject.AddComponent(type);
      return gameObject;
    }

    public static GameObject AddMonoBehaviour<T>(this GameObject gameObject) where T : MonoBehaviour
    {
      gameObject.AddComponent<T>();
      return gameObject;
    }

    public static MonoBehaviour GetOrAddMonoBehaviour(this GameObject gameObject, string name)
    {
      return (gameObject.GetComponent(name) ?? gameObject.AddMonoBehaviour(name).GetComponent(name)) as MonoBehaviour;
    }
    
    /// <summary>
    /// Returns the component of Type type. If one doesn't already exist on the GameObject it will be added.
    /// Source: Jotunn JVL
    /// </summary>
    /// <remarks>Source: https://wiki.unity3d.com/index.php/GetOrAddComponent</remarks>
    /// <typeparam name="T">The type of Component to return.</typeparam>
    /// <param name="gameObject">The GameObject this Component is attached to.</param>
    /// <returns>Component</returns>
    public static T GetOrAddMonoBehaviour<T>(this GameObject gameObject) where T : MonoBehaviour
    {
      return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
  }
}
