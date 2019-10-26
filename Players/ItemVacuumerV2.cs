using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Players
{
	public class ItemVacuumerV2 : ModPlayer
	{
		
		public static bool vacuumSwitch=false;
		public static int serchNumber=0;

		public ItemVacuumerV2()
		{
			vacuumSwitch = false;
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("vacuumSwitch", vacuumSwitch);
			
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if(tag.ContainsKey("vacuumSwitch"))
			{
				vacuumSwitch=tag.GetBool("vacuumSwitch");
			}
			else
			{
				vacuumSwitch=false;
			}
		}
		
		public override void PreUpdate()
		{
			if(vacuumSwitch)
			{
				Item item = Main.item[serchNumber];

				Player player = Main.LocalPlayer;
				if (item.active && item.noGrabDelay == 0 && !ItemLoader.GrabStyle(item, player) && ItemLoader.CanPickup(item, player))
				{
					Terraria.ModLoader.ItemLoader.OnPickup(item,Main.player[Main.myPlayer]);
				}
				serchNumber += 1;
				if(serchNumber >= Main.item.Length )
				{
					serchNumber=0;
				}
			}
		}
	}
}
