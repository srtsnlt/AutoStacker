using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Players
{
	public class ItemVacuumerV2 : ModPlayer
	{
		public static bool vacuumSwitch=false;
		public int serchNumber=0;

		public ItemVacuumerV2()
		{
			vacuumSwitch = false;
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("vacuumSwitchV2", vacuumSwitch);
			
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if(tag.ContainsKey("vacuumSwitchV2"))
			{
				vacuumSwitch=tag.GetBool("vacuumSwitchV2");
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
					item.position = player.Center;
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
