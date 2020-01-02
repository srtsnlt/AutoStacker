using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AutoStacker.Worlds
{
	public class ItemGrowerChest : Terraria.ModLoader.ModWorld
	{
		
		private int moonPhasePrev;
		private int time2Prev;
		private int timeStep=10;
		private Dictionary<Chest,List<ChestItem>> chestItems = new Dictionary<Chest,List<ChestItem>>();
		
		
		public override void PreUpdate()
		{
			int moonPhase=Main.moonPhase;
			//time2 = Main.time + 16200 * Convert.ToInt32(Main.dayTime) + 70200 * (1-Convert.ToInt32(Main.dayTime))
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
			
			int itemGrowerChestType = mod.GetTile("ItemGrowerChest").Type;
			var chests = Main.chest.Where
			(
				chest => 
					   chest != null 
					&& Main.tile[chest.x, chest.y] != null 
					&& Main.tile[chest.x, chest.y].active() 
					&& Main.tile[chest.x, chest.y].type == itemGrowerChestType
			);
			
			foreach(Chest chest in chests)
			{
				
				if( !chestItems.ContainsKey(chest) )
				{
					int len = chest.item.Length;
					chestItems[chest]= new List<ChestItem>(len);
					for(int itemNo=0; itemNo < len; itemNo++)
					{
						chestItems[chest].Add(new ChestItem());
					}
				}
				
				for(int itemNo=0; itemNo < chestItems[chest].Count; itemNo++)
				{
					
					//if item is nothing
					if( chest.item[itemNo].stack == 0 )
					{
						//skip
					}
					
					//if change or add items
					else if
					( 
						   chestItems[chest][itemNo].type   != chest.item[itemNo].type  
						|| chestItems[chest][itemNo].stack  != chest.item[itemNo].stack 
						|| chestItems[chest][itemNo].prefix != chest.item[itemNo].prefix
					)
					{
						//init growing data
						chestItems[chest][itemNo].init
						(
							 chest.item[itemNo].type    
							,chest.item[itemNo].stack   
							,chest.item[itemNo].prefix  
							,chest.item[itemNo].maxStack
						);
					}
					
					//if over max time
					else if( chestItems[chest][itemNo].totalGrowPassTime2 > chestItems[chest][itemNo].maxGrowPassTime2 )
					{
						//stack max
						chest.item[itemNo].stack        = chestItems[chest][itemNo].maxStack;
						chestItems[chest][itemNo].stack = chestItems[chest][itemNo].maxStack;
					}
					
					//others
					else
					{
						//grow item
						long totalGrowPassTime2 = chestItems[chest][itemNo].totalGrowPassTime2 + passTime2;
						chestItems[chest][itemNo].totalGrowPassTime2 = totalGrowPassTime2;
						
						int nextStack = (int)Math.Pow(2, (double)totalGrowPassTime2 * 0.0000115740740740741);
						
						chest.item[itemNo].stack        = nextStack;
						chestItems[chest][itemNo].stack = nextStack;
					}
				}
			}
			time2Prev=time2;
			moonPhasePrev=moonPhase;
		}
		
		class ChestItem
		{
			public int type;
			public int stack;
			public int prefix;
			public int maxStack;
			
			public long maxGrowPassTime2;
			public long totalGrowPassTime2;
			
			public ChestItem()
			{
				type=0;
				stack=0;
				prefix=0;
				maxStack=0;
				
				maxGrowPassTime2=0;
				totalGrowPassTime2=0;
				
			}
			
			public void init(int type_, int stack_, int prefix_ ,int maxStack_)
			{
				type=type_;
				stack=stack_;
				prefix=prefix_;
				maxStack=maxStack_;
				
				maxGrowPassTime2  =(long) ( (double)Math.Log(maxStack_,2)*86400);
				totalGrowPassTime2=(long) ( (double)Math.Log(stack_,   2)*86400);
			}
		}
	}
}
