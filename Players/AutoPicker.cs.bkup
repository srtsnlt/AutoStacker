using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoStacker.Players
{
	public class AutoPicker : Terraria.Player
	{
		public void PickTile2(int x, int y, int pickPower, Tiles.AutoPicker autoPicker)
		{
			int num8 = 0;
			int tileId = hitTile.HitObject(x, y, 1);
			Tile tile = Main.tile[x, y];
			if (Main.tileNoFail[tile.type])
			{
				num8 = 100;
			}
			if (Main.tileDungeon[tile.type] || tile.type == 25 || tile.type == 58 || tile.type == 117 || tile.type == 203)
			{
				num8 += pickPower / 2;
			}
			else if (tile.type == 48 || tile.type == 232)
			{
				num8 += pickPower / 4;
			}
			else if (tile.type == 226)
			{
				num8 += pickPower / 4;
			}
			else if (tile.type == 107 || tile.type == 221)
			{
				num8 += pickPower / 2;
			}
			else if (tile.type == 108 || tile.type == 222)
			{
				num8 += pickPower / 3;
			}
			else if (tile.type == 111 || tile.type == 223)
			{
				num8 += pickPower / 4;
			}
			else if (tile.type == 211)
			{
				num8 += pickPower / 5;
			}
			else
			{
				TileLoader.MineDamage(pickPower, ref num8);
			}
			if (tile.type == 211 && pickPower < 200)
			{
				num8 = 0;
			}
			if ((tile.type == 25 || tile.type == 203) && pickPower < 65)
			{
				num8 = 0;
			}
			else if (tile.type == 117 && pickPower < 65)
			{
				num8 = 0;
			}
			else if (tile.type == 37 && pickPower < 50)
			{
				num8 = 0;
			}
			else if (tile.type == 404 && pickPower < 65)
			{
				num8 = 0;
			}
			else if ((tile.type == 22 || tile.type == 204) && (double)y > Main.worldSurface && pickPower < 55)
			{
				num8 = 0;
			}
			else if (tile.type == 56 && pickPower < 65)
			{
				num8 = 0;
			}
			else if (tile.type == 58 && pickPower < 65)
			{
				num8 = 0;
			}
			else if ((tile.type == 226 || tile.type == 237) && pickPower < 210)
			{
				num8 = 0;
			}
			else if (Main.tileDungeon[tile.type] && pickPower < 65)
			{
				if ((double)x < (double)Main.maxTilesX * 0.35 || (double)x > (double)Main.maxTilesX * 0.65)
				{
					num8 = 0;
				}
			}
			else if (tile.type == 107 && pickPower < 100)
			{
				num8 = 0;
			}
			else if (tile.type == 108 && pickPower < 110)
			{
				num8 = 0;
			}
			else if (tile.type == 111 && pickPower < 150)
			{
				num8 = 0;
			}
			else if (tile.type == 221 && pickPower < 100)
			{
				num8 = 0;
			}
			else if (tile.type == 222 && pickPower < 110)
			{
				num8 = 0;
			}
			else if (tile.type == 223 && pickPower < 150)
			{
				num8 = 0;
			}
			else
			{
				TileLoader.PickPowerCheck(tile, pickPower, ref num8);
			}
			if (tile.type == 147 || tile.type == 0 || tile.type == 40 || tile.type == 53 || tile.type == 57 || tile.type == 59 || tile.type == 123 || tile.type == 224 || tile.type == 397)
			{
				num8 += pickPower;
			}
			if (tile.type == 165 || Main.tileRope[tile.type] || tile.type == 199 || Main.tileMoss[tile.type])
			{
				num8 = 100;
			}
			if (hitTile.AddDamage(tileId, num8, updateAmount: false) >= 100 && (tile.type == 2 || tile.type == 23 || tile.type == 60 || tile.type == 70 || tile.type == 109 || tile.type == 199 || Main.tileMoss[tile.type]))
			{
				num8 = 0;
			}
			if (tile.type == 128 || tile.type == 269)
			{
				if (tile.frameX == 18 || tile.frameX == 54)
				{
					x--;
					tile = Main.tile[x, y];
					hitTile.UpdatePosition(tileId, x, y);
				}
				if (tile.frameX >= 100)
				{
					num8 = 0;
					Main.blockMouse = true;
				}
			}
			if (tile.type == 334)
			{
				if (tile.frameY == 0)
				{
					y++;
					tile = Main.tile[x, y];
					hitTile.UpdatePosition(tileId, x, y);
				}
				if (tile.frameY == 36)
				{
					y--;
					tile = Main.tile[x, y];
					hitTile.UpdatePosition(tileId, x, y);
				}
				int j = tile.frameX;
				bool flag3 = j >= 5000;
				bool flag2 = false;
				if (!flag3)
				{
					int num7 = j / 18;
					num7 %= 3;
					x -= num7;
					tile = Main.tile[x, y];
					if (tile.frameX >= 5000)
					{
						flag3 = true;
					}
				}
				if (flag3)
				{
					j = tile.frameX;
					int num5 = 0;
					while (j >= 5000)
					{
						j -= 5000;
						num5++;
					}
					if (num5 != 0)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					num8 = 0;
					Main.blockMouse = true;
				}
			}
			if (!Terraria.WorldGen.CanKillTile(x, y))
			{
				num8 = 0;
			}
			if (hitTile.AddDamage(tileId, num8) >= 100)
			{
				AchievementsHelper.CurrentlyMining = true;
				hitTile.Clear(tileId);
				if (Main.netMode == 1 && Main.tileContainer[Main.tile[x, y].type])
				{
					WorldGen.AutoPicker.KillTile2(autoPicker, x, y, fail: true);
					NetMessage.SendData(17, -1, -1, null, 0, x, y, 1f);
					if (Main.tile[x, y].type == 21 || (Main.tile[x, y].type >= 470 && TileID.Sets.BasicChest[Main.tile[x, y].type]))
					{
						NetMessage.SendData(34, -1, -1, null, 1, x, y);
					}
					if (Main.tile[x, y].type == 467)
					{
						NetMessage.SendData(34, -1, -1, null, 5, x, y);
					}
					if (TileLoader.IsDresser(Main.tile[x, y].type))
					{
						NetMessage.SendData(34, -1, -1, null, 3, x, y);
					}
					if (Main.tile[x, y].type >= 470 && TileID.Sets.BasicChest[Main.tile[x, y].type])
					{
						NetMessage.SendData(34, -1, -1, null, 101, x, y, 0f, 0, Main.tile[x, y].type);
					}
					if (Main.tile[x, y].type >= 470 && TileLoader.IsDresser(Main.tile[x, y].type))
					{
						NetMessage.SendData(34, -1, -1, null, 103, x, y, 0f, 0, Main.tile[x, y].type);
					}
				}
				else
				{
					int num4 = y;
					bool num9 = Main.tile[x, num4].active();
					WorldGen.AutoPicker.KillTile2(autoPicker, x, num4);
					if (num9 && !Main.tile[x, num4].active())
					{
						AchievementsHelper.HandleMining();
					}
					if (Main.netMode == 1)
					{
						NetMessage.SendData(17, -1, -1, null, 0, x, num4);
					}
				}
				AchievementsHelper.CurrentlyMining = false;
			}
			else
			{
				WorldGen.AutoPicker.KillTile2(autoPicker, x, y, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, x, y, 1f);
				}
			}
			if (num8 != 0)
			{
				hitTile.Prune();
			}
		}

	}	
}
