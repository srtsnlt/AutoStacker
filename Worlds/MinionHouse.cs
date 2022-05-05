using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AutoStacker.Worlds
{
	public class MinionHouse : ModSystem
	{
		Dictionary<int, Player> minionHousePlayer   = new Dictionary<int, Player>();
		Dictionary<int, int   > minionHousePlayerNo = new Dictionary<int, int   >();
		Dictionary<int, int   > minionHousePlayerAi = new Dictionary<int, int   >();
		
		public void init()
		{
			this.minionHousePlayer   = new Dictionary<int, Player>();
			this.minionHousePlayerNo = new Dictionary<int, int   >();
			this.minionHousePlayerAi = new Dictionary<int, int   >();
		}
		
		
		public override void PreUpdateWorld()
		{
			if(!Terraria.Program.LoadedEverything || !Terraria.Main.PlayerLoaded)
			{
				return;
			}
			
			// int minionHouseChestType = mod.GetTile("MinionHouse").Type;
			int minionHouseChestType = ModContent.TileType<Tiles.MinionHouse>();
			
			for(int chestNo = 0; chestNo < Main.chest.Length; chestNo ++)
			{
				Chest chest = Main.chest[chestNo];
				if(
					chest == null 
					|| Main.tile[chest.x, chest.y] == null 
					|| Main.tile[chest.x, chest.y].IsActuated
					|| Main.tile[chest.x, chest.y].TileType != minionHouseChestType
				)
				{
					continue;
				}
				
				//spawn player
				if(!minionHousePlayer.ContainsKey(chestNo))
				{
					for(int playerNo=0;playerNo<Main.player.Length;playerNo++)
					{
						if(Main.player[playerNo]==null || !Main.player[playerNo].active)
						{
							Main.player[playerNo] = (Player)Main.player[Main.myPlayer].Clone();

							


							minionHousePlayer[chestNo]   = Main.player[playerNo];
							minionHousePlayerNo[chestNo] = playerNo;
							minionHousePlayerAi[chestNo] = 10;
							
							Main.player[playerNo].active               =true;
							Main.player[playerNo].dead                 =false;
							Main.player[playerNo].nearbyActiveNPCs     =1f;
							Main.player[playerNo].townNPCs             =1f;
							Main.player[playerNo].name                 ="Minion House Keeper";
							Main.player[playerNo].velocity.X           =0f;
							Main.player[playerNo].velocity.Y           =0f;
							Main.player[playerNo].releaseDown          =false;
							Main.player[playerNo].releaseRight         =false;
							Main.player[playerNo].releaseHook          =false;
							Main.player[playerNo].releaseInventory     =false;
							Main.player[playerNo].releaseJump          =false;
							Main.player[playerNo].releaseLeft          =false;
							Main.player[playerNo].releaseMapFullscreen =false;
							Main.player[playerNo].releaseMapStyle      =false;
							Main.player[playerNo].releaseMount         =false;
							Main.player[playerNo].releaseQuickHeal     =false;
							Main.player[playerNo].releaseQuickMana     =false;
							Main.player[playerNo].releaseRight         =false;
							Main.player[playerNo].releaseSmart         =false;
							Main.player[playerNo].releaseThrow         =false;
							Main.player[playerNo].releaseUp            =false;
							Main.player[playerNo].releaseUseItem       =false;
							Main.player[playerNo].releaseUseTile       =false;
							Main.player[playerNo].selectedItem=0;
							
							Main.player[playerNo].position.X =chest.x * 16 - 16*2;
							Main.player[playerNo].position.Y =chest.y * 16 - 16*4;
							
							break;
						}
					}
				}
				
				//use item
				if(minionHousePlayerAi[chestNo] > 0)
				{
					minionHousePlayerAi[chestNo] -= 1;
					continue;
				}
				
				for(int itemNo = 0; itemNo < chest.item.Length; itemNo++)
				{
					Item item = chest.item[itemNo];
					
					if(item==null || item.IsAir || item.DamageType != DamageClass.Summon )
					{
						continue;
					}
					
					minionHousePlayerAi[chestNo] = 2 * 1000;
					
					int myPlayer = Main.myPlayer;
					Main.myPlayer = minionHousePlayerNo[chestNo];
					
					Item playerItem = minionHousePlayer[chestNo].inventory[0].Clone();
					minionHousePlayer[chestNo].inventory[0] = item.Clone();
					
					minionHousePlayer[chestNo].controlUseItem=true;
					minionHousePlayer[chestNo].releaseUseItem=true;
					minionHousePlayer[chestNo].ItemCheck(minionHousePlayerNo[chestNo]);
					minionHousePlayer[chestNo].controlUseItem=false;
					minionHousePlayer[chestNo].releaseUseItem=false;
					
					minionHousePlayer[chestNo].inventory[0] = playerItem;

					Main.myPlayer = myPlayer;
				}
				minionHousePlayer[chestNo].controlUseItem=false;
				minionHousePlayer[chestNo].releaseUseItem=false;
			}
			
			
			//take damage
			for(int projectileNo = 0; projectileNo < Main.projectile.Length; projectileNo++)
			{
				Projectile projectile=Main.projectile[projectileNo];
				
				if(!minionHousePlayerNo.ContainsValue(projectile.owner))
				{
					continue;
				}
				Player player = Main.player[projectile.owner];
				
				for(int npcNo=0; npcNo < Main.npc.Length; npcNo++)
				{
					NPC npc = Main.npc[npcNo];
					if (
						   projectile.position.X + (float)projectile.width  > npc.position.X 
						&& projectile.position.X < npc.position.X + (float)npc.width 
						&& projectile.position.Y + (float)projectile.height > npc.position.Y 
						&& projectile.position.Y < npc.position.Y + (float)npc.height 
						&& !npc.friendly 
					)
					{
						npc.StrikeNPC( projectile.damage, projectile.knockBack, (int)projectile.rotation, false, false, false);
					}
				}
			}
			
			//check destroyed chest -> player delete
			foreach(int chestNo in minionHousePlayer.Keys)
			{
				if(Main.chest[chestNo]==null)
				{
					foreach(int chestNo2 in minionHousePlayer.Keys)
					{
						Main.player[minionHousePlayerNo[chestNo2]] = new Player();
						Main.player[minionHousePlayerNo[chestNo2]].active=false;
					}
					this.init();
					break;
				}
			}
		}
	}
}
