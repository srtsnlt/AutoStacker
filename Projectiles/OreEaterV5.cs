using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace AutoStacker.Projectiles
{
	public class OreEaterV5 : OreEaterBase
	{
		string displayName="Ore Eater Ver.5";
		
		public OreEaterV5()
		{
			this.maxSerchNum= 40;
			this.speed=16 * 5;
			this.light = 4f;
		}
		
		public override void AI()
		{
			if(!Terraria.Program.LoadedEverything )
			{
				return;
			}
			
			Player player = Main.player[projectile.owner];
			Players.OreEater modPlayer = player.GetModPlayer<Players.OreEater>();
			
			if(modPlayer.pet == null)
			{
				modPlayer.pet = (PetBase)new PetV5();
			}
			AI2(player, modPlayer, (PetBase)modPlayer.pet);
			
		}
	}
	
	public class PetV5 : PetBase
	{
		public override bool checkCanMove(int index, int dX, int dY, int pickPower)
		{
			Tile tile = Main.tile[AX[index], AY[index]];
			
			if
			(
				(
					!petDictionaryA.ContainsKey(AX[index] + dX) 
					|| !petDictionaryA[AX[index] + dX].ContainsKey(AY[index] + dY) 
				)
				&& AX[index] + dX < Main.Map.MaxWidth
				&& AX[index] + dX > 1
				&& AY[index] + dY < Main.Map.MaxHeight
				&& AY[index] + dY > 1
				//&& Main.Map.IsRevealed(AX[index] + dX,AY[index] + dY)
				//&& 
				//(
				//	tile.liquid == 0 
				//	|| tile.liquid == 1 
				//	|| tile.liquid == 2 
				//)
				//&&
				//(
				//	tile == null 
				//	||
				//	(
				//		tile != null 
				//		&&
				//		(
				//			!tile.active()
				//			||
				//			(
				//				tile.active()
				//				&& 
				//				(
				//					oreTile.ContainsKey(tile.type)
				//					&& oreTile[tile.type]
				//				)
				//			)
				//		)
				//	)
				//)
			)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
