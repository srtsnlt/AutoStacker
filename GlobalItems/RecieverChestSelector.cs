using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.GlobalItems
{
    class RecieverChestSelector : GlobalItem
    {
        public static short[] ExcludeItemList = new short[10] { ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin, ItemID.Heart, ItemID.CandyApple, ItemID.CandyCane, ItemID.Star, ItemID.SugarPlum, ItemID.SoulCake };
        private static Dictionary<int, List<int>> deleteQue = new Dictionary<int, List<int>>();


        public override bool OnPickup(Item item, Player player)
        {
            Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>();
            Point16 topLeft = modPlayer.topLeft;
            if
            (
                modPlayer.activeItem == null
                || (topLeft.X == -1 && topLeft.Y == -1)
                || !modPlayer.autoSendEnabled
                || ExcludeItemList.Where(x => x == item.type).Any()
                || item.stack <= 0
                || item.IsAir
            )
            {
                return true;
            }

            //Item depositItem=item.Clone();
            if (deposit(item, player))
            {
                item.SetDefaults(0, true);
                if (!deleteQue.ContainsKey(item.type))
                {
                    deleteQue[item.type] = new List<int>();
                }
                deleteQue[item.type].Add(item.stack);
            }
            return true;
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (deleteQue.ContainsKey(item.type))
            {
                item.stack -= deleteQue[item.type].Sum(stack => stack);
                if (item.stack <= 0)
                {
                    item.SetDefaults(0, true);
                }
                deleteQue.Remove(item.type);
            }
        }

        public bool deposit(Item item, Player player)
        {
            Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>();
            Point16 topLeft = modPlayer.topLeft;

            //chest
            int chestNo = Common.AutoStackerCommon.FindChest(topLeft.X, topLeft.Y);
            if (chestNo != -1)
            {
                //stack item
                for (int slot = 0; slot < Main.chest[chestNo].item.Length; slot++)
                {
                    if (Main.chest[chestNo].item[slot].stack == 0)
                    {
                        Main.chest[chestNo].item[slot] = item.Clone();
                        item.SetDefaults(0, true);
                        Wiring.TripWire(topLeft.X, topLeft.Y, 2, 2);
                        return true;
                    }

                    Item chestItem = Main.chest[chestNo].item[slot];
                    if (item.type == chestItem.type && item.prefix == chestItem.prefix && chestItem.stack < chestItem.maxStack)
                    {
                        int spaceLeft = chestItem.maxStack - chestItem.stack;
                        if (spaceLeft >= item.stack)
                        {
                            chestItem.stack += item.stack;
                            item.SetDefaults(0, true);
                            Wiring.TripWire(topLeft.X, topLeft.Y, 2, 2);
                            return true;
                        }
                        else
                        {
                            item.stack -= spaceLeft;
                            chestItem.stack = chestItem.maxStack;
                        }
                    }
                }
            }

            //storage heart
            // else if(AutoStacker.modMagicStorage != null || AutoStacker.modMagicStorageExtra != null)
            else if (AutoStacker.modMagicStorage != null)
            {
                Common.MagicStorageConnecter.InjectItem(topLeft, item);
                if (item.stack == 0)
                {
                    item.SetDefaults(0, true);
                    return true;
                }
            }
            return false;
        }
    }
}

