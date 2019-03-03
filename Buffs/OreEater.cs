using Microsoft.Xna.Framework;
using System;
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
			player.GetModPlayer<Players.OreEater>(mod).oreEater = true;
			//ModNPC modNPC = mod.GetNPC<NPCs.OreEater>();
			
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("OreEater")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("OreEater"), 0, 0f, player.whoAmI, 0f, 0f);
				
				//int index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, mod.NPCType<NPCs.OreEater>() );
				
				//Main.npc[ index ].SetDefaults(0);
				//Main.npc[ index ].position.X = player.position.X;
				//Main.npc[ index ].position.Y = player.position.Y;
				
			}
		}
	}
}