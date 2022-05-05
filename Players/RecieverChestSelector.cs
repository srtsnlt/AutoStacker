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
		
		public override void SaveData(TagCompound tag)
		{
			tag["autoSendEnabled"] = autoSendEnabled;
			int index = Array.IndexOf(this.Player.inventory, activeItem);
			tag["activeItem"] = index;
			tag["topLeftX"] = topLeft.X;
			tag["topLeftY"] = topLeft.Y;

		}
		
		public override void LoadData(TagCompound tag)
		{
			int itemNo;
			
			if( tag.ContainsKey("autoSendEnabled") )
			{
				autoSendEnabled=tag.GetBool("autoSendEnabled");
			}
			if( tag.ContainsKey("activeItem") )
			{
				itemNo = tag.GetInt("activeItem");
				
				if( itemNo >= 0 && itemNo < this.Player.inventory.Length && this.Player.inventory[itemNo].type == ModContent.ItemType<Items.RecieverChestSelector>() )
				{
					activeItem = this.Player.inventory[itemNo];
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
			
			if(!Main.playerInventory || item.type != ModContent.ItemType<Items.RecieverChestSelector>())
			{
				notSmartCursor=false;
			}
			
			if(notSmartCursor)
			{
				Terraria.Main.SmartCursorWanted=false;
				Player.tileRangeX = Main.Map.MaxWidth;
				Player.tileRangeY = Main.Map.MaxHeight;
			}
		}
	}
}

