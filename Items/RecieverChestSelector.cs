using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
				tooltips.Insert(1,lineH1);
			}
			else
			{
				TooltipLine lineH1 = new TooltipLine(mod, "head1", "Switch [ ]");
				tooltips.Insert(1,lineH1);
			}
			
			if(topLeft.X != -1 && topLeft.Y != -1)
			{
				TooltipLine lineH2 = new TooltipLine(mod, "head2", "Chest [" + topLeft.X + "," + topLeft.Y + "]\n ");
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
			tag.Set("active", active);
			tag.Set("topLeftX", topLeft.X);
			tag.Set("topLeftY", topLeft.Y);
			return tag;
		}
		
		public override void Load(TagCompound tag)
		{
			if(tag.ContainsKey("active"))
			{
				active = tag.GetBool("active");
			}
			if(tag.ContainsKey("topLeftX") && tag.ContainsKey("topLeftY"))
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
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>();
			if (player.altFunctionUse == 0)
			{
				Point16 origin = Common.AutoStacker.GetOrigin(Player.tileTargetX,Player.tileTargetY);
				
				if(Common.AutoStacker.FindChest(origin.X,origin.Y) != -1 || (AutoStacker.modMagicStorage != null && callMagicStorageFindHeart(origin)))
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
				int chestNo=Common.AutoStacker.FindChest(topLeft.X, topLeft.Y);
				if(chestNo != -1)
				{
					player.chest = chestNo;
					Main.playerInventory = true;
					Main.recBigList = false;
					player.chestX = topLeft.X;
					player.chestY = topLeft.Y;
					
					modPlayer.notSmartCursor=true;
					
					Terraria.Main.SmartCursorEnabled=false;
					Player.tileRangeX = Main.Map.MaxWidth;
					Player.tileRangeY = Main.Map.MaxHeight;

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
		
		// RightClick
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public override bool CanRightClick()
		{
			return true;
		}
		
		public override void RightClick(Player player)
		{
			Players.RecieverChestSelector modPlayer = (Players.RecieverChestSelector)Main.LocalPlayer.GetModPlayer<Players.RecieverChestSelector>();
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
