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
	public class OreEaterV2 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.2");
			Description.SetDefault("");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			if(!Terraria.Program.LoadedEverything )//&& Terraria.Main.tilesLoaded))
			{
				return;
			}
			
			Players.OreEaterV2 modPlayer = player.GetModPlayer<Players.OreEaterV2>(mod);
			if(modPlayer.petV2 == null)
			{
				modPlayer.petV2 = new Projectiles.PetV2();
			}
			
			if(!modPlayer.oreEaterV2)
			{
				modPlayer.oreEaterV2 = true;
				bool petV2ProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("OreEaterV2")] <= 0;
				if (petV2ProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				{
					Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("OreEaterV2"), 0, 0f, player.whoAmI, 0f, 0f);
					for (int _type=Main.npcTexture.Length-1; _type >= 0; _type--)
					{
						if(Main.npcTexture[_type].ToString() == "AutoStacker/NPCs/OreEaterV2")
						{
							modPlayer.type=_type;
							
							foreach(var npc in Main.npc.Where( npc => npc.type == _type ))
							{
								npc.active = false;
							}
							
							modPlayer.index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, _type );
							modPlayer.npc = Main.npc[modPlayer.index];
							
							Projectiles.PetV2 petV2=modPlayer.petV2;
							petV2.initListA();
							petV2.routeListX.Clear();
							petV2.routeListY.Clear();
							
							break;
						}
					}
				}
			}
		}
	}
}