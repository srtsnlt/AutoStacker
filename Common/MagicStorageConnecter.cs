using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.Localization;
using MagicStorage;
using MagicStorage.Components;


namespace AutoStacker.Common
{
	class MagicStorageConnecter
	{
		
		//Magic Storage
		public static TEStorageHeart FindHeart(Point16 origin)
		{
			if( !TileEntity.ByPosition.ContainsKey(origin) )
				return null;
			
			var tEStorageCenter = TileEntity.ByPosition[origin];
			if(tEStorageCenter == null || tEStorageCenter.GetType().Name != "TEStorageHeart")
				return null;
			
			TEStorageHeart heart = ((TEStorageCenter)tEStorageCenter).GetHeart();
			return heart;
			
		}
		
		public static bool InjectItem(Point16 origin, Item item)
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
		
		private static void HandleStorageItemChange(TEStorageHeart heart)
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



