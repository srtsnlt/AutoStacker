using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class ItemVacuumer : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Item Vacuumer");
			Tooltip.SetDefault("Useage\nRight click this item : ON/OFF Vaccume ");

		}
		
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 100;
			Item.rare = 1;
		}

		// RightClick
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			if(Players.ItemVacuumer.vacuumSwitch){
				Players.ItemVacuumer.vacuumSwitch=false;
				Main.NewText("Vacuume OFF!!");
			}else{
				Players.ItemVacuumer.vacuumSwitch=true;
				Main.NewText("Vacuume ON!!");
			}
			Item.stack++;
		}
		
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.ReinforcedFishingPole,1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
