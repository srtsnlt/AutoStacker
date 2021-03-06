using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class OreEaterV1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.1");
			string str = "Summons a Pet Ore Eater Ver.1\n";
			str +=       "[status] \n";
			str +=       "ore serch range      : 10\n";
			str +=       "speed                : 3\n";
			str +=       "pick in water        : disenable\n";
			str +=       "pick in lava         : disenable\n";
			str +=       "through block        : disenable\n";
			str +=       "through unreveal map : disenable\n";
			str +=       "light                : none";
			Tooltip.SetDefault(str);
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.useStyle = 1;
			item.shoot = mod.ProjectileType("OreEaterV1");
			item.width = 16;
			item.height = 30;
			item.UseSound = SoundID.Item2;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 8;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.buffType = mod.BuffType("OreEaterV1");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(Terraria.ID.ItemID.IronPickaxe, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(Terraria.ID.ItemID.LeadPickaxe, 1);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.SetResult(this);
			recipe2.AddRecipe();

		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.ClearBuff(mod.BuffType("OreEaterV1"));
				player.ClearBuff(mod.BuffType("OreEaterV2"));
				player.ClearBuff(mod.BuffType("OreEaterV3"));
				player.ClearBuff(mod.BuffType("OreEaterV4"));
				player.ClearBuff(mod.BuffType("OreEaterV5"));
				
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}