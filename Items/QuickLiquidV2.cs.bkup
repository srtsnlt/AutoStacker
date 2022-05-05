using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class QuickLiquidV2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quick Liquid V2");
			Tooltip.SetDefault("Useage\nRight click this item : ON/OFF Quick Liquid");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 500;
			item.createTile = mod.TileType("QuickLiquidV2");
		}
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			if(ModWorld.QuickLiquidV2.quickSwitch){
				ModWorld.QuickLiquidV2.quickSwitch=false;
				Main.NewText("Quick Liquid OFF!!");
			}else{
				ModWorld.QuickLiquidV2.quickSwitch=true;
				Main.NewText("Quick Liquid ON!!");
			}
			item.stack++;
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