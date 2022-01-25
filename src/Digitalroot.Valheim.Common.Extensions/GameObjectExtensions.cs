using JetBrains.Annotations;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming

namespace Digitalroot.Valheim.Common.Extensions
{
  public static class GameObjectExtensions
  {
    public static GameObject AddLedgeJumping(this GameObject prefab)
    {
      prefab.GetOrAddMonoBehaviour<AutoJumpLedge>();
      return prefab;
    }

    public static bool HasParent(this GameObject prefab) => prefab.transform.GetParent() != null;

    public static GameObject GetParent(this GameObject prefab) => prefab.HasParent() ? prefab.transform.GetParent().gameObject : null;

    public static bool IsBoss(this GameObject prefab) => prefab.GetComponent<Character>()?.IsBoss() ?? false;

    private static Vector3 GetScale(string itemName, Vector3 currentScale)
    {
      List<string> names = new()
      {
        "shield"
        , "axe"
        , "mace"
      };

      if (names.Contains(itemName.ToLowerInvariant()))
      {
        return Vector3.one * 2;
      }

      return Vector3.one;
    }

    public static string GetUniqueName(this GameObject prefab)
    {
      List<string> paths = new();

      var parent = prefab.transform.GetParent();

      while (parent != null)
      {
        paths.Add(parent.name);
        parent = parent.GetParent();
      }

      var sb = new StringBuilder();
      for (var i = paths.Count; i > 0; i--)
      {
        sb.Append(paths[i - 1]).Append('.');
      }

      sb.Append(prefab.name);
      return sb.ToString();
    }

    /// <summary>
    /// Sets De-spawn In Day to false;
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <returns></returns>
    public static GameObject AsDayWalker(this GameObject prefab)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;

      monsterAI.SetDespawnInDay(false);
      return prefab;
    }

    /// <summary>
    /// Sets De-spawn In Day to true;
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <returns></returns>
    public static GameObject AsNightStalker(this GameObject prefab)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;

      monsterAI.SetDespawnInDay(true);
      return prefab;
    }

    /// <summary>
    /// Configure MonsterAI to patrol spawn point.
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <returns></returns>
    public static GameObject AsSpawnPointPatroler(this GameObject prefab)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;

      if (prefab.HasParent())
      {
        prefab.AsSpawnPointPatroler(prefab.transform.parent.transform.position);
      }
      else
      {
        monsterAI.SetPatrolPoint();
      }

      return prefab;
    }

    /// <summary>
    /// Configure MonsterAI to patrol spawn point.
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <returns></returns>
    public static GameObject AsSpawnPointPatroler(this GameObject prefab, Vector3 point)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;

      monsterAI.SetPatrolPoint(point);

      return prefab;
    }

    /// <summary>
    /// Configure MonsterAI to jump randomly.
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <param name="min">Min random range.</param>
    /// <param name="max">Max random range.</param>
    /// <returns></returns>
    public static GameObject AsRandomJumper(this GameObject prefab, float min = 5f, float max = 9f)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;
      if (monsterAI.m_randomFly) return prefab;
      monsterAI.m_jumpInterval = Random.Range(min, max);
      return prefab;
    }

    /// <summary>
    /// Configure the base AI to not flee
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <returns></returns>
    public static GameObject AsNoFleeing(this GameObject prefab)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;
      monsterAI.m_fleeIfLowHealth = 0f;
      return prefab;
    }

    /// <summary>
    /// Sets Path Agent Type
    /// </summary>
    /// <param name="prefab">MonsterAI</param>
    /// <param name="agentType"></param>
    /// <returns></returns>
    public static GameObject SetPathAgentType(this GameObject prefab, Pathfinding.AgentType agentType)
    {
      var monsterAI = prefab.GetComponent<MonsterAI>();
      if (monsterAI == null) return prefab;
      if (monsterAI.m_randomFly) return prefab;

      monsterAI.m_pathAgentType = agentType;

      return prefab;
    }

    /// <summary>
    /// Set the level of the prefab.
    /// </summary>
    /// <param name="prefab">Character</param>
    /// <param name="levelMin"></param>
    /// <param name="levelMax"></param>
    /// <returns></returns>
    public static GameObject SetLevel(this GameObject prefab, int levelMin, int levelMax)
    {
      var level = levelMin == levelMax ? levelMax : Random.Range(levelMin, levelMax + 1);
      var character = prefab.GetComponent<Character>();
      character?.SetLevel(level);
      character?.SetupMaxHealth();

      // prefab.SendMessage("SetLevel", level, SendMessageOptions.RequireReceiver);
      return prefab;
    }

    /// <summary>
    /// Set prefab's local position.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public static GameObject SetLocalPosition(this GameObject prefab, int i)
    {
      switch (i)
      {
        case 1:
          prefab.transform.localPosition += Vector3.left * 2.5f;
          break;

        case 2:
          prefab.transform.localPosition += Vector3.right * 2.5f;
          break;

        case 3:
          prefab.transform.localPosition += Vector3.forward * 2.5f;
          break;

        case 4:
          prefab.transform.localPosition += Vector3.back * 2.5f;
          break;
      }

      return prefab;
    }

    /// <summary>
    /// Set prefab's local scale
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="scaleSize"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static GameObject SetLocalScale(this GameObject prefab, float scaleSize)
    {
      prefab.SetLocalScale(new Vector3(scaleSize, scaleSize, scaleSize));
      return prefab;
    }

    /// <summary>
    /// Set prefab's local scale
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="scaleSize"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static GameObject SetLocalScale(this GameObject prefab, Vector3 scaleSize)
    {
      var zNetView = prefab.GetComponent<ZNetView>();
      zNetView.m_syncInitialScale = true;
      zNetView.SetLocalScale(scaleSize);
      return prefab;
    }

    /// <summary>
    /// Set prefab's local scale
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public static GameObject SetLocalRotation(this GameObject prefab, Quaternion quaternion)
    {
      prefab.transform.rotation = quaternion;
      return prefab;
    }

    public static GameObject ScaleEquipment(this GameObject prefab)
    {
      var visEquipment = prefab.GetComponent<VisEquipment>();
      if (visEquipment == null) return prefab;
      if (visEquipment.m_leftItemInstance != null)
      {
        for (var i = 0; i < visEquipment.m_leftItemInstance.transform.childCount; i++)
        {
          var item = visEquipment.m_leftItemInstance.transform.GetChild(i);
          item.localScale = GetScale(item.gameObject.name, item.localScale);
        }
      }

      if (visEquipment.m_rightItemInstance != null)
      {
        for (var i = 0; i < visEquipment.m_rightItemInstance.transform.childCount; i++)
        {
          var item = visEquipment.m_rightItemInstance.transform.GetChild(i);
          item.localScale = GetScale(item.gameObject.name, item.localScale);
        }
      }

      return prefab;
    }

    /// <summary>
    /// Returns the component of Type type. If one doesn't already exist on the GameObject it will be added.
    /// </summary>
    /// <remarks>
    /// Inspired by Jotunn JVL
    /// Source: https://wiki.unity3d.com/index.php/GetOrAddComponent
    /// </remarks>
    /// <typeparam name="T">The type of Component to return.</typeparam>
    /// <param name="gameObject">The GameObject the Component is attached to.</param>
    /// <returns>Returns the component of Type T</returns>
    [UsedImplicitly]
    public static T GetOrAddMonoBehaviour<T>([NotNull] this GameObject gameObject)
      where T : MonoBehaviour
    {
      return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
  }
}
