using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class WitchsCauldron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Witch's Cauldron");
			Tooltip.SetDefault("This is a modded chest.");
		}

		public override void SetDefaults()
		{
			item.width = 39;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 500;
			item.createTile = mod.TileType("WitchsCauldron");
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe;
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chest,1);
			recipe.AddIngredient(ItemID.BlackCounterweight,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chest,1);
			recipe.AddIngredient(ItemID.BlueCounterweight,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chest,1);
			recipe.AddIngredient(ItemID.GreenCounterweight,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chest,1);
			recipe.AddIngredient(ItemID.PurpleCounterweight,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chest,1);
			recipe.AddIngredient(ItemID.RedCounterweight,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chest,1);
			recipe.AddIngredient(ItemID.YellowCounterweight,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
		
	}
}