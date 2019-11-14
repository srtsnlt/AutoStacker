using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
			
			if( tag.ContainsKey("autoSendEnabled") )
			{
				autoSendEnabled=tag.GetBool("autoSendEnabled");
			}
			if( tag.ContainsKey("activeItem") )
			{
				itemNo = tag.GetInt("activeItem");
				
				Main.NewText(itemNo);
				Main.NewText(ModContent.ItemType<Items.RecieverChestSelector>());
				
				if( itemNo > 0 && itemNo < this.player.inventory.Length && this.player.inventory[itemNo].type == ModContent.ItemType<Items.RecieverChestSelector>() )
				{
					activeItem = this.player.inventory[itemNo];
				}
			}
			
			if(tag.ContainsKey("topLeftX") && tag.ContainsKey("topLeftY"))
			{
				topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
			}
		}
		
		public bool notSmartCursor = false;
		
		public override void ResetEffects()
		{
			
			Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
			
			if(!Main.playerInventory || item.type != mod.ItemType("RecieverChestSelector"))
			{
				notSmartCursor=false;
			}
			
			if(notSmartCursor)
			{
				Terraria.Main.SmartCursorEnabled=false;
				Player.tileRangeX = Main.Map.MaxWidth;
				Player.tileRangeY = Main.Map.MaxHeight;
			}
		}
	}
}

