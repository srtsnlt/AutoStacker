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
			Item.width = 39;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Tiles.WitchsCauldron>();
		}
		

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.Chest,1)
				.AddIngredient(ItemID.BlackCounterweight,1)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.Chest,1)
				.AddIngredient(ItemID.BlueCounterweight,1)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.Chest,1)
				.AddIngredient(ItemID.GreenCounterweight,1)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.Chest,1)
				.AddIngredient(ItemID.PurpleCounterweight,1)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.Chest,1)
				.AddIngredient(ItemID.RedCounterweight,1)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.Chest,1)
				.AddIngredient(ItemID.YellowCounterweight,1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}