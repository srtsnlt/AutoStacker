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
	class MagicStorageExtraAdapter
	{
		public static bool DepositItem(TileEntity tEStorageCenter, Item item)
		{
			int oldstack = item.stack;

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
	}
}
