namespace cleanCore.UI
{
    public static class Merchant
    {
        public static bool CanRepair
        {
            get
            {
                var ret = WoWScript.Execute("CanMerchantRepair()");
                return ret.Count > 0 && (ret[0] == "1" || ret[1] == "true");
            }
        }

        public static int RepairAllCost
        {
            get { return int.Parse(WoWScript.Execute("GetRepairAllCost()")[0]); }
        }

        public static void RepairAll()
        {
            WoWScript.ExecuteNoResults("RepairAllItems()");
        }

        public static void SellAll(ItemQuality quality)
        {
            WoWScript.ExecuteNoResults("for i=0,4 do for j=1, GetContainerNumSlots(i) do l=GetContainerItemLink(i,j) if l then _,_,q=GetItemInfo(l) if q == " + (int)quality + " then UseContainerItem(i,j) end end end end");
        }
    }
}