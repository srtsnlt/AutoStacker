using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Items
{
	public class AutoPickerController : ModItem
	{
		public Point16 topLeft = new Point16((short)-1,(short)-1);
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Auto Picker Controller");
			
			String tooltip_str ="Useage \n";
			tooltip_str       +="  Click chest           : Select Recever chest\n";
			tooltip_str       +="  Right click AutoPicker: Select AutoPicker\n";
			tooltip_str       +="  Right click this item : ON/OFF auto pick \n";
			Tooltip.SetDefault(tooltip_str);
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if(topLeft.X != -1 && topLeft.Y != -1)
			{
				TooltipLine lineH2 = new TooltipLine(mod, "head2", "ReceverChest [" + topLeft.X + "," + topLeft.Y + "]\n ");
				tooltips.Insert(2,lineH2);
			}
			else
			{
				TooltipLine lineH2 = new TooltipLine(mod, "head2", "Chest [ none ]\n ");
				tooltips.Insert(2,lineH2);
			}
			
		}
		
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 100;
			item.rare = 1;
			item.useStyle = 1;
			item.useAnimation = 28;
			item.useTime = 28;
			
		}
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("topLeftXR", topLeft.X);
			tag.Set("topLeftYR", topLeft.Y);
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if(tag.ContainsKey("topLeftXR") && tag.ContainsKey("topLeftYR"))
			{
				topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
			}
		}
		
		public override bool UseItem(Player player)
		{
			//Players.AutoPicker modPlayer = (Players.AutoPicker)Main.LocalPlayer.GetModPlayer<Players.AutoPicker>();
			Point16 origin = Common.AutoStacker.GetOrigin(Player.tileTargetX,Player.tileTargetY);
            if (player.altFunctionUse == 0)
			{
				
				if(
					(
						Common.AutoStacker.FindChest(origin.X,origin.Y) != -1 
						&& Main.tile[origin.X,origin.Y].type != ModContent.TileType<Tiles.AutoPicker>()
					)
					|| (AutoStacker.modMagicStorage != null && callMagicStorageFindHeart(origin))
				)
				{
					topLeft=origin;
					Main.NewText("Reciever Chest Selected to x:"+origin.X+", y:"+origin.Y + " !");
				}
				else
				{
					Main.NewText("No chest to be found.");
				}
			}
			return true;
		}
				

		private bool callMagicStorageFindHeart(Point16 origin)
		{
			if(Common.MagicStorageConnecter.FindHeart(origin) == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
				
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"RecieverChestSelector", 1);
			recipe.AddIngredient(ItemID.Wire,1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override ModItem Clone()
		{
			AutoPickerController newItem =(AutoPickerController)base.MemberwiseClone();
			newItem.topLeft = this.topLeft;
			return (ModItem)newItem;
		}
		
		public override ModItem Clone(Item item)
		{
			AutoPickerController newItem = (AutoPickerController)this.NewInstance(item);
			newItem.topLeft = this.topLeft;
			return (ModItem)newItem;
		}

	}
}
