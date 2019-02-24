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
			Tooltip.SetDefault("Useage");
			Tooltip.SetDefault("Right click this item : ON/OFF Vaccume ");
		}
		
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 100;
			item.rare = 1;
		}
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("vacuumSwitch", AutoStackerPlayer.vacuumSwitch);
			
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			AutoStackerPlayer.vacuumSwitch = tag.GetBool("vacuumSwitch");
		}
		
		// RightClick
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			if(AutoStackerPlayer.vacuumSwitch){
				AutoStackerPlayer.vacuumSwitch=false;
				Main.NewText("Vacuume ON!!");
			}else{
				AutoStackerPlayer.vacuumSwitch=true;
				Main.NewText("Vacuume OFF!!");
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
