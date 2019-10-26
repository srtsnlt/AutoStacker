using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class AutoPicker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Auto Picker");
			Tooltip.SetDefault("This is a modded chest.");
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
			item.createTile = mod.TileType("AutoPicker");
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"WitchsCauldron", 1);
			recipe.AddIngredient(ItemID.Switch,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}