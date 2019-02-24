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
	public class RecieverChestSelector : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reciever Chest Selector");
			Tooltip.SetDefault("Useage");
			Tooltip.SetDefault("Click chest : To select chest");
			Tooltip.SetDefault("Right click : To open selected chest");
			Tooltip.SetDefault("Right click this item : Deselect/Select Recieve chest");
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
			tag.Set("topLeftX", AutoSender.topLeft.X);
			tag.Set("topLeftY", AutoSender.topLeft.Y);
			tag.Set("autoSendEnabled", AutoSender.autoSendEnabled);
			
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			AutoSender.topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
			AutoSender.autoSendEnabled = tag.GetBool("autoSendEnabled");
		}
		

		// UseItem
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 0)
			{
				Point16 origin = GetOrigin(Player.tileTargetX,Player.tileTargetY);
				
				if(FindChest(origin.X,origin.Y) != -1)
				{
					AutoSender.topLeft = origin;
					AutoSender.autoSendEnabled=true;
					Main.NewText("Reciever Chest Selected to x:"+origin.X+", y:"+origin.Y + " !");
				}
				else if(FindHeart(origin) != null)
				{
					AutoSender.topLeft = origin;
					AutoSender.autoSendEnabled=true;
					Main.NewText("Reciever Storage Heart Selected to x:"+origin.X+", y:"+origin.Y + " !");
				}
				else
				{
					Main.NewText("No chest to be found.");
				}
				
			}
			else
			{
				int chestNo=FindChest(AutoSender.topLeft.X, AutoSender.topLeft.Y);
				if(chestNo != -1)
				{
					player.chest = chestNo;
					Main.playerInventory = true;
					Main.recBigList = false;
					player.chestX = AutoSender.topLeft.X;
					player.chestY = AutoSender.topLeft.Y;
					
					Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
				}
			}
			return true;
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
		
		public static int FindChest(int originX, int originY)
		{
			Tile tile = Main.tile[originX, originY];
			if (tile == null || !tile.active())
				return -1;

			if (!Chest.isLocked(originX, originY))
				return Chest.FindChest(originX, originY);
			else
				return -1;
		}
		
		//Magic Storage
		private TEStorageHeart FindHeart(Point16 origin)
		{
			if( !TileEntity.ByPosition.ContainsKey(origin) )
				return null;
			
			TEStorageCenter tEStorageCenter = (TEStorageCenter)TileEntity.ByPosition[origin];
			if(tEStorageCenter == null)
				return null;
			
			TEStorageHeart heart = tEStorageCenter.GetHeart();
			return heart;
		}
		
		// RightClick
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			if(AutoSender.autoSendEnabled){
				AutoSender.autoSendEnabled = false;
				Main.NewText("Reciever Chest Deselected!");
			}else{
				if(AutoSender.topLeft.X != 0 && AutoSender.topLeft.Y != 0)
				{
					AutoSender.autoSendEnabled = true;
					Main.NewText("Reciever Chest Selected to x:"+AutoSender.topLeft.X+", y:"+AutoSender.topLeft.Y + " !");
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
