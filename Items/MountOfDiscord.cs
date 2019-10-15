using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class MountOfDiscord : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("MountOfDiscord.");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.value = 30000;
			item.rare = 2;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.mountType = ModContent.MountType<Mounts.MountOfDiscord>();
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(Terraria.ID.ItemID.Glass, 9999);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}