using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class HitWireRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hit Wire Rod");
			Tooltip.SetDefault("Useage\nClick : Hit Wire");
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
		
		
		// UseItem
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool UseItem(Player player)
		{
			Wiring.TripWire(Player.tileTargetX,Player.tileTargetY, 1, 1);
			return true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wire,100);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
