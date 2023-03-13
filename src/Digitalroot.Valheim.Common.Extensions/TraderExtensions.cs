namespace Digitalroot.Valheim.Common.Extensions;

public static class TraderExtensions
{
  public static bool IsHaldor(this Trader trader)
  {
    return trader != null && trader.m_name == "$npc_haldor";
  }
}
