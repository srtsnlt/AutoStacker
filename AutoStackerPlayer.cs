using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker
{
	public class AutoStackerPlayer : ModPlayer
	{
		
		public static bool vacuumSwitch = true;
		private int moonPhasePrev=Main.moonPhase;
		
		public override void PreUpdate()
		{
			if(!vacuumSwitch){
				Player player = Main.LocalPlayer;
				int velocity = 8;
				foreach (Item item in Main.item)
				{
					if (item.active && item.noGrabDelay == 0 && !ItemLoader.GrabStyle(item, player) && ItemLoader.CanPickup(item, player))
					{
						//item.beingGrabbed = true;
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
			
			if(moonPhasePrev!=Main.moonPhase){
				moonPhasePrev=Main.moonPhase;
				int itemGrowChestType = mod.GetTile("ItemGrowChest").Type;
				
				var items = Main.chest.Where( chest => chest != null && Main.tile[chest.x, chest.y] != null && Main.tile[chest.x, chest.y].active() && Main.tile[chest.x, chest.y].type == itemGrowChestType).SelectMany( chest => chest.item );
				foreach (var item in items)
				{
					if(item.stack *2 <= item.maxStack){
						item.stack *= 2;
					}
					else
					{
						item.stack = item.maxStack;
					}
				}
				
				Main.NewText("Items in ItemGrowChest have been Growing...");
				
			}
		}
	}
}
