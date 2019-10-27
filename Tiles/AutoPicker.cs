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
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16,16 };
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
					
					//topLeftPicker=new Point16((short)left,(short)top);
					topLeftPicker=autoPickerController.GetOrigin(Player.tileTargetX,Player.tileTargetY);
					topLeftRecever = autoPickerController.topLeft;
					
					picker = new Point16((short)topLeftPicker.X < topLeftRecever.X ? topLeftPicker.X +3 : topLeftPicker.X -2 , (short)topLeftPicker.Y > topLeftRecever.Y ? topLeftRecever.Y +2 : topLeftPicker.Y +2);
					direction = topLeftPicker.X < topLeftRecever.X ? 1:-1;
					Main.NewText("Auto Picker Selected to x:"+left+", y:"+top + " !");
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
				player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Witch's Pot";
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
		static Players.AutoPicker player = new Players.AutoPicker();
		public override void HitWire(int i, int j)
		{
			if(topLeftRecever.X != -1 && topLeftRecever.Y != -1)
			{
				Point16 Origin = GetOrigin(picker.X,picker.Y);
				int fieldChest = FindChest(Origin.X,Origin.Y);
				if(fieldChest == -1 || Main.chest[fieldChest].item.Where(chestItem => chestItem.stack > 0).Count() == 0)
				{
					int pickerChest = FindChest(topLeftPicker.X,topLeftPicker.Y);
					int pickPower=Main.chest[pickerChest].item.Max(chestItem => chestItem.pick);
					if(pickPower != 0 && canPick(picker.X,picker.Y,pickPower))
					{
						player.PickTile2(picker.X,picker.Y,100,this);
						//Main.NewText(picker.X +","+picker.Y);
						if(!Main.tile[picker.X,picker.Y].active())
						{
							//Wiring.TripWire(topLeftRecever.X, topLeftRecever.Y, 2, 2);
							moveNext();
						}
					}
					else
					{
						moveNext();
					}
				}
				else
				{
					Item item =Main.chest[fieldChest].item.Where(chestItem => chestItem.stack > 0).First();
					if(!deposit(item.Clone()))
					{
						Item.NewItem(picker.X * 16, picker.Y * 16, 16, 16, item.type, item.stack, noBroadcast: false, -1);
					}
					item.SetDefaults(0, true);
				}
				
			}
		}
		
		private void moveNext()
		{
			if(
				(picker.X + 4*direction < topLeftRecever.X && picker.X + 4*direction < topLeftPicker.X)
				||(picker.X + 3*direction > topLeftRecever.X && picker.X + 3*direction > topLeftPicker.X)
			)
			{
				if(picker.Y <= Main.Map.MaxHeight)
				{
					picker = new Point16((short)picker.X, (short)picker.Y +1);
					direction *= -1;
				}
			}
			else
			{
				picker = new Point16((short)picker.X +direction, (short)picker.Y);
			}
		}

		private bool canPick(int x, int y, int pickPower)
		{
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

			return true;
		}

		public bool deposit(int X, int Y, int Width, int Height, int Type, int Stack = 1, bool noBroadcast = false, int pfix = 0, bool noGrabDelay = false, bool reverseLookup = false)
		{
			Item item = new Item();
			item.SetDefaults(Type);
			item.stack=Stack;
			return deposit(item);
		}

		public bool deposit(Item item)
		{
			Point16 topLeft=topLeftRecever;
			
			//chest
			int chestNo=FindChest(topLeft.X,topLeft.Y);
			if(chestNo != -1)
			{
				//stack item
				for (int slot = 0; slot < Main.chest[chestNo].item.Length; slot++)
				{
					if (Main.chest[chestNo].item[slot].stack==0)
					{
						Main.chest[chestNo].item[slot] = item.Clone();
						item.SetDefaults(0, true);
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
			else if(AutoStacker.modMagicStorage != null)
			{
				if(Common.MagicStorageConnecter.InjectItem(topLeft, item))
				{
					item.SetDefaults(0, true);
					return true;
				}
			}
			return false;
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
	}
}