using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;

namespace AutoStacker.Players
{
	class RecieverChestSelector : ModPlayer
	{
		public Point16 topLeft = new Point16((short)0, (short)0);
		public bool autoSendEnabled = false;
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("topLeftX", topLeft.X);
			tag.Set("topLeftY", topLeft.Y);
			tag.Set("autoSendEnabled", autoSendEnabled);
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if( tag.HasTag("topLeftX") && tag.HasTag("topLeftY") && tag.HasTag("autoSendEnabled") )
			{
				topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
				autoSendEnabled = tag.GetBool("autoSendEnabled");
			}
		}
	}
}

