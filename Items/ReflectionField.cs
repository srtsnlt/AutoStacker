using System;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	// [AutoloadEquip(EquipType.Shield)]
	public class ReflectionField : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reflection Field");
			Tooltip.SetDefault("Enemys will be move away from you.");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.buyPrice(0, 10, 0, 0);
			Item.accessory = true;
		}
		
		int reflectDictance=16*8;
		int awayDictance=16*6;
		
		// OrderedDictionary friendlyList = new OrderedDictionary();

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			NPC targetNpc = null;
			float distanceMin = float.MaxValue;

			foreach(NPC npc in Main.npc)
			{
				if(
					npc == null
					|| !npc.active
					|| npc.townNPC
					|| npc.friendly
					|| npc.damage == 0
				)
				{
					continue;
				}
				Vector2 distance= player.Center - npc.Center;

				distance.X -= (float)(Math.Sign(distance.X) * npc.width);
				distance.Y -= (float)(Math.Sign(distance.Y) * npc.height);

				float distanceSum = Math.Abs(distance.X) + Math.Abs(distance.Y);
				if(distanceSum < distanceMin && npc.active)
				{
					targetNpc = npc;
					distanceMin = distanceSum;
				}

				if(distanceSum <= reflectDictance)
				{
					if(distanceSum <= awayDictance)
					{
						npc.velocity.X  = -Math.Sign(distance.X) * (32 - (distanceSum/awayDictance)*32);
						npc.velocity.Y  = -Math.Sign(distance.Y) * (32 - (distanceSum/awayDictance)*32);
						
						npc.position.X += -Math.Sign(distance.X) * (32 - (distanceSum/awayDictance)*32);
						npc.position.Y += -Math.Sign(distance.Y) * (32 - (distanceSum/awayDictance)*32);
						
					}
					else
					{
						npc.velocity.X = -Math.Sign(distance.X) * Math.Abs(npc.velocity.X);
						npc.velocity.Y = -Math.Sign(distance.Y) * Math.Abs(npc.velocity.Y);
					}
				}
			}
			
			foreach(Projectile projectile in Main.projectile)
			{
				if( 
					projectile == null
					|| !projectile.active
					|| projectile.damage == 0 
					|| !projectile.hostile
					|| (
							projectile.minion 
							&& projectile.OwnerMinionAttackTargetNPC is null
						)
				)
				{
					continue;
				}

				Vector2 distance= player.Center - projectile.Center;
				distance.X -= (float)(Math.Sign(distance.X) * projectile.width);
				distance.Y -= (float)(Math.Sign(distance.Y) * projectile.height);

				float distanceSum = Math.Abs(distance.X) + Math.Abs(distance.Y);
				if(projectile.friendly)
				{
					if(targetNpc != null)
					{
						Vector2 distanceNPC= targetNpc.Center - projectile.Center;
						float distanceNPCSum = Math.Abs(distanceNPC.X) + Math.Abs(distanceNPC.Y);
						distanceNPC.X -= (float)(Math.Sign(distanceNPC.X) * projectile.width);
						distanceNPC.Y -= (float)(Math.Sign(distanceNPC.Y) * projectile.height);

						projectile.velocity.X = (float)( projectile.velocity.X * 0.9875 + distanceNPC.X * 0.00125 );
						projectile.velocity.Y = (float)( projectile.velocity.Y * 0.9875 + distanceNPC.Y * 0.00125 );
					}
				}
				else
				{
					if(distanceSum <= reflectDictance)
					{
						projectile.velocity.X = -Math.Sign(distance.X) * Math.Abs(projectile.velocity.X);
						projectile.velocity.Y = -Math.Sign(distance.Y) * Math.Abs(projectile.velocity.Y);
						projectile.friendly = true;
						projectile.penetrate = 1;
					}
					else
					{
						projectile.velocity.X = (float)( projectile.velocity.X * 0.9875 + distance.X * 0.00125 );
						projectile.velocity.Y = (float)( projectile.velocity.Y * 0.9875 + distance.Y * 0.00125 );
					}
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.MagicMirror,3)
				.AddTile(TileID.WorkBenches)
				.Register();
		}	
		
	}
}