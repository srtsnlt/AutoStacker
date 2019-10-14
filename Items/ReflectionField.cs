using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class ReflectionField : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reflection Field");
			Tooltip.SetDefault("Enemys will be move away from you.");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.accessory = true;
		}
		
		int reflectDictance=16*8;
		int awayDictance=16*6;
		
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			foreach(NPC npc in Main.npc)
			{
				if(
					npc.townNPC
					|| npc.friendly
				)
				{
					continue;
				}
				Vector2 distance= player.Center - npc.Center;
				float distanceOffset = (float)(Math.Max(npc.width, npc.height) * 0.5);
				
				if(distanceOffset > Math.Abs(distance.X))
				{
					distance.X += (float)((distance.X >= 0 ? -1 : 1));
				}
				else
				{
					distance.X += (float)((distance.X >= 0 ? -1 : 1) * distanceOffset);
				}
				
				if(distanceOffset > Math.Abs(distance.Y))
				{
					distance.Y += (float)((distance.Y >= 0 ? -1 : 1));
				}
				else
				{
					distance.Y += (float)((distance.Y >= 0 ? -1 : 1) * distanceOffset);
				}
				
				float distanceSum = Math.Abs(distance.X) + Math.Abs(distance.Y);
				if(distanceSum <= reflectDictance)
				{
					npc.velocity.X = distance.X >= 0 ? -1 * Math.Abs(npc.velocity.X) : Math.Abs(npc.velocity.X);
					npc.velocity.Y = distance.Y >= 0 ? -1 * Math.Abs(npc.velocity.Y) : Math.Abs(npc.velocity.Y);
					if(distanceSum <= awayDictance)
					{
						npc.velocity.X  = distance.X >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						npc.velocity.Y  = distance.Y >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						
						npc.position.X += distance.X >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						npc.position.Y += distance.Y >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						
					}
				}
			}
			
			foreach(Projectile projectile in Main.projectile)
			{
				//if( projectile.owner == player.whoAmI && projectile.whoAmI == player.whoAmI && projectile.friendly)
				//if( projectile.friendly || (projectile.owner == player.whoAmI && projectile.whoAmI == player.whoAmI))
				//if(projectile.owner == player.whoAmI)
				if( projectile.owner == player.whoAmI  && (projectile.whoAmI == player.whoAmI || projectile.friendly))
				{
					continue;
				}
				
				Vector2 distance= player.Center - projectile.Center;
				float distanceOffset = (float)(Math.Max(projectile.width, projectile.height) * 0.5);
				
				if(distanceOffset > Math.Abs(distance.X))
				{
					distance.X += (float)((distance.X >= 0 ? -1 : 1));
				}
				else
				{
					distance.X += (float)((distance.X >= 0 ? -1 : 1) * distanceOffset);
				}
				
				if(distanceOffset > Math.Abs(distance.Y))
				{
					distance.Y += (float)((distance.Y >= 0 ? -1 : 1));
				}
				else
				{
					distance.Y += (float)((distance.Y >= 0 ? -1 : 1) * distanceOffset);
				}
				
				float distanceSum = Math.Abs(distance.X) + Math.Abs(distance.Y);
				if(distanceSum <= reflectDictance)
				{
					projectile.velocity.X = distance.X >= 0 ? -1 * Math.Abs(projectile.velocity.X) : Math.Abs(projectile.velocity.X);
					projectile.velocity.Y = distance.Y >= 0 ? -1 * Math.Abs(projectile.velocity.Y) : Math.Abs(projectile.velocity.Y);
					if(distanceSum <= awayDictance)
					{
						projectile.velocity.X  = distance.X >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						projectile.velocity.Y  = distance.Y >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						
						projectile.position.X += distance.X >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						projectile.position.Y += distance.Y >= 0 ? -32 + (distanceSum/awayDictance)*32 : 32 - (distanceSum/awayDictance)*32;
						
					}
					projectile.friendly = true;
					projectile.owner = player.whoAmI;
					projectile.whoAmI = player.whoAmI;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			return true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}