using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class MapShiner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Map Shiner");
			Tooltip.SetDefault("Useage\nRight click this item : Map Shine");
		}
		
        public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 100;
			item.rare = 1;
			item.useStyle = 5;
			item.useAnimation = 28;
			item.useTime = 28;
		}
		
		public static bool mapShiner=false;

		// RightClick
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			if(mapShiner)
			{
				mapShiner=false;
				Main.NewText("Map Shine OFF!!");
			}
			else
			{
				mapShiner=true;
				Main.NewText("Map Shine ON!!");
			}
			item.stack++;
		}
		public override void UpdateInventory(Player player)
		{
			if(mapShiner)
			{
				for(int x = Main.mapMinX;x < Main.mapMaxX; x++)
				{
					for(int y=Main.mapMinY;y < Main.mapMaxY; y++)
					{
						Main.Map.Update(x, y, Byte.MaxValue);
					}
				}
			}
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
