using System;
using System.Linq;
using System.Collections.Generic;
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
		public static short[] ExcludeItemList = new short[10]{ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin, ItemID.Heart, ItemID.CandyApple, ItemID.CandyCane, ItemID.Star, ItemID.SugarPlum, ItemID.SoulCake };
		private static Dictionary<int,List<int>> depositQue = new Dictionary<int,List<int>>();
		
		
		public override bool OnPickup(Item item, Player player)
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>(mod);
			Point16 topLeft=modPlayer.topLeft;
			if
			(
				modPlayer.activeItem == null 
				|| (topLeft.X == -1 && topLeft.Y == -1) 
				|| !modPlayer.autoSendEnabled 
				|| ExcludeItemList.Where(x => x == item.type).Any()
				|| item.stack <= 0
			)
			{
				return true;
			}
			
			if(!depositQue.ContainsKey(item.type))
			{
				depositQue[item.type] = new List<int>();
			}
			depositQue[item.type].Add(item.stack);
			return true;
		}
		
		public override void UpdateInventory(Item item, Player player)
		{
			if(depositQue.ContainsKey(item.type))
			{
				Item depositItem=item.Clone();
				depositItem.stack=depositQue[item.type].Sum( stack => stack );
				item.stack -= depositItem.stack;
				if(depositItem.stack > 0)
				{
					depositQue.Remove(item.type);
					deposit(depositItem, player);
				}
			}
		}
		
		public bool deposit(Item item, Player player)
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>(mod);
			Point16 topLeft=modPlayer.topLeft;
			
			//chest
			int chestNo=FindChest(topLeft.X,topLeft.Y);
			if(chestNo != -1)
			{
				//stack item
				for (int slot = 0; slot < Main.chest[chestNo].item.Length; slot++)
				{
					if (Main.chest[chestNo].item[slot].stack==0)
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
				Wiring.TripWire(topLeft.X, topLeft.Y, 2, 2);
			}
			
			//storage heart
			else if(AutoStacker.modMagicStorage != null)
			{
				if(Common.MagicStorageConnecter.InjectItem(topLeft, item))
				{
					item.stack = 0;
				}
			}
			
			if(item.stack > 0)
			{
				player.GetItem(Main.myPlayer, item, false, false);
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

