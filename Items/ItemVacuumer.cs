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
	public class ItemVacuumer : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Item Vacuumer");
			Tooltip.SetDefault("Useage\nRight click this item : ON/OFF Vaccume ");
		}
		
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 100;
			item.rare = 1;
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
			item.stack++;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
