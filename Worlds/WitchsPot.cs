using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Worlds
{
	public class WitchsPot : ModSystem
	{
		public List<int> chestNo =new List<int>();
		
		public override void PreUpdateWorld()
		{
			if(chestNo.Count() <= 0)
			{
				return;
			}
			
			//Change Rondom Item
			Chest chest = Main.chest[chestNo[0]];
			List<Item> items = new List<Item>();
			
			System.Random rand = new System.Random();
			for(int itemNo=0; itemNo < chest.item.Length; itemNo++)
			{
				//if item is nothing
				if( chest.item[itemNo].stack != 0 )
				{
					//Item Change
					do
					{
						try
						{
							chest.item[itemNo].SetDefaults( (int)(rand.Next( Main.item.Length -1 ) +1) );
						}
						finally
						{
							//nothing
						}
					}
					while(chest.item[itemNo].stack == 0);
				}
				
				items.Add(chest.item[itemNo].Clone());
			}
			
			//Change Chest Type
			Terraria.WorldGen.PlaceChestDirect(Main.chest[chestNo[0]].x, Main.chest[chestNo[0]].y +1, TileID.Containers, 0, chestNo[0]);
			
			//Copy Item
			chest = Main.chest[chestNo[0]];
			for(int itemNo =0; itemNo < chest.item.Length; itemNo++)
			{
				chest.item[itemNo] = items[itemNo].Clone();
			}
			
			//Delete Que
			if(chestNo.Count() >= 2 && chestNo[0] == chestNo[1])
			{
				chestNo.RemoveAt(1);
			}
			chestNo.RemoveAt(0);
		}
	}
}
