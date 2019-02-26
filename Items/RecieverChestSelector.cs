using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.ObjectData;

namespace AutoStacker.Items
{
	public class RecieverChestSelector : ModItem
	{
		
		public Point16 topLeft;
		public bool autoSendEnabled;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reciever Chest Selector\n");
			
			String tooltip_str = "Useage :\n";
			tooltip_str       +="Click chest : Select chest\n";
			tooltip_str       +="Right click : Open selected chest\n";
			tooltip_str       +="Right click this item : Deselect/Select Recieve chest\n";
			Tooltip.SetDefault(tooltip_str);
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
			
			topLeft = new Point16((short)0, (short)0);
			autoSendEnabled = false;
		}
		
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Set("topLeftX", topLeft.X);
			tag.Set("topLeftY", topLeft.Y);
			tag.Set("autoSendEnabled", autoSendEnabled);
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if( tag.HasTag("topLeftX") && tag.HasTag("topLeftY") && tag.HasTag("autoSendEnabled") )
			{
				topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
				autoSendEnabled = tag.GetBool("autoSendEnabled");
			}
		}
		
		// UseItem
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool UseItem(Player player)
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>(mod);
			
			
			if (player.altFunctionUse == 0)
			{
				Point16 origin = GetOrigin(Player.tileTargetX,Player.tileTargetY);
				
				if(GlobalItems.RecieverChestSelector.FindChest(origin.X,origin.Y) != -1)
				{
					topLeft = origin;
					modPlayer.topLeft = origin;
					modPlayer.autoSendEnabled=true;
					Main.NewText("Reciever Chest Selected to x:"+origin.X+", y:"+origin.Y + " !");
					
				}
				else if(AutoStacker.modMagicStorage != null )
				{
					if(callMagicStorageFindHeart(origin))
					{
						topLeft = origin;
						modPlayer.topLeft = origin;
						modPlayer.autoSendEnabled=true;
						Main.NewText("Reciever Storage Heart Selected to x:"+origin.X+", y:"+origin.Y + " !");
						
					}else{
						Main.NewText("No chest to be found.");
					}
				}
				else
				{
					Main.NewText("No chest to be found.");
				}
			}
			else
			{
				int chestNo=GlobalItems.RecieverChestSelector.FindChest(topLeft.X, topLeft.Y);
				if(chestNo != -1)
				{
					player.chest = chestNo;
					Main.playerInventory = true;
					Main.recBigList = false;
					player.chestX = topLeft.X;
					player.chestY = topLeft.Y;
					
					Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
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
		
		public Point16 GetOrigin(int x, int y)
		{
			
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active())
				return new Point16(x, y);
			
			TileObjectData tileObjectData = TileObjectData.GetTileData(tile.type, 0);
			if (tileObjectData == null)
				return new Point16(x, y);
			
			//OneByOne
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			if (tileObjectData.Width == 1 && tileObjectData.Height == 1)
				return new Point16(x, y);
			
			//xOffset
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			int xOffset = tile.frameX % tileObjectData.CoordinateFullWidth / tileObjectData.CoordinateWidth ;
			
			//yOffset
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//Rectangle(single)
			int yOffset;
			if (tileObjectData.CoordinateHeights.Distinct().Count() == 1)
			{
				yOffset = tile.frameY % tileObjectData.CoordinateFullHeight / tileObjectData.CoordinateHeights[0] ;
			}
			
			//Rectangle(complex)
			else
			{
				yOffset = 0;
				int FullY = tile.frameY % tileObjectData.CoordinateFullHeight;
				for (int i = 0; i < tileObjectData.CoordinateHeights.Length && FullY >= tileObjectData.CoordinateHeights[i]; i++)
				{
					FullY -= tileObjectData.CoordinateHeights[i];
					yOffset++;
				}
			}
			return new Point16(x - xOffset, y - yOffset);
			
		}
		
		
		// RightClick
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>(mod);
			if(modPlayer.autoSendEnabled)
			{
				modPlayer.autoSendEnabled = false;
				Main.NewText("Reciever Chest Deselected!");
			}
			else
			{
				if(topLeft.X != 0 && topLeft.Y != 0)
				{
					modPlayer.autoSendEnabled = true;
					modPlayer.topLeft = topLeft;
					
					Main.NewText("Reciever Chest Selected to x:"+modPlayer.topLeft.X+", y:"+modPlayer.topLeft.Y + " !");
				}
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
