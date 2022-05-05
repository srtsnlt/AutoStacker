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
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Tiles.AutoPicker>();
		}
		
		// public override void AddRecipes()
		// {
		// 	ModRecipe recipe = new ModRecipe(mod);
		// 	recipe.AddIngredient(ItemID.Chest,1);
		// 	recipe.AddIngredient(ItemID.Wire,1);
		// 	recipe.AddTile(TileID.WorkBenches);
		// 	recipe.SetResult(this);
		// 	recipe.AddRecipe();
		// }

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.Chest, 1)
				.AddIngredient(ItemID.Wire, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
		
	}
}