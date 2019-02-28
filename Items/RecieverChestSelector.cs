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
		public Point16 topLeft = new Point16((short)-1,(short)-1);
		public bool active = false;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reciever Chest Selector");
			
			String tooltip_str ="Useage \n";
			tooltip_str       +="  Click chest           : Select chest\n";
			tooltip_str       +="  Right click           : Open selected chest\n";
			tooltip_str       +="  Right click this item : ON/OFF auto stack \n";
			Tooltip.SetDefault(tooltip_str);
			
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if( active )
			{
				TooltipLine lineH1 = new TooltipLine(mod, "head1", "Switch [ *** ON *** ]");
				//lineH1.overrideColor = new Color(100, 100, 255);
				tooltips.Insert(1,lineH1);
			}
			else
			{
				TooltipLine lineH1 = new TooltipLine(mod, "head1", "Switch [ ]");
				//lineH1.overrideColor = new Color(100, 100, 255);
				tooltips.Insert(1,lineH1);
			}
			
			if(topLeft.X != -1 && topLeft.Y != -1)
			{
				TooltipLine lineH2 = new TooltipLine(mod, "head2", "Chest [" + topLeft.X + "," + topLeft.Y + "]\n ");
				//lineH2.overrideColor = new Color(100, 100, 255);
				tooltips.Insert(2,lineH2);
			}
			else
			{
				TooltipLine lineH2 = new TooltipLine(mod, "head2", "Chest [ none ]\n ");
				//lineH2.overrideColor = new Color(100, 100, 255);
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
			tag.Set("active", active);
			tag.Set("topLeftX", topLeft.X);
			tag.Set("topLeftY", topLeft.Y);
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if(tag.HasTag("active"))
			{
				active = tag.GetBool("active");
			}
			if(tag.HasTag("topLeftX") && tag.HasTag("topLeftY"))
			{
				topLeft = new Point16(tag.GetShort("topLeftX"), tag.GetShort("topLeftY"));
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
				
				if(GlobalItems.RecieverChestSelector.FindChest(origin.X,origin.Y) != -1 || (AutoStacker.modMagicStorage != null && callMagicStorageFindHeart(origin)))
				{
					modPlayer.autoSendEnabled=true;
					
					active=true;
					if(modPlayer.activeItem != null && modPlayer.activeItem.modItem != null && modPlayer.activeItem.modItem != null)
					{
						if(!modPlayer.activeItem.Equals(this.item))
						{
							((RecieverChestSelector)modPlayer.activeItem.modItem).active = false;
						}
					}
					modPlayer.activeItem = this.item;
					
					topLeft=origin;
					modPlayer.topLeft = origin;
					Main.NewText("Reciever Chest Selected to x:"+origin.X+", y:"+origin.Y + " !");
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
			if(  modPlayer.autoSendEnabled  && (topLeft.X == -1 && topLeft.Y == -1) )
			{
				Main.NewText("Reciever chest is not set.Click chest before use.");
			}
			else if(  modPlayer.autoSendEnabled  &&  !(topLeft.X == -1 && topLeft.Y == -1) )
			{
				if(this.item.Equals( modPlayer.activeItem ))
				{
					modPlayer.autoSendEnabled = false;
					
					active=false;
					Main.NewText("Reciever Chest Deselected!");
				}
				else
				{
					active=true;
					if(modPlayer.activeItem != null && modPlayer.activeItem.modItem != null)
					{
						((RecieverChestSelector)modPlayer.activeItem.modItem).active = false;
					}
					modPlayer.activeItem = this.item;
					
					modPlayer.topLeft = topLeft;
					Main.NewText("Reciever Chest Selected to x:" + modPlayer.topLeft.X + ", y:" + modPlayer.topLeft.Y + " !");
					
				}
			}
			else if( !modPlayer.autoSendEnabled  && (topLeft.X == -1 && topLeft.Y == -1) )
			{
				Main.NewText("Reciever chest is not set.Click chest before use.");
			}
			else if( !modPlayer.autoSendEnabled  &&  !(topLeft.X == -1 && topLeft.Y == -1) )
			{
				modPlayer.autoSendEnabled = true;
				
				active=true;
				modPlayer.activeItem=this.item;
				
				modPlayer.topLeft = topLeft;
				Main.NewText("Reciever Chest Selected to x:" + modPlayer.topLeft.X + ", y:" + modPlayer.topLeft.Y + " !");
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
		
		/*
		~RecieverChestSelector()
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>(mod);
			modPlayer.topLeftDictionary.Remove(this.item);
			if(modPlayer.activeItem.Equals( this.item ))
			{
				modPlayer.activeItem = null;
			}
		}
		*/
		
		public override ModItem Clone()
		{
			RecieverChestSelector newItem =(RecieverChestSelector)base.MemberwiseClone();
			newItem.topLeft = this.topLeft;
			newItem.active  = this.active;
			return (ModItem)newItem;
		}
		
		public override ModItem Clone(Item item)
		{
			RecieverChestSelector newItem = (RecieverChestSelector)this.NewInstance(item);
			newItem.topLeft = this.topLeft;
			newItem.active  = this.active;
			return (ModItem)newItem;
		}

	}
}
