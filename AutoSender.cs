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
using Terraria.Localization;
using MagicStorage;
using MagicStorage.Components;

namespace AutoStacker
{
	class AutoSender : GlobalItem
	{
		public static Point16 topLeft = new Point16((short)0, (short)0);
		public static bool autoSendEnabled = false;
		public static short[] ExcludeItemList = new short[10]{ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin, ItemID.Heart, ItemID.CandyApple, ItemID.CandyCane, ItemID.Star, ItemID.SugarPlum, ItemID.SoulCake };
		
		public override bool OnPickup(Item item, Player player)
		{
			//chest
			if(autoSendEnabled && !ExcludeItemList.Where(x => x == item.type).Any() )
			{
				int chestNo=FindChest(topLeft.X,topLeft.Y);
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
					
					Wiring.TripWire(topLeft.X, topLeft.Y, 2, 2);
					
				}
				
				//storage heart
				else if(InjectItem(topLeft, item))
				{
					item.stack = 0;
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
		
		//Magic Storage
		private TEStorageHeart FindHeart(Point16 origin)
		{
			if( !TileEntity.ByPosition.ContainsKey(origin) )
				return null;
			
			TEStorageCenter tEStorageCenter = (TEStorageCenter)TileEntity.ByPosition[origin];
			if(tEStorageCenter == null)
				return null;
			
			TEStorageHeart heart = tEStorageCenter.GetHeart();
			return heart;
		}
		
		
		public bool InjectItem(Point16 origin, Item item)
		{
			int oldstack = item.stack;
			
			TEStorageHeart targetHeart = FindHeart(origin);
			if (targetHeart == null)
				return false;
			
			targetHeart.DepositItem(item);
			
			if (oldstack != item.stack)
			{
				HandleStorageItemChange(targetHeart);
				return true;
				
			}
			return false;
		}
		
		
		private void HandleStorageItemChange(TEStorageHeart heart)
		{
			if (Main.netMode == 2)
			{
				NetHelper.SendRefreshNetworkItems(heart.ID);
			}
			else if (Main.netMode == 0)
			{
				StorageGUI.RefreshItems();
			}
		}
	}
}



