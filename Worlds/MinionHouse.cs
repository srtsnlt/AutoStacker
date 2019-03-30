using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Worlds
{
	public class MinionHouse : ModWorld
	{
		Dictionary<int, Player> minionHousePlayer   = new Dictionary<int, Player>();
		Dictionary<int, int   > minionHousePlayerNo = new Dictionary<int, int   >();
		
		public void init()
		{
			this.minionHousePlayer   = new Dictionary<int, Player>();
			this.minionHousePlayerNo = new Dictionary<int, int   >();
		}
		
		
		public override void PreUpdate()
		{
			if(!Terraria.Program.LoadedEverything )
			{
				return;
			}
			
			
			int minionHouseChestType = mod.GetTile("MinionHouse").Type;
			var chests = Main.chest.Where
			(
				chest => 
					chest != null 
					&& Main.tile[chest.x, chest.y] != null 
					&& Main.tile[chest.x, chest.y].active() 
					&& Main.tile[chest.x, chest.y].type == minionHouseChestType
			);
			
			for(int chestNo = 0; chestNo < Main.chest.Length; chestNo ++)
			{
				Chest chest = Main.chest[chestNo];
				if(
					chest == null 
					|| Main.tile[chest.x, chest.y] == null 
					|| !Main.tile[chest.x, chest.y].active() 
					|| Main.tile[chest.x, chest.y].type != minionHouseChestType
				)
				{
					continue;
				}
				
				//spawn player
				if(!minionHousePlayer.ContainsKey(chestNo))
				{
					for(int playerNo=0;playerNo<=255;playerNo++)
					{
						if(!Main.player[playerNo].active)
						{
							//Main.player[playerNo] = new Player();
							Main.player[playerNo] = (Player)Main.player[Main.myPlayer].Clone();
							//Main.player[playerNo] = Main.player[Main.myPlayer];
							
							minionHousePlayer[chestNo]   = Main.player[playerNo];
							minionHousePlayerNo[chestNo] = playerNo;
							
							Main.player[playerNo].active     =true;
							Main.player[playerNo].dead       =false;
							Main.player[playerNo].activeNPCs =1f;
							Main.player[playerNo].townNPCs   =1f;
							Main.player[playerNo].name       ="Minion House";
							Main.player[playerNo].position.X =chest.x * 16;
							Main.player[playerNo].position.Y =chest.y * 16;
							
							//Main.player[playerNo].index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, _type );
							//Main.player[playerNo].npc = Main.npc[Main.player[playerNo].index];
							//NPC.setNPCName("Ore Eater",Main.player[playerNo].type);
							
							//for(int i=0; i < Main.player[playerNo].hideInfo.Length; i++)
							//{
							//	Main.player[playerNo].hideInfo[i] = true;
							//	Main.player[playerNo].hideVisual.Add(true);
							//}
							
							break;
						}
					}
				}
				
				int myPlayer = Main.myPlayer;
				Main.myPlayer = minionHousePlayerNo[chestNo];
				Item playerItem = minionHousePlayer[chestNo].inventory[0].Clone();
				
				//use item
				for(int itemNo = 0; itemNo < chest.item.Length; itemNo++)
				{
					Item item = chest.item[itemNo];
					
					
					if(item.IsAir || item.buffType == 0)
					//if(item.IsAir)
					{
						continue;
					}
					
					if(minionHousePlayer[chestNo].HasBuff(item.buffType))
					//if(Main.LocalPlayer.HasBuff(item.buffType))
					{
						continue;
					}
					
					/*
					Main.LocalPlayer.inventory[0]=item.Clone();
					Main.LocalPlayer.selectedItem=0;
					Main.LocalPlayer.controlUseItem=true;
					Main.LocalPlayer.releaseUseItem=true;
					Main.LocalPlayer.ItemCheck(Main.myPlayer);
					*/
					
					
					minionHousePlayer[chestNo].inventory[0]=item.Clone();
					minionHousePlayer[chestNo].selectedItem=0;
					minionHousePlayer[chestNo].controlUseItem=true;
					minionHousePlayer[chestNo].releaseUseItem=true;
					minionHousePlayer[chestNo].ItemCheck(minionHousePlayerNo[chestNo]);
					
					//minionHousePlayer[chestNo].ItemCheck(Main.myPlayer);
					//Main.LocalPlayer.ItemCheck(Main.myPlayer);
					//Main.LocalPlayer.ItemCheck(minionHousePlayerNo[chestNo]);
					
				}
				minionHousePlayer[chestNo].inventory[0] = playerItem;
				Main.myPlayer = myPlayer;
				
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
					)
					{
						npc.StrikeNPC( projectile.damage, projectile.knockBack, (int)projectile.rotation, false, false, false);
					}
				}
				//Main.NewText("aaa");
			}
			
			
			
			
			
			//check destroyed chest -> player delete
		}
		
		class ChestInfo
		{
			
			
		}
		
	}
	
	
	
}
