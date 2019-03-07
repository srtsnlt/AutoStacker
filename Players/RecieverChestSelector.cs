using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using System.Threading.Tasks;

namespace AutoStacker.Players
{
	class RecieverChestSelector : ModPlayer
	{
		public bool autoSendEnabled = false;
		public Item activeItem;
		public Point16 topLeft = new Point16((short)-1,(short)-1);
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("autoSendEnabled", autoSendEnabled);
			
			int index = Array.IndexOf(this.player.inventory, activeItem);
			tag.Set("activeItem", index);
			
			tag.Set("topLeftX", topLeft.X);
			tag.Set("topLeftY", topLeft.Y);
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			int itemNo;
			if( tag.HasTag("autoSendEnabled") )
			{
				autoSendEnabled=tag.GetBool("autoSendEnabled");
			}
			if( tag.HasTag("activeItem") )
			{
				itemNo = tag.GetInt("activeItem");
				
				Main.NewText(itemNo);
				Main.NewText(mod.ItemType<Items.RecieverChestSelector>());
				
				if( itemNo > 0 && itemNo < this.player.inventory.Length && this.player.inventory[itemNo].type == mod.ItemType<Items.RecieverChestSelector>() )
				{
					activeItem = this.player.inventory[itemNo];
				}
			}
			
			if(tag.HasTag("topLeftX") && tag.HasTag("topLeftY"))
			{
				topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
			}
		}
		
		
		public override void ResetEffects()
		{
			//Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
			//if (item.type == mod.ItemType("RecieverChestSelector") )
			if (Main.playerInventory)
			//if( item.type == mod.ItemType("RecieverChestSelector") || Main.playerInventory  )
			{
				Terraria.Main.SmartCursorEnabled=false;
				Player.tileRangeX = Main.Map.MaxWidth;
				Player.tileRangeY = Main.Map.MaxHeight;
			}
		}
	}
}

