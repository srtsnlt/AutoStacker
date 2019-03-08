using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AutoStacker.Buffs
{
	public class OreEater : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Annoying Light");
			Description.SetDefault("Ugh, soooo annoying");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			Players.OreEater modPlayer = player.GetModPlayer<Players.OreEater>(mod);
			if(!modPlayer.oreEater)
			{
				modPlayer.oreEater = true;
				bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("OreEater")] <= 0;
				if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				{
					Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("OreEater"), 0, 0f, player.whoAmI, 0f, 0f);
					for (int _type=Main.npcTexture.Length-1; _type >= 0; _type--)
					{
						if(Main.npcTexture[_type].ToString() == "AutoStacker/NPCs/OreEater")
						{
							modPlayer.type=_type;
							//modPlayer.index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, _type );
							Projectiles.Pet pet=modPlayer.pet;
							pet.initListA();
							pet.routeListX.Clear();
							pet.routeListY.Clear();
							
							break;
						}
					}
				}
			}
		}
	}
}