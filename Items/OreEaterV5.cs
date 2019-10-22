using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Items
{
	public class OreEaterV5 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.5");
			string str = "Summons a Pet Ore Eater Ver.5\n";
			str +=       "[status] \n";
			str +=       "ore serch range      : 50\n";
			str +=       "speed                : 5\n";
			str +=       "pick in water        : enable\n";
			str +=       "pick in lava         : enable\n";
			str +=       "through block        : enable\n";
			str +=       "through unreveal map : enable\n";
			str +=       "light                : super bright";
			Tooltip.SetDefault(str);
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.useStyle = 1;
			item.shoot = mod.ProjectileType("OreEaterV5");
			item.width = 16;
			item.height = 30;
			item.UseSound = SoundID.Item2;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 8;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.buffType = mod.BuffType("OreEaterV5");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"OreEaterV4", 1);
			recipe.AddIngredient(Terraria.ID.ItemID.TeleportationPotion, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
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