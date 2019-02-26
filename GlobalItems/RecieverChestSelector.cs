using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;

namespace AutoStacker.GlobalItems
{
	class RecieverChestSelector : GlobalItem
	{
		//public static Point16 topLeft = new Point16((short)0, (short)0);
		//public static bool autoSendEnabled = false;
		public static short[] ExcludeItemList = new short[10]{ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin, ItemID.Heart, ItemID.CandyApple, ItemID.CandyCane, ItemID.Star, ItemID.SugarPlum, ItemID.SoulCake };
		
		public override bool OnPickup(Item item, Player player)
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>(mod);
			
			//chest
			if(modPlayer.autoSendEnabled && !ExcludeItemList.Where(x => x == item.type).Any() )
			{
				int chestNo=FindChest(modPlayer.topLeft.X,modPlayer.topLeft.Y);
				if(chestNo != -1)
				{
					//stack item
					for (int slot = 0; slot < Main.chest[chestNo].item.Length; slot++)
					{
						if (Main.chest[chestNo].item[slot].IsAir)
						{
							Main.chest[chestNo].item[slot] = item.Clone();
							item.stack=0;
							break;
						}
						
						Item chestItem = Main.chest[chestNo].item[slot];
						if (item.IsTheSameAs(chestItem) && chestItem.stack < chestItem.maxStack)
						{
							int spaceLeft = chestItem.maxStack - chestItem.stack;
							if (spaceLeft >= item.stack)
							{
								chestItem.stack += item.stack;
								item.stack = 0;
								break;
							}
							else
							{
								item.stack -= spaceLeft;
								chestItem.stack = chestItem.maxStack;
							}
						}
					}
					
					Wiring.TripWire(modPlayer.topLeft.X, modPlayer.topLeft.Y, 2, 2);
				}
				
				//storage heart
				else if(AutoStacker.modMagicStorage != null)
				{
					if(Common.MagicStorageConnecter.InjectItem(modPlayer.topLeft, item))
					{
						item.stack = 0;
					}
				}
			}
			return true;
		}
		
		public static int FindChest(int originX, int originY)
		{
			Tile tile = Main.tile[originX, originY];
			if (tile == null || !tile.active())
				return -1;

			if (!Chest.isLocked(originX, originY))
				return Chest.FindChest(originX, originY);
			else
				return -1;
		}
	}
}

