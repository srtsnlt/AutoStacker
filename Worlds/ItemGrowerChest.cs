using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Worlds
{
	public class ItemGrowerChest : ModWorld
	{
		
		private int  moonPhasePrev;
		private bool firstCall = true;
		
		public override void PreUpdate()
		{
			if(firstCall)
			{
				moonPhasePrev=Main.moonPhase;
				firstCall=false;
			}
			
			if( moonPhasePrev!=Main.moonPhase ){
				moonPhasePrev=Main.moonPhase;
				int itemGrowerChestType = mod.GetTile("ItemGrowerChest").Type;
				var items = Main.chest.Where( chest => chest != null && Main.tile[chest.x, chest.y] != null && Main.tile[chest.x, chest.y].active() && Main.tile[chest.x, chest.y].type == itemGrowerChestType).SelectMany( chest => chest.item );
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
				
				Main.NewText("ItemGrowerChests are growing items...");
			}
		}
	}
}
