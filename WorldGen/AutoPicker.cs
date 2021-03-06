

// Terraria.WorldGen
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AutoStacker.WorldGen
{
	public class AutoPicker : Terraria.WorldGen
	{

      public static void KillTile2(Tiles.AutoPicker autoPicker,int i, int j, bool fail = false, bool effectOnly = false, bool noItem = false)
      {
         if (i < 0 || j < 0 || i >= Main.maxTilesX || j >= Main.maxTilesY)
         {
            return;
         }
         Tile tile = Main.tile[i, j];
         if (tile == null)
         {
            tile = new Tile();
            Main.tile[i, j] = tile;
         }
         if (!tile.active())
         {
            return;
         }
         if (j >= 1 && Main.tile[i, j - 1] == null)
         {
            Main.tile[i, j - 1] = new Tile();
         }
         if (j >= 1 && Main.tile[i, j - 1].active() && ((Main.tile[i, j - 1].type == 5 && tile.type != 5) || (Main.tile[i, j - 1].type == 323 && tile.type != 323) || (TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && !TileID.Sets.BasicChest[tile.type]) || (Main.tile[i, j - 1].type == 323 && tile.type != 323) || (TileLoader.IsDresser(Main.tile[i, j - 1].type) && !TileLoader.IsDresser(tile.type)) || (Main.tile[i, j - 1].type == 26 && tile.type != 26) || (Main.tile[i, j - 1].type == 72 && tile.type != 72)))
         {
            if (Main.tile[i, j - 1].type == 5)
            {
               if ((Main.tile[i, j - 1].frameX != 66 || Main.tile[i, j - 1].frameY < 0 || Main.tile[i, j - 1].frameY > 44) && (Main.tile[i, j - 1].frameX != 88 || Main.tile[i, j - 1].frameY < 66 || Main.tile[i, j - 1].frameY > 110) && Main.tile[i, j - 1].frameY < 198)
               {
                  return;
               }
            }
            else if (Main.tile[i, j - 1].type != 323 || Main.tile[i, j - 1].frameX == 66 || Main.tile[i, j - 1].frameX == 220)
            {
               return;
            }
         }
         if (tile.type == 10 && tile.frameY >= 594 && tile.frameY <= 646)
         {
            fail = true;
         }
         if (tile.type == 138)
         {
            fail = CheckBoulderChest(i, j);
         }
         if (tile.type == 235)
         {
            int frameX = tile.frameX;
            int num53 = i - frameX % 54 / 18;
            for (int l = 0; l < 3; l++)
            {
               if (Main.tile[num53 + l, j - 1].active() && (TileID.Sets.BasicChest[Main.tile[num53 + l, j - 1].type] || TileID.Sets.BasicChestFake[Main.tile[num53 + l, j - 1].type] || TileLoader.IsDresser(Main.tile[num53 + l, j - 1].type)))
               {
                  fail = true;
                  break;
               }
            }
         }
         TileLoader.KillTile(i, j, tile.type, ref fail, ref effectOnly, ref noItem);
         if (!effectOnly && !stopDrops)
         {
            if (!noItem && FixExploitManEaters.SpotProtected(i, j))
            {
               return;
            }
            if (TileLoader.KillSound(i, j, tile.type))
            {
               if (tile.type == 127)
               {
                  Main.PlaySound(SoundID.Item27, i * 16, j * 16);
               }
               else if (tile.type == 147 || tile.type == 224)
               {
                  if (genRand.Next(2) == 0)
                  {
                     Main.PlaySound(SoundID.Item48, i * 16, j * 16);
                  }
                  else
                  {
                     Main.PlaySound(SoundID.Item49, i * 16, j * 16);
                  }
               }
               else if (tile.type == 161 || tile.type == 163 || tile.type == 164 || tile.type == 200)
               {
                  Main.PlaySound(SoundID.Item50, i * 16, j * 16);
               }
               else if (tile.type == 3 || tile.type == 110)
               {
                  Main.PlaySound(6, i * 16, j * 16);
                  if (tile.frameX == 144)
                  {
                     if(!autoPicker.deposit(i * 16, j * 16, 16, 16, 5))
                     {
                        Item.NewItem(i * 16, j * 16, 16, 16, 5);
                     }
                  }
               }
               else if (tile.type == 254)
               {
                  Main.PlaySound(6, i * 16, j * 16);
               }
               else if (tile.type == 24)
               {
                  Main.PlaySound(6, i * 16, j * 16);
                  if (tile.frameX == 144)
                  {
                     if(!autoPicker.deposit(i * 16, j * 16, 16, 16, 60))
                     {
                        Item.NewItem(i * 16, j * 16, 16, 16, 60);
                     }
                  }
               }
               else if (Main.tileAlch[tile.type] || tile.type == 384 || tile.type == 227 || tile.type == 32 || tile.type == 51 || tile.type == 52 || tile.type == 61 || tile.type == 62 || tile.type == 69 || tile.type == 71 || tile.type == 73 || tile.type == 74 || tile.type == 113 || tile.type == 115 || tile.type == 184 || tile.type == 192 || tile.type == 205 || tile.type == 233 || tile.type == 352 || tile.type == 382)
               {
                  Main.PlaySound(6, i * 16, j * 16);
               }
               else if (tile.type == 201)
               {
                  Main.PlaySound(6, i * 16, j * 16);
                  if (tile.frameX == 270)
                  {
                     if(!autoPicker.deposit(i * 16, j * 16, 16, 16, 2887))
                     {
                        Item.NewItem(i * 16, j * 16, 16, 16, 2887);
                     }
                  }
               }
               else if (tile.type == 1 || tile.type == 6 || tile.type == 7 || tile.type == 8 || tile.type == 9 || tile.type == 22 || tile.type == 140 || tile.type == 25 || tile.type == 37 || tile.type == 38 || tile.type == 39 || tile.type == 41 || tile.type == 43 || tile.type == 44 || tile.type == 45 || tile.type == 46 || tile.type == 47 || tile.type == 48 || tile.type == 56 || tile.type == 58 || tile.type == 63 || tile.type == 64 || tile.type == 65 || tile.type == 66 || tile.type == 67 || tile.type == 68 || tile.type == 75 || tile.type == 76 || tile.type == 107 || tile.type == 108 || tile.type == 111 || tile.type == 117 || tile.type == 118 || tile.type == 119 || tile.type == 120 || tile.type == 121 || tile.type == 122 || tile.type == 150 || tile.type == 151 || tile.type == 152 || tile.type == 153 || tile.type == 154 || tile.type == 155 || tile.type == 156 || tile.type == 160 || tile.type == 161 || tile.type == 166 || tile.type == 167 || tile.type == 168 || tile.type == 169 || tile.type == 175 || tile.type == 176 || tile.type == 177 || tile.type == 203 || tile.type == 202 || tile.type == 204 || tile.type == 206 || tile.type == 211 || tile.type == 221 || tile.type == 222 || tile.type == 223 || tile.type == 226 || tile.type == 248 || tile.type == 249 || tile.type == 250 || tile.type == 272 || tile.type == 273 || tile.type == 274 || tile.type == 284 || tile.type == 325 || tile.type == 346 || tile.type == 347 || tile.type == 348 || tile.type == 350 || tile.type == 367 || tile.type == 357 || tile.type == 368 || tile.type == 369 || tile.type == 370 || tile.type == 407)
               {
                  Main.PlaySound(21, i * 16, j * 16);
               }
               else if (tile.type == 231 || tile.type == 195)
               {
                  Main.PlaySound(4, i * 16, j * 16);
               }
               else if (tile.type == 26 && tile.frameX >= 54)
               {
                  Main.PlaySound(4, i * 16, j * 16);
               }
               else if (tile.type == 314)
               {
                  Main.PlaySound(SoundID.Item52, i * 16, j * 16);
               }
               else if (tile.type >= 330 && tile.type <= 333)
               {
                  Main.PlaySound(18, i * 16, j * 16);
               }
               else if (tile.type != 138)
               {
                  Main.PlaySound(0, i * 16, j * 16);
               }
            }
            if ((tile.type == 162 || tile.type == 385 || tile.type == 129 || (tile.type == 165 && tile.frameX < 54)) && !fail)
            {
               Main.PlaySound(SoundID.Item27, i * 16, j * 16);
            }
         }
         if (tile.type == 128 || tile.type == 269)
         {
            int num86 = i;
            int m = tile.frameX;
            int m2;
            for (m2 = tile.frameX; m2 >= 100; m2 -= 100)
            {
            }
            while (m2 >= 36)
            {
               m2 -= 36;
            }
            if (m2 == 18)
            {
               m = Main.tile[i - 1, j].frameX;
               num86--;
            }
            if (m >= 100)
            {
               int num89 = 0;
               while (m >= 100)
               {
                  m -= 100;
                  num89++;
               }
               int num90 = Main.tile[num86, j].frameY / 18;
               if (num90 == 0)
               {
                  if(!autoPicker.deposit(i * 16, j * 16, 16, 16, Item.headType[num89]))
                  {
                     Item.NewItem(i * 16, j * 16, 16, 16, Item.headType[num89]);
                  }
               }
               if (num90 == 1)
               {
                  if(!autoPicker.deposit(i * 16, j * 16, 16, 16, Item.bodyType[num89]))
                  {
                     Item.NewItem(i * 16, j * 16, 16, 16, Item.bodyType[num89]);
                  }
               }
               if (num90 == 2)
               {
                  if(!autoPicker.deposit(i * 16, j * 16, 16, 16, Item.legType[num89]))
                  {
                     Item.NewItem(i * 16, j * 16, 16, 16, Item.legType[num89]);
                  }
               }
               for (m = Main.tile[num86, j].frameX; m >= 100; m -= 100)
               {
               }
               Main.tile[num86, j].frameX = (short)m;
            }
         }
         if (tile.type == 334)
         {
            int num88 = i;
            int n = tile.frameX;
            int num87 = tile.frameX;
            int num85 = 0;
            while (num87 >= 5000)
            {
               num87 -= 5000;
               num85++;
            }
            if (num85 != 0)
            {
               num87 = (num85 - 1) * 18;
            }
            num87 %= 54;
            if (num87 == 18)
            {
               n = Main.tile[i - 1, j].frameX;
               num88--;
            }
            if (num87 == 36)
            {
               n = Main.tile[i - 2, j].frameX;
               num88 -= 2;
            }
            if (n >= 5000)
            {
               int num83 = n % 5000;
               num83 -= 100;
               int num81 = Main.tile[num88 + 1, j].frameX;
               num81 = ((num81 < 25000) ? (num81 - 10000) : (num81 - 25000));
               if (Main.netMode != 1)
               {
                  Item item = new Item();
                  item.netDefaults(num83);
                  item.Prefix(num81);
                  if(!autoPicker.deposit(i * 16, j * 16, 16, 16, num83, 1, noBroadcast: true))
                  {
                     int num79 = Item.NewItem(i * 16, j * 16, 16, 16, num83, 1, noBroadcast: true);
                     item.position = Main.item[num79].position;
                     Main.item[num79] = item;
                     NetMessage.SendData(21, -1, -1, null, num79);
                  }
               }
               n = Main.tile[num88, j].frameX;
               int num78 = 0;
               while (n >= 5000)
               {
                  n -= 5000;
                  num78++;
               }
               if (num78 != 0)
               {
                  n = (num78 - 1) * 18;
               }
               Main.tile[num88, j].frameX = (short)n;
               Main.tile[num88 + 1, j].frameX = (short)(n + 18);
            }
         }
         if (tile.type == 395)
         {
            int num77 = TEItemFrame.Find(i - tile.frameX % 36 / 18, j - tile.frameY % 36 / 18);
            if (num77 != -1 && ((TEItemFrame)TileEntity.ByID[num77]).item.stack > 0)
            {
               ((TEItemFrame)TileEntity.ByID[num77]).DropItem();
               if (Main.netMode != 2)
               {
                  Main.blockMouse = true;
               }
               return;
            }
         }
         int num76 = KillTile_GetTileDustAmount(fail, tile, i, j);
         for (int num75 = 0; num75 < num76; num75++)
         {
            KillTile_MakeTileDust(i, j, tile);
         }
         if (effectOnly)
         {
            return;
         }
         if (fail)
         {
            if (tile.type == 2 || tile.type == 23 || tile.type == 109 || tile.type == 199)
            {
               tile.type = 0;
            }
            if (tile.type == 60 || tile.type == 70)
            {
               tile.type = 59;
            }
            if (Main.tileMoss[tile.type])
            {
               tile.type = 1;
            }
            SquareTileFrame(i, j);
            return;
         }
         if (TileID.Sets.BasicChest[tile.type] && Main.netMode != 1)
         {
            int num74 = tile.frameX / 18;
            int y3 = j - tile.frameY / 18;
            while (num74 > 1)
            {
               num74 -= 2;
            }
            num74 = i - num74;
            if (!Chest.DestroyChest(num74, y3))
            {
               return;
            }
         }
         if (TileLoader.IsDresser(tile.type) && Main.netMode != 1)
         {
            int num72 = tile.frameX / 18;
            int y2 = j - tile.frameY / 18;
            num72 %= 3;
            num72 = i - num72;
            if (!Chest.DestroyChest(num72, y2))
            {
               return;
            }
         }
         if (tile.type == 51 && tile.wall == 62 && genRand.Next(4) != 0)
         {
            noItem = true;
         }
         if (!noItem && !stopDrops && Main.netMode != 1)
         {
            bool flag3 = false;
            int num69 = -1;
            int num68 = -1;
            int num67 = -1;
            if (tile.type == 3)
            {
               num69 = 400;
               num68 = 100;
               if (tile.frameX >= 108)
               {
                  num69 *= 3;
                  num68 *= 3;
               }
            }
            if (tile.type == 73)
            {
               num69 = 200;
               num68 = 50;
               if (tile.frameX >= 108)
               {
                  num69 *= 3;
                  num68 *= 3;
               }
            }
            if (tile.type == 61)
            {
               num67 = 80;
               if (tile.frameX >= 108)
               {
                  num67 *= 3;
               }
            }
            if (tile.type == 74)
            {
               num67 = 40;
               if (tile.frameX >= 108)
               {
                  num67 *= 3;
               }
            }
            if (tile.type == 62)
            {
               num67 = 250;
            }
            if (tile.type == 185)
            {
               if (tile.frameY == 0 && tile.frameX < 214)
               {
                  num69 = 6;
               }
               if (tile.frameY == 18 && (tile.frameX < 214 || tile.frameX >= 1368))
               {
                  num69 = 6;
               }
            }
            else if (tile.type == 186)
            {
               if (tile.frameX >= 378 && tile.frameX <= 700)
               {
                  num69 = 6;
               }
            }
            else if (tile.type == 187)
            {
               if (tile.frameX >= 756 && tile.frameX <= 916)
               {
                  num69 = 6;
               }
               if (tile.frameX <= 322)
               {
                  num69 = 6;
               }
            }
            else if (tile.type == 233)
            {
               num67 = 10;
            }
            TileLoader.DropCritterChance(i, j, tile.type, ref num69, ref num68, ref num67);
            if (num69 > 0 && NPC.CountNPCS(357) < 5 && genRand.Next(num69) == 0)
            {
               int type12 = 357;
               if (genRand.Next(NPC.goldCritterChance) == 0)
               {
                  type12 = 448;
               }
               int num66 = NPC.NewNPC(i * 16 + 10, j * 16, type12);
               Main.npc[num66].TargetClosest();
               Main.npc[num66].velocity.Y = (float)genRand.Next(-50, -21) * 0.1f;
               Main.npc[num66].velocity.X = (float)genRand.Next(0, 26) * 0.1f * (0f - (float)Main.npc[num66].direction);
               Main.npc[num66].direction *= -1;
               Main.npc[num66].netUpdate = true;
            }
            if (num68 > 0 && NPC.CountNPCS(377) < 5 && genRand.Next(num68) == 0)
            {
               int type11 = 377;
               if (genRand.Next(NPC.goldCritterChance) == 0)
               {
                  type11 = 446;
               }
               int num65 = NPC.NewNPC(i * 16 + 10, j * 16, type11);
               Main.npc[num65].TargetClosest();
               Main.npc[num65].velocity.Y = (float)genRand.Next(-50, -21) * 0.1f;
               Main.npc[num65].velocity.X = (float)genRand.Next(0, 26) * 0.1f * (0f - (float)Main.npc[num65].direction);
               Main.npc[num65].direction *= -1;
               Main.npc[num65].netUpdate = true;
            }
            if (num67 > 0 && NPC.CountNPCS(485) + NPC.CountNPCS(486) + NPC.CountNPCS(487) < 8 && genRand.Next(num67) == 0)
            {
               int type10 = 485;
               if (genRand.Next(4) == 0)
               {
                  type10 = 486;
               }
               if (genRand.Next(12) == 0)
               {
                  type10 = 487;
               }
               int num64 = NPC.NewNPC(i * 16 + 10, j * 16, type10);
               Main.npc[num64].TargetClosest();
               Main.npc[num64].velocity.Y = (float)genRand.Next(-50, -21) * 0.1f;
               Main.npc[num64].velocity.X = (float)genRand.Next(0, 26) * 0.1f * (0f - (float)Main.npc[num64].direction);
               Main.npc[num64].direction *= -1;
               Main.npc[num64].netUpdate = true;
            }
            int num63 = 0;
            int num62 = 0;
            if (tile.type == 0 || tile.type == 2 || tile.type == 109)
            {
               num63 = 2;
            }
            else if (tile.type == 426)
            {
               num63 = 3621;
            }
            else if (tile.type == 430)
            {
               num63 = 3633;
            }
            else if (tile.type == 431)
            {
               num63 = 3634;
            }
            else if (tile.type == 432)
            {
               num63 = 3635;
            }
            else if (tile.type == 433)
            {
               num63 = 3636;
            }
            else if (tile.type == 434)
            {
               num63 = 3637;
            }
            else if (tile.type == 427)
            {
               num63 = 3622;
            }
            else if (tile.type == 435)
            {
               num63 = 3638;
            }
            else if (tile.type == 436)
            {
               num63 = 3639;
            }
            else if (tile.type == 437)
            {
               num63 = 3640;
            }
            else if (tile.type == 438)
            {
               num63 = 3641;
            }
            else if (tile.type == 439)
            {
               num63 = 3642;
            }
            else if (tile.type == 446)
            {
               num63 = 3736;
            }
            else if (tile.type == 447)
            {
               num63 = 3737;
            }
            else if (tile.type == 448)
            {
               num63 = 3738;
            }
            else if (tile.type == 449)
            {
               num63 = 3739;
            }
            else if (tile.type == 450)
            {
               num63 = 3740;
            }
            else if (tile.type == 451)
            {
               num63 = 3741;
            }
            else if (tile.type == 368)
            {
               num63 = 3086;
            }
            else if (tile.type == 369)
            {
               num63 = 3087;
            }
            else if (tile.type == 367)
            {
               num63 = 3081;
            }
            else if (tile.type == 379)
            {
               num63 = 3214;
            }
            else if (tile.type == 353)
            {
               num63 = 2996;
            }
            else if (tile.type == 365)
            {
               num63 = 3077;
            }
            else if (tile.type == 366)
            {
               num63 = 3078;
            }
            else if ((tile.type == 52 || tile.type == 62) && genRand.Next(2) == 0 && Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].cordage)
            {
               num63 = 2996;
            }
            else if (tile.type == 357)
            {
               num63 = 3066;
            }
            else if (tile.type == 1)
            {
               num63 = 3;
            }
            else if (tile.type == 3 || tile.type == 73)
            {
               if (genRand.Next(2) == 0 && (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].HasItem(281) || Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].HasItem(986)))
               {
                  num63 = 283;
               }
            }
            else if (tile.type == 227)
            {
               int num61 = tile.frameX / 34;
               num63 = 1107 + num61;
               if (num61 >= 8 && num61 <= 11)
               {
                  num63 = 3385 + num61 - 8;
               }
            }
            else if (tile.type == 4)
            {
               int num60 = tile.frameY / 22;
               switch (num60)
               {
               case 0:
                  num63 = 8;
                  break;
               case 8:
                  num63 = 523;
                  break;
               case 9:
                  num63 = 974;
                  break;
               case 10:
                  num63 = 1245;
                  break;
               case 11:
                  num63 = 1333;
                  break;
               case 12:
                  num63 = 2274;
                  break;
               case 13:
                  num63 = 3004;
                  break;
               case 14:
                  num63 = 3045;
                  break;
               case 15:
                  num63 = 3114;
                  break;
               default:
                  num63 = 426 + num60;
                  break;
               }
            }
            else if (tile.type == 239)
            {
               int num91 = tile.frameX / 18;
               if (num91 == 0)
               {
                  num63 = 20;
               }
               if (num91 == 1)
               {
                  num63 = 703;
               }
               if (num91 == 2)
               {
                  num63 = 22;
               }
               if (num91 == 3)
               {
                  num63 = 704;
               }
               if (num91 == 4)
               {
                  num63 = 21;
               }
               if (num91 == 5)
               {
                  num63 = 705;
               }
               if (num91 == 6)
               {
                  num63 = 19;
               }
               if (num91 == 7)
               {
                  num63 = 706;
               }
               if (num91 == 8)
               {
                  num63 = 57;
               }
               if (num91 == 9)
               {
                  num63 = 117;
               }
               if (num91 == 10)
               {
                  num63 = 175;
               }
               if (num91 == 11)
               {
                  num63 = 381;
               }
               if (num91 == 12)
               {
                  num63 = 1184;
               }
               if (num91 == 13)
               {
                  num63 = 382;
               }
               if (num91 == 14)
               {
                  num63 = 1191;
               }
               if (num91 == 15)
               {
                  num63 = 391;
               }
               if (num91 == 16)
               {
                  num63 = 1198;
               }
               if (num91 == 17)
               {
                  num63 = 1006;
               }
               if (num91 == 18)
               {
                  num63 = 1225;
               }
               if (num91 == 19)
               {
                  num63 = 1257;
               }
               if (num91 == 20)
               {
                  num63 = 1552;
               }
               if (num91 == 21)
               {
                  num63 = 3261;
               }
               if (num91 == 22)
               {
                  num63 = 3467;
               }
            }
            else if (tile.type == 380)
            {
               int num59 = tile.frameY / 18;
               num63 = 3215 + num59;
            }
            else if (tile.type == 442)
            {
               num63 = 3707;
            }
            else if (tile.type == 383)
            {
               num63 = 620;
            }
            else if (tile.type == 315)
            {
               num63 = 2435;
            }
            else if (tile.type == 330)
            {
               num63 = 71;
            }
            else if (tile.type == 331)
            {
               num63 = 72;
            }
            else if (tile.type == 332)
            {
               num63 = 73;
            }
            else if (tile.type == 333)
            {
               num63 = 74;
            }
            else if (tile.type == 5)
            {
               if (tile.frameX >= 22 && tile.frameY >= 198)
               {
                  if (Main.netMode != 1)
                  {
                     if (genRand.Next(2) == 0)
                     {
                        int num58;
                        for (num58 = j; Main.tile[i, num58] != null && (!Main.tile[i, num58].active() || !Main.tileSolid[Main.tile[i, num58].type] || Main.tileSolidTop[Main.tile[i, num58].type]); num58++)
                        {
                        }
                        if (Main.tile[i, num58] != null)
                        {
                           if (Main.tile[i, num58].type == 2 || Main.tile[i, num58].type == 109 || Main.tile[i, num58].type == 147 || Main.tile[i, num58].type == 199 || Main.tile[i, num58].type == 23 || TileLoader.CanDropAcorn(Main.tile[i, num58].type))
                           {
                              num63 = 9;
                              num62 = 27;
                           }
                           else
                           {
                              num63 = 9;
                           }
                        }
                     }
                     else
                     {
                        num63 = 9;
                     }
                  }
               }
               else
               {
                  num63 = 9;
               }
               if (num63 == 9)
               {
                  int num57 = i;
                  int num56 = j;
                  if (tile.frameX == 66 && tile.frameY <= 45)
                  {
                     num57++;
                  }
                  if (tile.frameX == 88 && tile.frameY >= 66 && tile.frameY <= 110)
                  {
                     num57--;
                  }
                  if (tile.frameX == 22 && tile.frameY >= 132 && tile.frameY <= 176)
                  {
                     num57--;
                  }
                  if (tile.frameX == 44 && tile.frameY >= 132 && tile.frameY <= 176)
                  {
                     num57++;
                  }
                  if (tile.frameX == 44 && tile.frameY >= 198)
                  {
                     num57++;
                  }
                  if (tile.frameX == 66 && tile.frameY >= 198)
                  {
                     num57--;
                  }
                  for (; !Main.tile[num57, num56].active() || !Main.tileSolid[Main.tile[num57, num56].type]; num56++)
                  {
                  }
                  if (Main.tile[num57, num56].active())
                  {
                     ushort type9 = Main.tile[num57, num56].type;
                     switch (type9)
                     {
                     case 70:
                        num63 = 183;
                        break;
                     case 60:
                        num63 = 620;
                        break;
                     case 23:
                        num63 = 619;
                        break;
                     case 199:
                        num63 = 911;
                        break;
                     case 147:
                        num63 = 2503;
                        break;
                     case 109:
                        num63 = 621;
                        break;
                     }
                     TileLoader.DropTreeWood(type9, ref num63);
                  }
                  int num55 = Player.FindClosest(new Vector2(num57 * 16, num56 * 16), 16, 16);
                  int axe = Main.player[num55].inventory[Main.player[num55].selectedItem].axe;
                  if (genRand.Next(100) < axe || Main.rand.Next(3) == 0)
                  {
                     flag3 = true;
                  }
               }
            }
            else if (tile.type == 323)
            {
               num63 = 2504;
               if (tile.frameX <= 132 && tile.frameX >= 88)
               {
                  num62 = 27;
               }
               int num54;
               for (num54 = j; !Main.tile[i, num54].active() || !Main.tileSolid[Main.tile[i, num54].type]; num54++)
               {
               }
               if (Main.tile[i, num54].active())
               {
                  ushort type8 = Main.tile[i, num54].type;
                  switch (type8)
                  {
                  case 234:
                     num63 = 911;
                     break;
                  case 116:
                     num63 = 621;
                     break;
                  case 112:
                     num63 = 619;
                     break;
                  }
                  TileLoader.DropPalmTreeWood(type8, ref num63);
               }
            }
            else if (tile.type == 408)
            {
               num63 = 3460;
            }
            else if (tile.type == 409)
            {
               num63 = 3461;
            }
            else if (tile.type == 415)
            {
               num63 = 3573;
            }
            else if (tile.type == 416)
            {
               num63 = 3574;
            }
            else if (tile.type == 417)
            {
               num63 = 3575;
            }
            else if (tile.type == 418)
            {
               num63 = 3576;
            }
            else if (tile.type >= 255 && tile.type <= 261)
            {
               num63 = 1970 + tile.type - 255;
            }
            else if (tile.type >= 262 && tile.type <= 268)
            {
               num63 = 1970 + tile.type - 262;
            }
            else if (tile.type == 171)
            {
               if (tile.frameX >= 10)
               {
                  dropXmasTree(i, j, 0);
                  dropXmasTree(i, j, 1);
                  dropXmasTree(i, j, 2);
                  dropXmasTree(i, j, 3);
               }
            }
            else if (tile.type == 324)
            {
               switch (tile.frameY / 22)
               {
               case 0:
                  num63 = 2625;
                  break;
               case 1:
                  num63 = 2626;
                  break;
               }
            }
            else if (tile.type == 421)
            {
               num63 = 3609;
            }
            else if (tile.type == 422)
            {
               num63 = 3610;
            }
            else if (tile.type == 419)
            {
               switch (tile.frameX / 18)
               {
               case 0:
                  num63 = 3602;
                  break;
               case 1:
                  num63 = 3618;
                  break;
               case 2:
                  num63 = 3663;
                  break;
               }
            }
            else if (tile.type == 428)
            {
               switch (tile.frameY / 18)
               {
               case 0:
                  num63 = 3630;
                  break;
               case 1:
                  num63 = 3632;
                  break;
               case 2:
                  num63 = 3631;
                  break;
               case 3:
                  num63 = 3626;
                  break;
               }
               PressurePlateHelper.DestroyPlate(new Point(i, j));
            }
            else if (tile.type == 420)
            {
               switch (tile.frameY / 18)
               {
               case 0:
                  num63 = 3603;
                  break;
               case 1:
                  num63 = 3604;
                  break;
               case 2:
                  num63 = 3605;
                  break;
               case 3:
                  num63 = 3606;
                  break;
               case 4:
                  num63 = 3607;
                  break;
               case 5:
                  num63 = 3608;
                  break;
               }
            }
            else if (tile.type == 423)
            {
               TELogicSensor.Kill(i, j);
               switch (tile.frameY / 18)
               {
               case 0:
                  num63 = 3613;
                  break;
               case 1:
                  num63 = 3614;
                  break;
               case 2:
                  num63 = 3615;
                  break;
               case 3:
                  num63 = 3726;
                  break;
               case 4:
                  num63 = 3727;
                  break;
               case 5:
                  num63 = 3728;
                  break;
               case 6:
                  num63 = 3729;
                  break;
               }
            }
            else if (tile.type == 424)
            {
               num63 = 3616;
            }
            else if (tile.type == 445)
            {
               num63 = 3725;
            }
            else if (tile.type == 429)
            {
               num63 = 3629;
            }
            else if (tile.type == 272)
            {
               num63 = 1344;
            }
            else if (tile.type == 273)
            {
               num63 = 2119;
            }
            else if (tile.type == 274)
            {
               num63 = 2120;
            }
            else if (tile.type == 460)
            {
               num63 = 3756;
            }
            else if (tile.type == 326)
            {
               num63 = 2693;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 327)
            {
               num63 = 2694;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 458)
            {
               num63 = 3754;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 459)
            {
               num63 = 3755;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 345)
            {
               num63 = 2787;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 328)
            {
               num63 = 2695;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 329)
            {
               num63 = 2697;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 346)
            {
               num63 = 2792;
            }
            else if (tile.type == 347)
            {
               num63 = 2793;
            }
            else if (tile.type == 348)
            {
               num63 = 2794;
            }
            else if (tile.type == 350)
            {
               num63 = 2860;
            }
            else if (tile.type == 336)
            {
               num63 = 2701;
            }
            else if (tile.type == 340)
            {
               num63 = 2751;
            }
            else if (tile.type == 341)
            {
               num63 = 2752;
            }
            else if (tile.type == 342)
            {
               num63 = 2753;
            }
            else if (tile.type == 343)
            {
               num63 = 2754;
            }
            else if (tile.type == 344)
            {
               num63 = 2755;
            }
            else if (tile.type == 351)
            {
               num63 = 2868;
            }
            else if (tile.type == 251)
            {
               num63 = 1725;
            }
            else if (tile.type == 252)
            {
               num63 = 1727;
            }
            else if (tile.type == 253)
            {
               num63 = 1729;
            }
            else if (tile.type == 325)
            {
               num63 = 2692;
            }
            else if (tile.type == 370)
            {
               num63 = 3100;
            }
            else if (tile.type == 396)
            {
               num63 = 3271;
            }
            else if (tile.type == 400)
            {
               num63 = 3276;
            }
            else if (tile.type == 401)
            {
               num63 = 3277;
            }
            else if (tile.type == 403)
            {
               num63 = 3339;
            }
            else if (tile.type == 397)
            {
               num63 = 3272;
            }
            else if (tile.type == 398)
            {
               num63 = 3274;
            }
            else if (tile.type == 399)
            {
               num63 = 3275;
            }
            else if (tile.type == 402)
            {
               num63 = 3338;
            }
            else if (tile.type == 404)
            {
               num63 = 3347;
            }
            else if (tile.type == 407)
            {
               num63 = 3380;
            }
            else if (tile.type == 170)
            {
               num63 = 1872;
            }
            else if (tile.type == 284)
            {
               num63 = 2173;
            }
            else if (tile.type == 214)
            {
               num63 = 85;
            }
            else if (tile.type == 213)
            {
               num63 = 965;
            }
            else if (tile.type == 211)
            {
               num63 = 947;
            }
            else if (tile.type == 6)
            {
               num63 = 11;
            }
            else if (tile.type == 7)
            {
               num63 = 12;
            }
            else if (tile.type == 8)
            {
               num63 = 13;
            }
            else if (tile.type == 9)
            {
               num63 = 14;
            }
            else if (tile.type == 202)
            {
               num63 = 824;
            }
            else if (tile.type == 234)
            {
               num63 = 1246;
            }
            else if (tile.type == 226)
            {
               num63 = 1101;
            }
            else if (tile.type == 224)
            {
               num63 = 1103;
            }
            else if (tile.type == 36)
            {
               num63 = 1869;
            }
            else if (tile.type == 311)
            {
               num63 = 2260;
            }
            else if (tile.type == 312)
            {
               num63 = 2261;
            }
            else if (tile.type == 313)
            {
               num63 = 2262;
            }
            else if (tile.type == 229)
            {
               num63 = 1125;
            }
            else if (tile.type == 230)
            {
               num63 = 1127;
            }
            else if (tile.type == 225)
            {
               if (genRand.Next(3) == 0)
               {
                  tile.honey(honey: true);
                  tile.liquid = byte.MaxValue;
               }
               else
               {
                  num63 = 1124;
                  if (Main.netMode != 1 && genRand.Next(2) == 0)
                  {
                     int num52 = 1;
                     if (genRand.Next(3) == 0)
                     {
                        num52 = 2;
                     }
                     for (int num51 = 0; num51 < num52; num51++)
                     {
                        int type7 = genRand.Next(210, 212);
                        int num50 = NPC.NewNPC(i * 16 + 8, j * 16 + 15, type7, 1);
                        Main.npc[num50].velocity.X = (float)genRand.Next(-200, 201) * 0.002f;
                        Main.npc[num50].velocity.Y = (float)genRand.Next(-200, 201) * 0.002f;
                        Main.npc[num50].netUpdate = true;
                     }
                  }
               }
            }
            else if (tile.type == 221)
            {
               num63 = 1104;
            }
            else if (tile.type == 222)
            {
               num63 = 1105;
            }
            else if (tile.type == 223)
            {
               num63 = 1106;
            }
            else if (tile.type == 248)
            {
               num63 = 1589;
            }
            else if (tile.type == 249)
            {
               num63 = 1591;
            }
            else if (tile.type == 250)
            {
               num63 = 1593;
            }
            else if (tile.type == 191)
            {
               num63 = 9;
            }
            else if (tile.type == 203)
            {
               num63 = 836;
            }
            else if (tile.type == 204)
            {
               num63 = 880;
            }
            else if (tile.type == 166)
            {
               num63 = 699;
            }
            else if (tile.type == 167)
            {
               num63 = 700;
            }
            else if (tile.type == 168)
            {
               num63 = 701;
            }
            else if (tile.type == 169)
            {
               num63 = 702;
            }
            else if (tile.type == 123)
            {
               num63 = 424;
            }
            else if (tile.type == 124)
            {
               num63 = 480;
            }
            else if (tile.type == 157)
            {
               num63 = 619;
            }
            else if (tile.type == 158)
            {
               num63 = 620;
            }
            else if (tile.type == 159)
            {
               num63 = 621;
            }
            else if (tile.type == 161)
            {
               num63 = 664;
            }
            else if (tile.type == 206)
            {
               num63 = 883;
            }
            else if (tile.type == 232)
            {
               num63 = 1150;
            }
            else if (tile.type == 198)
            {
               num63 = 775;
            }
            else if (tile.type == 314)
            {
               num63 = Minecart.GetTrackItem(tile);
            }
            else if (tile.type == 189)
            {
               num63 = 751;
            }
            else if (tile.type == 195)
            {
               num63 = 763;
            }
            else if (tile.type == 194)
            {
               num63 = 766;
            }
            else if (tile.type == 193)
            {
               num63 = 762;
            }
            else if (tile.type == 196)
            {
               num63 = 765;
            }
            else if (tile.type == 197)
            {
               num63 = 767;
            }
            else if (tile.type == 178)
            {
               switch (tile.frameX / 18)
               {
               case 0:
                  num63 = 181;
                  break;
               case 1:
                  num63 = 180;
                  break;
               case 2:
                  num63 = 177;
                  break;
               case 3:
                  num63 = 179;
                  break;
               case 4:
                  num63 = 178;
                  break;
               case 5:
                  num63 = 182;
                  break;
               case 6:
                  num63 = 999;
                  break;
               }
            }
            else if (tile.type == 149)
            {
               if (tile.frameX == 0 || tile.frameX == 54)
               {
                  num63 = 596;
               }
               else if (tile.frameX == 18 || tile.frameX == 72)
               {
                  num63 = 597;
               }
               else if (tile.frameX == 36 || tile.frameX == 90)
               {
                  num63 = 598;
               }
            }
            else if (tile.type == 13)
            {
               Main.PlaySound(13, i * 16, j * 16);
               switch (tile.frameX / 18)
               {
               case 1:
                  num63 = 28;
                  break;
               case 2:
                  num63 = 110;
                  break;
               case 3:
                  num63 = 350;
                  break;
               case 4:
                  num63 = 351;
                  break;
               case 5:
                  num63 = 2234;
                  break;
               case 6:
                  num63 = 2244;
                  break;
               case 7:
                  num63 = 2257;
                  break;
               case 8:
                  num63 = 2258;
                  break;
               default:
                  num63 = 31;
                  break;
               }
            }
            else if (tile.type == 19)
            {
               int num49 = tile.frameY / 18;
               switch (num49)
               {
               case 0:
                  num63 = 94;
                  break;
               case 1:
                  num63 = 631;
                  break;
               case 2:
                  num63 = 632;
                  break;
               case 3:
                  num63 = 633;
                  break;
               case 4:
                  num63 = 634;
                  break;
               case 5:
                  num63 = 913;
                  break;
               case 6:
                  num63 = 1384;
                  break;
               case 7:
                  num63 = 1385;
                  break;
               case 8:
                  num63 = 1386;
                  break;
               case 9:
                  num63 = 1387;
                  break;
               case 10:
                  num63 = 1388;
                  break;
               case 11:
                  num63 = 1389;
                  break;
               case 12:
                  num63 = 1418;
                  break;
               case 13:
                  num63 = 1457;
                  break;
               case 14:
                  num63 = 1702;
                  break;
               case 15:
                  num63 = 1796;
                  break;
               case 16:
                  num63 = 1818;
                  break;
               case 17:
                  num63 = 2518;
                  break;
               case 18:
                  num63 = 2549;
                  break;
               case 19:
                  num63 = 2566;
                  break;
               case 20:
                  num63 = 2581;
                  break;
               case 21:
                  num63 = 2627;
                  break;
               case 22:
                  num63 = 2628;
                  break;
               case 23:
                  num63 = 2629;
                  break;
               case 24:
                  num63 = 2630;
                  break;
               case 25:
                  num63 = 2744;
                  break;
               case 26:
                  num63 = 2822;
                  break;
               case 27:
                  num63 = 3144;
                  break;
               case 28:
                  num63 = 3146;
                  break;
               case 29:
                  num63 = 3145;
                  break;
               case 30:
               case 31:
               case 32:
               case 33:
               case 34:
               case 35:
                  num63 = 3903 + num49 - 30;
                  break;
               }
            }
            else if (tile.type == 22)
            {
               num63 = 56;
            }
            else if (tile.type == 140)
            {
               num63 = 577;
            }
            else if (tile.type == 23)
            {
               num63 = 2;
            }
            else if (tile.type == 25)
            {
               num63 = 61;
            }
            else if (tile.type == 30)
            {
               num63 = 9;
            }
            else if (tile.type == 191)
            {
               num63 = 9;
            }
            else if (tile.type == 208)
            {
               num63 = 911;
            }
            else if (tile.type == 33)
            {
               int num48 = tile.frameY / 22;
               num63 = 105;
               switch (num48)
               {
               case 1:
                  num63 = 1405;
                  break;
               case 2:
                  num63 = 1406;
                  break;
               case 3:
                  num63 = 1407;
                  break;
               case 4:
               case 5:
               case 6:
               case 7:
               case 8:
               case 9:
               case 10:
               case 11:
               case 12:
               case 13:
                  num63 = 2045 + num48 - 4;
                  break;
               default:
                  if (num48 >= 14 && num48 <= 16)
                  {
                     num63 = 2153 + num48 - 14;
                     break;
                  }
                  switch (num48)
                  {
                  case 17:
                     num63 = 2236;
                     break;
                  case 18:
                     num63 = 2523;
                     break;
                  case 19:
                     num63 = 2542;
                     break;
                  case 20:
                     num63 = 2556;
                     break;
                  case 21:
                     num63 = 2571;
                     break;
                  case 22:
                     num63 = 2648;
                     break;
                  case 23:
                     num63 = 2649;
                     break;
                  case 24:
                     num63 = 2650;
                     break;
                  case 25:
                     num63 = 2651;
                     break;
                  case 26:
                     num63 = 2818;
                     break;
                  case 27:
                     num63 = 3171;
                     break;
                  case 28:
                     num63 = 3173;
                     break;
                  case 29:
                     num63 = 3172;
                     break;
                  case 30:
                     num63 = 3890;
                     break;
                  }
                  break;
               }
            }
            else if (tile.type == 372)
            {
               num63 = 3117;
            }
            else if (tile.type == 371)
            {
               num63 = 3113;
            }
            else if (tile.type == 174)
            {
               num63 = 713;
            }
            else if (tile.type == 37)
            {
               num63 = 116;
            }
            else if (tile.type == 38)
            {
               num63 = 129;
            }
            else if (tile.type == 39)
            {
               num63 = 131;
            }
            else if (tile.type == 40)
            {
               num63 = 133;
            }
            else if (tile.type == 41)
            {
               num63 = 134;
            }
            else if (tile.type == 43)
            {
               num63 = 137;
            }
            else if (tile.type == 44)
            {
               num63 = 139;
            }
            else if (tile.type == 45)
            {
               num63 = 141;
            }
            else if (tile.type == 46)
            {
               num63 = 143;
            }
            else if (tile.type == 47)
            {
               num63 = 145;
            }
            else if (tile.type == 48)
            {
               num63 = 147;
            }
            else if (tile.type == 49)
            {
               num63 = 148;
            }
            else if (tile.type == 51)
            {
               num63 = 150;
            }
            else if (tile.type == 53)
            {
               num63 = 169;
            }
            else if (tile.type == 151)
            {
               num63 = 607;
            }
            else if (tile.type == 152)
            {
               num63 = 609;
            }
            else if (tile.type == 54)
            {
               num63 = 170;
               Main.PlaySound(13, i * 16, j * 16);
            }
            else if (tile.type == 56)
            {
               num63 = 173;
            }
            else if (tile.type == 57)
            {
               num63 = 172;
            }
            else if (tile.type == 58)
            {
               num63 = 174;
            }
            else if (tile.type == 60)
            {
               num63 = 176;
            }
            else if (tile.type == 70)
            {
               num63 = 176;
            }
            else if (tile.type == 75)
            {
               num63 = 192;
            }
            else if (tile.type == 76)
            {
               num63 = 214;
            }
            else if (tile.type == 78)
            {
               num63 = 222;
            }
            else if (tile.type == 81)
            {
               num63 = 275;
            }
            else if (tile.type == 80)
            {
               num63 = 276;
            }
            else if (tile.type == 188)
            {
               num63 = 276;
            }
            else if (tile.type == 107)
            {
               num63 = 364;
            }
            else if (tile.type == 108)
            {
               num63 = 365;
            }
            else if (tile.type == 111)
            {
               num63 = 366;
            }
            else if (tile.type == 150)
            {
               num63 = 604;
            }
            else if (tile.type == 112)
            {
               num63 = 370;
            }
            else if (tile.type == 116)
            {
               num63 = 408;
            }
            else if (tile.type == 117)
            {
               num63 = 409;
            }
            else if (tile.type == 129)
            {
               num63 = 502;
            }
            else if (tile.type == 118)
            {
               num63 = 412;
            }
            else if (tile.type == 119)
            {
               num63 = 413;
            }
            else if (tile.type == 120)
            {
               num63 = 414;
            }
            else if (tile.type == 121)
            {
               num63 = 415;
            }
            else if (tile.type == 122)
            {
               num63 = 416;
            }
            else if (tile.type == 136)
            {
               num63 = 538;
            }
            else if (tile.type == 385)
            {
               num63 = 3234;
            }
            else if (tile.type == 137)
            {
               int num92 = tile.frameY / 18;
               if (num92 == 0)
               {
                  num63 = 539;
               }
               if (num92 == 1)
               {
                  num63 = 1146;
               }
               if (num92 == 2)
               {
                  num63 = 1147;
               }
               if (num92 == 3)
               {
                  num63 = 1148;
               }
               if (num92 == 4)
               {
                  num63 = 1149;
               }
            }
            else if (tile.type == 141)
            {
               num63 = 580;
            }
            else if (tile.type == 145)
            {
               num63 = 586;
            }
            else if (tile.type == 146)
            {
               num63 = 591;
            }
            else if (tile.type == 147)
            {
               num63 = 593;
            }
            else if (tile.type == 148)
            {
               num63 = 594;
            }
            else if (tile.type == 153)
            {
               num63 = 611;
            }
            else if (tile.type == 154)
            {
               num63 = 612;
            }
            else if (tile.type == 155)
            {
               num63 = 613;
            }
            else if (tile.type == 156)
            {
               num63 = 614;
            }
            else if (tile.type == 160)
            {
               num63 = 662;
            }
            else if (tile.type == 175)
            {
               num63 = 717;
            }
            else if (tile.type == 176)
            {
               num63 = 718;
            }
            else if (tile.type == 177)
            {
               num63 = 719;
            }
            else if (tile.type == 163)
            {
               num63 = 833;
            }
            else if (tile.type == 164)
            {
               num63 = 834;
            }
            else if (tile.type == 200)
            {
               num63 = 835;
            }
            else if (tile.type == 210)
            {
               num63 = 937;
            }
            else if (tile.type == 135)
            {
               int num93 = tile.frameY / 18;
               if (num93 == 0)
               {
                  num63 = 529;
               }
               if (num93 == 1)
               {
                  num63 = 541;
               }
               if (num93 == 2)
               {
                  num63 = 542;
               }
               if (num93 == 3)
               {
                  num63 = 543;
               }
               if (num93 == 4)
               {
                  num63 = 852;
               }
               if (num93 == 5)
               {
                  num63 = 853;
               }
               if (num93 == 6)
               {
                  num63 = 1151;
               }
            }
            else if (tile.type == 144)
            {
               if (tile.frameX == 0)
               {
                  num63 = 583;
               }
               if (tile.frameX == 18)
               {
                  num63 = 584;
               }
               if (tile.frameX == 36)
               {
                  num63 = 585;
               }
            }
            else if (tile.type == 130)
            {
               num63 = 511;
            }
            else if (tile.type == 131)
            {
               num63 = 512;
            }
            else if (tile.type == 61 || tile.type == 74)
            {
               if (tile.frameX == 144 && tile.type == 61)
               {
                  if(!autoPicker.deposit(i * 16, j * 16, 16, 16, 331, genRand.Next(2, 4)))
                  {
                     Item.NewItem(i * 16, j * 16, 16, 16, 331, genRand.Next(2, 4));
                  }
                  
               }
               else if (tile.frameX == 162 && tile.type == 61)
               {
                  num63 = 223;
               }
               else if (tile.frameX >= 108 && tile.frameX <= 126 && tile.type == 61 && genRand.Next(20) == 0)
               {
                  num63 = 208;
               }
               else if (genRand.Next(100) == 0)
               {
                  num63 = 195;
               }
            }
            else if (tile.type == 59 || tile.type == 60)
            {
               num63 = 176;
            }
            else if (tile.type == 190)
            {
               num63 = 183;
            }
            else if (tile.type == 71 || tile.type == 72)
            {
               if (genRand.Next(50) == 0)
               {
                  num63 = 194;
               }
               else if (genRand.Next(2) == 0)
               {
                  num63 = 183;
               }
            }
            else if (tile.type >= 63 && tile.type <= 68)
            {
               num63 = tile.type - 63 + 177;
            }
            else if (tile.type == 50)
            {
               num63 = ((tile.frameX != 90) ? 149 : 165);
            }
            else if (Main.tileAlch[tile.type])
            {
               if (tile.type > 82)
               {
                  int num47 = tile.frameX / 18;
                  bool flag2 = false;
                  num63 = 313 + num47;
                  int type6 = 307 + num47;
                  if (tile.type == 84)
                  {
                     flag2 = true;
                  }
                  if (num47 == 0 && Main.dayTime)
                  {
                     flag2 = true;
                  }
                  if (num47 == 1 && !Main.dayTime)
                  {
                     flag2 = true;
                  }
                  if (num47 == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
                  {
                     flag2 = true;
                  }
                  if (num47 == 4 && (Main.raining || Main.cloudAlpha > 0f))
                  {
                     flag2 = true;
                  }
                  if (num47 == 5 && !Main.raining && Main.dayTime && Main.time > 40500.0)
                  {
                     flag2 = true;
                  }
                  if (num47 == 6)
                  {
                     num63 = 2358;
                     type6 = 2357;
                  }
                  int num46 = Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16);
                  if (Main.player[num46].inventory[Main.player[num46].selectedItem].type == 213)
                  {
                     if(!autoPicker.deposit(i * 16, j * 16, 16, 16, type6, genRand.Next(1, 6)))
                     {
                        Item.NewItem(i * 16, j * 16, 16, 16, type6, genRand.Next(1, 6));
                     }
                     if(!autoPicker.deposit(i * 16, j * 16, 16, 16, num63, genRand.Next(1, 3)))
                     {
                        Item.NewItem(i * 16, j * 16, 16, 16, num63, genRand.Next(1, 3));
                     }
                     num63 = -1;
                  }
                  else if (flag2)
                  {
                     int stack = genRand.Next(1, 4);
                     if(!autoPicker.deposit(i * 16, j * 16, 16, 16, type6, stack))
                     {
                        Item.NewItem(i * 16, j * 16, 16, 16, type6, stack);
                     }
                  }
               }
            }
            else if (tile.type == 321)
            {
               num63 = 2503;
            }
            else if (tile.type == 322)
            {
               num63 = 2504;
            }
            bool num94 = TileLoader.Drop(i, j, tile.type);
            if (num94 && num63 > 0)
            {
               int num45 = 1;
               if (flag3)
               {
                  num45++;
               }
               if(!autoPicker.deposit(i * 16, j * 16, 16, 16, num63, num45, noBroadcast: false, -1))
               {
                  Item.NewItem(i * 16, j * 16, 16, 16, num63, num45, noBroadcast: false, -1);
               }
            }
            if (num94 && num62 > 0)
            {
               if(!autoPicker.deposit(i * 16, j * 16, 16, 16, num62, 1, noBroadcast: false, -1))
               {
                  Item.NewItem(i * 16, j * 16, 16, 16, num62, 1, noBroadcast: false, -1);
               }
            }
         }
         if (Main.netMode != 2)
         {
            AchievementsHelper.NotifyTileDestroyed(Main.player[Main.myPlayer], tile.type);
         }
         tile.active(active: false);
         tile.halfBrick(halfBrick: false);
         tile.frameX = -1;
         tile.frameY = -1;
         tile.color(0);
         tile.frameNumber(0);
         if (tile.type == 58 && j > Main.maxTilesY - 200)
         {
            tile.lava(lava: true);
            tile.liquid = 128;
         }
         else if (tile.type == 419)
         {
            Wiring.PokeLogicGate(i, j + 1);
         }
         else if (tile.type == 54)
         {
            SquareWallFrame(i, j);
         }
         tile.type = 0;
         tile.inActive(inActive: false);
         SquareTileFrame(i, j);
      }
      private static bool stopDrops = false;
   }
}