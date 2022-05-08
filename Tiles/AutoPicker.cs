using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AutoStacker.Tiles
{
	public class AutoPicker : ModTile
	{
		
		
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileValue[Type] = 500;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide , TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Auto Picker");
			AddMapEntry(new Color(200, 200, 200), name, MapChestName);
			dustType = mod.DustType("Sparkle");
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
			chest = "Auto Picker";
			chestDrop = mod.ItemType("AutoPicker");
		}
		
		
		
		public override bool HasSmartInteract()
		{
			return true;
		}

		public string MapChestName(string name, int i, int j)
		{
			int left = i;
			int top = j;
			Tile tile = Main.tile[i, j];
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			int chest = Chest.FindChest(left, top);
			if (Main.chest[chest].name == "")
			{
				return name;
			}
			else
			{
				return name + ": " + Main.chest[chest].name;
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
			Chest.DestroyChest(i, j);
		}

		[Obsolete]
		public override void RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int left = i;
			int top = j;
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			Item item = player.inventory[player.selectedItem];
			if (item.type == mod.ItemType("AutoPickerController"))
			{
				Items.AutoPickerController autoPickerController = (Items.AutoPickerController)item.modItem;
				if(autoPickerController.topLeft.X != -1 && autoPickerController.topLeft.Y != -1 )
				{
					player.tileInteractionHappened = true;
					
					topLeftPicker=Common.AutoStacker.GetOrigin(Player.tileTargetX,Player.tileTargetY);
					topLeftRecever = autoPickerController.topLeft;
					
					picker = new Point16((short)topLeftPicker.X < topLeftRecever.X ? topLeftPicker.X +3 : topLeftPicker.X -2 , (short)topLeftPicker.Y > topLeftRecever.Y ? topLeftRecever.Y +2 : topLeftPicker.Y +2);
					direction = topLeftPicker.X < topLeftRecever.X ? 1:-1;
					Main.NewText("Auto Picker Selected to x:"+picker.X+", y:"+picker.Y + " !");
				}
				else
				{
					Main.NewText("Select Reciever Chest before Select Auto Picker!");
				}
			}
			else
			{
				if (player.sign >= 0)
				{
					Main.PlaySound(SoundID.MenuClose);
					player.sign = -1;
					Main.editSign = false;
					Main.npcChatText = "";
				}
				if (Main.editChest)
				{
					Main.PlaySound(SoundID.MenuTick);
					Main.editChest = false;
					Main.npcChatText = "";
				}
				if (player.editedChestName)
				{
					NetMessage.SendData(33, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
					player.editedChestName = false;
				}
				if (Main.netMode == 1)
				{
					if (left == player.chestX && top == player.chestY && player.chest >= 0)
					{
						player.chest = -1;
						Recipe.FindRecipes();
						Main.PlaySound(SoundID.MenuClose);
					}
					else
					{
						NetMessage.SendData(31, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
						Main.stackSplit = 600;
					}
				}
				else
				{
					int chest = Chest.FindChest(left, top);
					if (chest >= 0)
					{
						Main.stackSplit = 600;
						if (chest == player.chest)
						{
							player.chest = -1;
							Main.PlaySound(SoundID.MenuClose);
						}
						else
						{
							player.chest = chest;
							Main.playerInventory = true;
							Main.recBigList = false;
							player.chestX = left;
							player.chestY = top;
							Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
						}
						Recipe.FindRecipes();
					}
				}
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			int chest = Chest.FindChest(left, top);
			player.showItemIcon2 = -1;
			if (chest < 0)
			{
				player.showItemIconText = Language.GetTextValue("LegacyChestType.0");
			}
			else
			{
				player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Auto Picker";
				if (player.showItemIconText == "Auto Picker")
				{
					player.showItemIcon2 = mod.ItemType("AutoPicker");
					player.showItemIconText = "";
				}
			}
			player.noThrow = 2;
			player.showItemIcon = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
		
		public Point16 topLeftRecever = new Point16((short)-1,(short)-1);
		public Point16 topLeftPicker = new Point16((short)-1,(short)-1);
		public Point16 picker = new Point16((short)-1,(short)-1);
		public int direction = 1;

		private int renda=0;
		public override void HitWire(int i, int j)
		{
			if((short)picker.X == (short)-1 || (short)picker.Y == (short)-1)
			{
				return;
			}
			int pickerChest = Common.AutoStacker.FindChest(topLeftPicker.X,topLeftPicker.Y);
			if(pickerChest == -1)
			{
				return;
			}
			int pickPower=Main.chest[pickerChest].item.Max(chestItem => chestItem.pick);
			if(pickPower <= 0)
			{
				return;
			}

			if(topLeftRecever.X != -1 && topLeftRecever.Y != -1)
			{
				Point16 Origin = Common.AutoStacker.GetOrigin(picker.X,picker.Y);
				int fieldChest = Common.AutoStacker.FindChest(Origin.X,Origin.Y);
				if(fieldChest == -1 || Main.chest[fieldChest].item.Where(chestItem => chestItem.stack > 0).Count() == 0)
				{
					if(canPick(picker.X,picker.Y,pickPower))
					{
						Tile tile = Main.tile[picker.X,picker.Y];
						WorldGen.KillTile(picker.X,picker.Y,false,false,false);
						Main.item.Where(item => item.active && item.spawnTime == 0 && item.stack > 0).ToList().ForEach(item => tryDeposit(item));
						moveNext(pickPower);

					}
					else
					{
						moveNext(pickPower);
					}
				}
				else
				{
					Item item =Main.chest[fieldChest].item.Where(chestItem => chestItem.stack > 0).First();
					tryDeposit(item);
				}
				
			}
		}

		private void moveNext(int pickPower)
		{
			short x = picker.X;
			short y = picker.Y;
			
			for(;;)
			{
				if(
					(x + 4*direction < topLeftRecever.X && x + 4*direction < topLeftPicker.X)
					||(x + 3*direction > topLeftRecever.X && x + 3*direction > topLeftPicker.X)
				)
				{
					y += 1;
					direction *= -1;

					if(y >= Main.maxTilesY )
					{
						y -= 1;
						x += (short)direction;
						break;						
					}
				}
				else
				{
					x += (short)direction;
				}

				Point16 Origin = Common.AutoStacker.GetOrigin(x,y);
				int fieldChest = Common.AutoStacker.FindChest(Origin.X,Origin.Y);

				if(fieldChest == -1 || Main.chest[fieldChest].item.Where(chestItem => chestItem.stack > 0).Count() == 0)
				{
					if(canPick(x,y,pickPower))
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			picker = new Point16(x, y);
		}

		private bool canPick(int x, int y, int pickPower)
		{
			if(pickPower == 0 || !Main.tile[x,y].active())
			{
				return false;
			}

			Tile tile = Main.tile[x,y];
			if ((tile.type == 211 && pickPower <= 200)
				|| ((tile.type == 25 || tile.type == 203) && pickPower <= 65)
				|| (tile.type == 117 && pickPower <= 65)
				|| (tile.type == 37 && pickPower <= 50)
				|| (tile.type == 404 && pickPower <= 65)
//				|| ((tile.type == 22 || tile.type == 204) && (double)AY[index] > Main.worldSurface && pickPower < 55)
				|| (tile.type == 56 && pickPower <= 65)
				|| (tile.type == 58 && pickPower <= 65)
				|| ((tile.type == 226 || tile.type == 237) && pickPower <= 210)
				|| (Main.tileDungeon[tile.type] && pickPower <= 65)
//				|| ((double)AX[index] < (double)Main.maxTilesX * 0.35 || (double)AX[index] > (double)Main.maxTilesX * 0.65)
				|| (tile.type == 107 && pickPower <= 100)
				|| (tile.type == 108 && pickPower <= 110)
				|| (tile.type == 111 && pickPower <= 150)
				|| (tile.type == 221 && pickPower <= 100)
				|| (tile.type == 222 && pickPower <= 110)
				|| (tile.type == 223 && pickPower <= 150)
			)
			{
				return false;
			}

			int check=1;
			TileLoader.PickPowerCheck(tile, pickPower, ref check);
			if(check == 0)
			{
				return false;
			}

			Tile tileUpper = Main.tile[x, y -1];
			if(
				tileUpper.type == TileID.Containers
				||tileUpper.type == TileID.DemonAltar
			)
			{
				return false;
			}

			return WorldGen.CanKillTile(x,y);
		}

		private void tryDeposit(Item item)
		{
			if(!deposit(item))
			{
				Item.NewItem(picker.X * 16, picker.Y * 16, 16, 16, item.type, item.stack, noBroadcast: false, -1);
				// int num =
				// Main.item[num].TryCombiningIntoNearbyItems(num);
			}
			item.SetDefaults(0, true);
			item.active=false;
			item.stack=0;
		}

		public bool deposit(Item item)
		{
			Point16 topLeft=topLeftRecever;
			
			//chest
			int chestNo=Common.AutoStacker.FindChest(topLeft.X,topLeft.Y);
			if(chestNo != -1)
			{
				//stack item
				for (int slot = 0; slot < Main.chest[chestNo].item.Length; slot++)
				{
					if (Main.chest[chestNo].item[slot].stack==0)
					{
						Main.chest[chestNo].item[slot] = item.Clone();
						item.SetDefaults(0, true);
						item.active=false;
						item.stack=0;
						Wiring.TripWire(topLeft.X, topLeft.Y, 2, 2);
						return true;
					}
					
					Item chestItem = Main.chest[chestNo].item[slot];
					if (item.IsTheSameAs(chestItem) && chestItem.stack < chestItem.maxStack)
					{
						int spaceLeft = chestItem.maxStack - chestItem.stack;
						if (spaceLeft >= item.stack)
						{
							chestItem.stack += item.stack;
							item.SetDefaults(0, true);
							item.active=false;
							item.stack=0;
							Wiring.TripWire(topLeft.X, topLeft.Y, 2, 2);
							return true;
						}
						else
						{
							item.stack -= spaceLeft;
							chestItem.stack = chestItem.maxStack;
						}
					}
				}
			}
			//storage heart
			else if(AutoStacker.modMagicStorage != null || AutoStacker.modMagicStorageExtra != null)
			{
				if(Common.MagicStorageConnecter.InjectItem(topLeft, item))
				{
					item.SetDefaults(0, true);
					item.active=false;
					item.stack=0;
					return true;
				}
			}
			return false;
		}
	}
}