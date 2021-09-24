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
			if(!TileEntity.ByPosition.ContainsKey(origin))
				return null;

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

			if(regexMagicStorage.IsMatch(tEStorageCenter.GetType().Assembly.GetName().Name))
			{
				Common.MagicStorageAdapter.DepositItem(tEStorageCenter,item);
				return true;
			}

			if(regexMagicStorageExtra.IsMatch(tEStorageCenter.GetType().Assembly.GetName().Name))
			{
				Common.MagicStorageExtraAdapter.DepositItem(tEStorageCenter,item);
				return true;
			}
			return false;
		}
	}
}
