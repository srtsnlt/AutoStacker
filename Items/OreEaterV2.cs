using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class OreEaterV2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.2");
			Tooltip.SetDefault("Summons a Pet Ore Eater Ver.2");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.useStyle = 1;
			item.shoot = mod.ProjectileType("OreEaterV2");
			item.width = 16;
			item.height = 30;
			item.UseSound = SoundID.Item2;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 8;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.buffType = mod.BuffType("OreEaterV2");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}