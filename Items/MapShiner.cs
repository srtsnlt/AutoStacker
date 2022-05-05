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
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 100;
			Item.rare = 1;
			Item.useStyle = 5;
			Item.useAnimation = 28;
			Item.useTime = 28;
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
			Item.stack++;
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
		
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
