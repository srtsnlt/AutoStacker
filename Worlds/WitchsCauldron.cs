using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AutoStacker.Worlds
{
	public class WitchsCauldron : ModSystem
	{
		private int moonPhasePrev;
		private int time2Prev;
		private int timeStep=10;
		System.Random rand = new System.Random();

		private System.Reflection.FieldInfo[] propList;
		
		public WitchsCauldron()
		{
			propList = typeof(Terraria.ID.ItemID).GetFields();
		}

		public override void PreUpdateWorld()
		{
			int moonPhase=Main.moonPhase;
			int time2 = (int)Main.time + 70200 - 54000 * Convert.ToInt32(Main.dayTime);
			int passTime2 = time2 - time2Prev + (moonPhase == moonPhasePrev ? 0 : 1) * 86400 ;
			
			if(passTime2 > 3600){
				time2Prev=time2 - time2 % timeStep;
				moonPhasePrev=moonPhase;
				return;
			}
			
			if(passTime2 < timeStep ){
				return;
			}
			int WitchsCauldronChestType = ModContent.TileType<Tiles.WitchsCauldron>();
			var chests = Main.chest.Where
			(
				chest => 
					   chest != null 
					&& Main.tile[chest.x, chest.y] != null 
					&& !Main.tile[chest.x, chest.y].IsActuated
					&& Main.tile[chest.x, chest.y].TileType == WitchsCauldronChestType
			);
			
			
			foreach(Chest chest in chests)
			{
				for(int itemNo=0; itemNo < chest.item.Length; itemNo++)
				{
					
					//if item is nothing
					if( chest.item[itemNo].stack == 0 )
					{
						//skip
					}
					
					//if change or add items
					else
					{
						//Item Change
						if(rand.Next( 60*60*24 ) <= passTime2)
						{
							do
							{
								try
								{
									Int32 randomNo=Convert.ToInt32(rand.Next( propList.Length ));
									System.Reflection.FieldInfo prop = propList[randomNo];
									Int32 propValue = Convert.ToInt32(prop.GetValue(prop));
									chest.item[itemNo].SetDefaults( propValue );
								}
								finally
								{
									//nothing
								}
							}
							while(chest.item[itemNo].stack == 0 || chest.item[itemNo].IsAir);
							
						}
					}
				}
			}
			time2Prev=time2;
			moonPhasePrev=moonPhase;
		}
	}
}
