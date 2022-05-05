using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class QuickLiquid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quick Liquid");
			Tooltip.SetDefault("Useage\nRight click this item : ON/OFF Quick Liquid");
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
			// Item.createTile = ModContent.TileType<Tiles.QuickLiquid>();
		}
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			if(ModWorld.QuickLiquid.quickSwitch){
				ModWorld.QuickLiquid.quickSwitch=false;
				Main.NewText("Quick Liquid OFF!!");
			}else{
				ModWorld.QuickLiquid.quickSwitch=true;
				Main.NewText("Quick Liquid ON!!");
			}
			Item.stack++;
		}
		
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
		
	}
}