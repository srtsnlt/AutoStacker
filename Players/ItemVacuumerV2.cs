using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace AutoStacker.Players
{
	public class ItemVacuumerV2 : ModPlayer
	{
		public static bool vacuumSwitch=false;
		public int serchNumber=0;
		Vector2 prePosition=Main.LocalPlayer.Center;

		public ItemVacuumerV2()
		{
			vacuumSwitch = false;
		}

		public override void PreUpdate()
		{
			if(vacuumSwitch)
			{
				Item item = Main.item[serchNumber];
				Player player = Main.LocalPlayer;
				if (item.active && item.noGrabDelay == 0 && !ItemLoader.GrabStyle(item, player) && ItemLoader.CanPickup(item, player))
				{
					item.position = player.Center + (player.Center - prePosition);
					if(item.position.X < 0)
					{
						item.position.X = 0;
					}
					else if(item.position.X > Main.rightWorld)
					{
						item.position.X = Main.rightWorld;
					}
					if(item.position.Y < 0)
					{
						item.position.Y = 0;
					}
					else if(item.position.Y > Main.bottomWorld)
					{
						item.position.Y = Main.bottomWorld;
					}
				}
				serchNumber += 1;
				if(serchNumber >= Main.item.Length )
				{
					serchNumber=0;
				}
			}
			prePosition=Main.LocalPlayer.Center;
		}
	}
}
