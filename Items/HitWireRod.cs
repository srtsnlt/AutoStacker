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
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 100;
			Item.rare = 1;
			Item.useStyle = 5;
			Item.useAnimation = 28;
			Item.useTime = 28;
		}
		
		
		// UseItem
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool? UseItem(Player player)
		{
			Wiring.TripWire(Player.tileTargetX,Player.tileTargetY, 1, 1);
			return true;
		}
		
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.Wire,100)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
		
	}
}
