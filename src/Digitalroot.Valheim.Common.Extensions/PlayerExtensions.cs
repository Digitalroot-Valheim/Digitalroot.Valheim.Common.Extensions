using System.Collections.Generic;
using System.Linq;

namespace Digitalroot.Valheim.Common.Extensions
{
  /// <summary>
  /// Source: EpicLoot
  /// </summary>
  public static class PlayerExtensions
  {
    public static ZDO GetZDO(this Player player)
    {
      return player.m_nview.GetZDO();
    }

    public static List<ItemDrop.ItemData> GetEquipment(this Player player)
    {
      var results = new List<ItemDrop.ItemData>();
      if (player.m_rightItem != null)
        results.Add(player.m_rightItem);
      if (player.m_leftItem != null)
        results.Add(player.m_leftItem);
      if (player.m_chestItem != null)
        results.Add(player.m_chestItem);
      if (player.m_legItem != null)
        results.Add(player.m_legItem);
      if (player.m_helmetItem != null)
        results.Add(player.m_helmetItem);
      if (player.m_shoulderItem != null)
        results.Add(player.m_shoulderItem);
      if (player.m_utilityItem != null)
        results.Add(player.m_utilityItem);
      return results;
    }
    
    public static bool HasEquipmentOfType(this Player player, ItemDrop.ItemData.ItemType type)
    {
      return player.GetEquipment().Exists(x => x != null && x.m_shared.m_itemType == type);
    }

    public static ItemDrop.ItemData GetEquipmentOfType(this Player player, ItemDrop.ItemData.ItemType type)
    {
      return player.GetEquipment().FirstOrDefault(x => x != null && x.m_shared.m_itemType == type);
    }

    public static Player GetPlayerWithEquippedItem(ItemDrop.ItemData itemData)
    {
      return Player.m_players.FirstOrDefault(player => player.IsItemEquiped(itemData));
    }
  }
}