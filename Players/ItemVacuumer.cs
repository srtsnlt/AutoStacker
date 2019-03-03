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
	public class ItemVacuumer : ModPlayer
	{
		
		public static bool vacuumSwitch = false;
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("vacuumSwitch", vacuumSwitch);
			
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if(tag.HasTag("vacuumSwitch"))
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
			if(vacuumSwitch){
				Player player = Main.LocalPlayer;
				int velocity = 12;
				foreach (Item item in Main.item)
				{
					if (item.active && item.noGrabDelay == 0 && !ItemLoader.GrabStyle(item, player) && ItemLoader.CanPickup(item, player))
					{
						int distanceX   = (int)player.Center.X - (int)item.position.X;
						int distanceY   = (int)player.Center.Y - (int)item.position.Y;
						int distanceSum = System.Math.Abs(distanceX) + System.Math.Abs(distanceY);
						if(distanceSum > 0){
							if( item.velocity.X <= 1 && item.velocity.Y <= 1){
								item.velocity.X = velocity * distanceX / distanceSum;
								item.velocity.Y = velocity * distanceY / distanceSum;
								item.position.X += item.velocity.X;
								item.position.Y += item.velocity.Y;
							}
							else
							{
								item.velocity.X = velocity * distanceX / distanceSum;
								item.velocity.Y = velocity * distanceY / distanceSum;
							}
						}
					}
				}
			}
		}
	}
}
