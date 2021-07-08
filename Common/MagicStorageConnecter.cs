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
using System.Text.RegularExpressions;

namespace AutoStacker.Common
{
	class MagicStorageConnecter
	{

		private static Regex regexMagicStorage = new Regex("^MagicStorage(?!Extra)");
		private static Regex regexMagicStorageExtra = new Regex("^MagicStorageExtra");

		//Magic Storage
		public static TileEntity FindHeart(Point16 origin)
		{
			var tEStorageCenter = TileEntity.ByPosition[origin];
			if(tEStorageCenter == null || tEStorageCenter.GetType().Name != "TEStorageHeart")
				return null;
			
			return tEStorageCenter;
		}

		public static bool InjectItem(Point16 origin, Item item)
		{
			TileEntity tEStorageCenter = FindHeart(origin);
			if (tEStorageCenter == null)
				return false;

			// Main.NewText(tEStorageCenter.GetType().Assembly.GetName().Name);
			
			int oldstack = item.stack;

			if( regexMagicStorage.IsMatch(tEStorageCenter.GetType().Assembly.GetName().Name))
			{
				MagicStorage.Components.TEStorageHeart heart = ((MagicStorage.Components.TEStorageCenter)tEStorageCenter).GetHeart();
				heart.DepositItem(item);

				if (oldstack != item.stack)
				{
					if (Main.netMode == 2)
					{
						MagicStorage.NetHelper.SendRefreshNetworkItems(heart.ID);
					}
					else if (Main.netMode == 0)
					{
						MagicStorage.StorageGUI.RefreshItems();
					}
				}
				return true;
			}

			if(regexMagicStorageExtra.IsMatch(tEStorageCenter.GetType().Assembly.GetName().Name))
			{
				MagicStorageExtra.Components.TEStorageHeart heart = ((MagicStorageExtra.Components.TEStorageCenter)tEStorageCenter).GetHeart();
				heart.DepositItem(item);

				if (oldstack != item.stack)
				{
					if (Main.netMode == 2)
					{
						MagicStorageExtra.NetHelper.SendRefreshNetworkItems(heart.ID);
					}
					else if (Main.netMode == 0)
					{
						MagicStorageExtra.StorageGUI.RefreshItems();
					}
					
				}
				return true;
			}
			return false;
		}
	}
}
