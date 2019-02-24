using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.ObjectData;
using MagicStorage;
using MagicStorage.Components;

namespace AutoStacker.Items
{
	public class HitWireRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hit Wire Rod");
			Tooltip.SetDefault("Useage");
			Tooltip.SetDefault("Click : Hit Wire");
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
			recipe.AddIngredient(ItemID.Wire,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
