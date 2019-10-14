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
	public class OreEaterV5 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.5");
			Description.SetDefault("");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			if(!Terraria.Program.LoadedEverything )
			{
				return;
			}
			
			Players.OreEater modPlayer = player.GetModPlayer<Players.OreEater>();
			
			if(!modPlayer.oreEaterEnable)
			{
				modPlayer.oreEaterEnable = true;
				bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("OreEaterV5")] <= 0;
				if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				{
					
					for (int _type=Main.npcTexture.Length-1; _type >= 0; _type--)
					{
						string npcTexture = Main.npcTexture[_type].ToString();
						if(npcTexture == "AutoStacker/NPCs/OreEaterV5")
						{
							foreach(var npc in Main.npc.Where( npc => npc.type == _type ))
							{
								npc.active = false;
							}
							modPlayer.index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, _type );
							modPlayer.npc = Main.npc[modPlayer.index];
							NPC.setNPCName("Ore Eater",modPlayer.npc.type);
							
							Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("OreEaterV5"), 0, 0f, player.whoAmI, 0f, 0f);
							//modPlayer.pet.initListA();
							//modPlayer.pet.routeListX.Clear();
							//modPlayer.pet.routeListY.Clear();
							modPlayer.pet = new Projectiles.PetV5();
							break;
						}
					}
				}
			}
		}
	}
}