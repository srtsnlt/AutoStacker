using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.GameContent.NetModules;
using Terraria.Net;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace AutoStacker.ModWorld
{
	public class QuickLiquidV2 : Terraria.ModLoader.ModWorld
	{
		public static Liquid2[] liquid2;
		int count=0;
		public static bool quickSwitch = false;
		
		public QuickLiquidV2()
		{
			liquid2 = new Liquid2[1000000];
			for(int i=0;i<liquid2.Length;i++)
			{
				liquid2[i]=new Liquid2();
			}
			Liquid2.ReInit();
		}

		public override void Load(TagCompound tag)
		{
			Liquid2.ReInit();
		}
		public override void PreUpdate()
		{
			
			if(quickSwitch)
			{
				count++;
				if(count >= 100)
				{
					count = 0;
					//Main.NewText(Liquid.numLiquid +","+ LiquidBuffer.numLiquidBuffer +","+ Liquid2.numLiquid);
				}

				Liquid.cycles=1;
				Liquid.panicCounter=0;
				Liquid.UpdateLiquid();

				while(LiquidBuffer.numLiquidBuffer > 0 && Liquid2.numLiquid != 1000000 -1)
				{
					//Liquid2.AddWater(Main.liquidBuffer[LiquidBuffer.numLiquidBuffer -1].x,Main.liquidBuffer[LiquidBuffer.numLiquidBuffer -1].y);
					//LiquidBuffer.DelBuffer(LiquidBuffer.numLiquidBuffer -1);
					Main.tile[Main.liquidBuffer[0].x, Main.liquidBuffer[0].y].checkingLiquid(false);
					Liquid2.AddWater(Main.liquidBuffer[0].x,Main.liquidBuffer[0].y);
					LiquidBuffer.DelBuffer(0);
				}
				Liquid2.cycles=1;
				Liquid2.panicCounter=0;
				Liquid2.UpdateLiquid();
			}
		}
	}


	public class Liquid2 : Terraria.Liquid
	{
		public new static int skipCount { get; set; } = 0;
		public new static int stuckCount { get; set; } = 0;
		public new static int stuckAmount { get; set; } = 0;
		public new static int cycles { get; set; } = 1;
		public new static int resLiquid { get; set; } = 1000000;
		public new static int maxLiquid { get; set; } = 1000000;
		public new static int numLiquid { get; set; } = 0;
		public new static bool stuck { get; set; } = false;
		public new static bool quickFall { get; set; } = false;
		public new static bool quickSettle { get; set; } = false;
		private static int wetCounter { get; set; } = 0;
		public new static int panicCounter { get; set; } = 0;
		public new static bool panicMode { get; set; } = false;
		public new static int panicY { get; set; } = 0;
		private static HashSet<int> _netChangeSet { get; set; } = new HashSet<int>();
		private static HashSet<int> _swapNetChangeSet { get; set; } = new HashSet<int>();

		public new static void AddWater(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (Main.tile[x, y] == null)
			{
				return;
			}
			if (tile.checkingLiquid())
			{
				return;
			}
			if (x >= Main.maxTilesX - 5 || y >= Main.maxTilesY - 5)
			{
				return;
			}
			if (x < 5 || y < 5)
			{
				return;
			}
			if (tile.liquid == 0)
			{
				return;
			}
			if (Liquid2.numLiquid >= Liquid2.maxLiquid - 1)
			{
				LiquidBuffer.AddBuffer(x, y);
				return;
			}
			tile.checkingLiquid(true);
			QuickLiquidV2.liquid2[Liquid2.numLiquid].kill = 0;
			QuickLiquidV2.liquid2[Liquid2.numLiquid].x = x;
			QuickLiquidV2.liquid2[Liquid2.numLiquid].y = y;
			QuickLiquidV2.liquid2[Liquid2.numLiquid].delay = 0;
			tile.skipLiquid(false);
			Liquid2.numLiquid++;
			if (Main.netMode == 2)
			{
				Liquid2.NetSendLiquid(x, y);
			}
			if (tile.active() && !Terraria.WorldGen.gen)
			{
				bool flag = false;
				if (tile.lava())
				{
					if (TileObjectData.CheckLavaDeath(tile))
					{
						flag = true;
					}
				}
				else if (TileObjectData.CheckWaterDeath(tile))
				{
					flag = true;
				}
				if (flag)
				{
					Terraria.WorldGen.KillTile(x, y, false, false, false);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)y, 0f, 0, 0, 0);
					}
				}
			}
		}

		public new static void DelWater(int l)
		{
			int num = QuickLiquidV2.liquid2[l].x;
			int num2 = QuickLiquidV2.liquid2[l].y;
			Tile tile = Main.tile[num - 1, num2];
			Tile tile2 = Main.tile[num + 1, num2];
			Tile tile3 = Main.tile[num, num2 + 1];
			Tile tile4 = Main.tile[num, num2];
			byte b = 2;
			if (tile4.liquid < b)
			{
				tile4.liquid = 0;
				if (tile.liquid < b)
				{
					tile.liquid = 0;
				}
				else
				{
					Liquid2.AddWater(num - 1, num2);
				}
				if (tile2.liquid < b)
				{
					tile2.liquid = 0;
				}
				else
				{
					Liquid2.AddWater(num + 1, num2);
				}
			}
			else if (tile4.liquid < 20)
			{
				if ((tile.liquid < tile4.liquid && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type])) || (tile2.liquid < tile4.liquid && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type])) || (tile3.liquid < 255 && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type])))
				{
					tile4.liquid = 0;
				}
			}
			else if (tile3.liquid < 255 && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type]) && !Liquid.stuck)
			{
				QuickLiquidV2.liquid2[l].kill = 0;
				return;
			}
			if (tile4.liquid < 250 && Main.tile[num, num2 - 1].liquid > 0)
			{
				Liquid2.AddWater(num, num2 - 1);
			}
			if (tile4.liquid == 0)
			{
				tile4.liquidType(0);
			}
			else
			{
				if ((tile2.liquid > 0 && Main.tile[num + 1, num2 + 1].liquid < 250 && !Main.tile[num + 1, num2 + 1].active()) || (tile.liquid > 0 && Main.tile[num - 1, num2 + 1].liquid < 250 && !Main.tile[num - 1, num2 + 1].active()))
				{
					Liquid2.AddWater(num - 1, num2);
					Liquid2.AddWater(num + 1, num2);
				}
				if (tile4.lava())
				{
					Liquid2.LavaCheck(num, num2);
					for (int i = num - 1; i <= num + 1; i++)
					{
						for (int j = num2 - 1; j <= num2 + 1; j++)
						{
							Tile tile5 = Main.tile[i, j];
							if (tile5.active())
							{
								if (tile5.type == 2 || tile5.type == 23 || tile5.type == 109 || tile5.type == 199)
								{
									tile5.type = 0;
									Terraria.WorldGen.SquareTileFrame(i, j, true);
									if (Main.netMode == 2)
									{
										NetMessage.SendTileSquare(-1, num, num2, 3, TileChangeType.None);
									}
								}
								else if (tile5.type == 60 || tile5.type == 70)
								{
									tile5.type = 59;
									Terraria.WorldGen.SquareTileFrame(i, j, true);
									if (Main.netMode == 2)
									{
										NetMessage.SendTileSquare(-1, num, num2, 3, TileChangeType.None);
									}
								}
							}
						}
					}
				}
				else if (tile4.honey())
				{
					Liquid2.HoneyCheck(num, num2);
				}
			}
			if (Main.netMode == 2)
			{
				Liquid2.NetSendLiquid(num, num2);
			}
			Liquid2.numLiquid--;
			Main.tile[QuickLiquidV2.liquid2[l].x, QuickLiquidV2.liquid2[l].y].checkingLiquid(false);
			QuickLiquidV2.liquid2[l].x = QuickLiquidV2.liquid2[Liquid2.numLiquid].x;
			QuickLiquidV2.liquid2[l].y = QuickLiquidV2.liquid2[Liquid2.numLiquid].y;
			QuickLiquidV2.liquid2[l].kill = QuickLiquidV2.liquid2[Liquid2.numLiquid].kill;
			if (Main.tileAlch[(int)tile4.type])
			{
				Terraria.WorldGen.CheckAlch(num, num2);
			}
		}

		public new static void ReInit()
		{
			Liquid2.skipCount = 0;
			Liquid2.stuckCount = 0;
			Liquid2.stuckAmount = 0;
			Liquid2.cycles = 10;
			Liquid2.resLiquid = 1000000;
			Liquid2.maxLiquid = 1000000;
			Liquid2.numLiquid = 0;
			Liquid2.stuck = false;
			Liquid2.quickFall = false;
			Liquid2.quickSettle = false;
			Liquid2.wetCounter = 0;
			Liquid2.panicCounter = 0;
			Liquid2.panicMode = false;
			Liquid2.panicY = 0;
		}


		public new static void UpdateLiquid()
		{
			int netMode = Main.netMode;
			if (Liquid2.quickSettle || Liquid2.numLiquid > 2000)
			{
				Liquid2.quickFall = true;
			}
			else
			{
				Liquid2.quickFall = false;
			}
			Liquid2.wetCounter++;
			int num7 = Liquid2.maxLiquid / Liquid2.cycles;
			int num2 = num7 * (Liquid2.wetCounter - 1);
			int num3 = num7 * Liquid2.wetCounter;
			if (Liquid2.wetCounter == Liquid2.cycles)
			{
				num3 = Liquid2.numLiquid;
			}
			if (num3 > Liquid2.numLiquid)
			{
				num3 = Liquid2.numLiquid;
				int netMode2 = Main.netMode;
				Liquid2.wetCounter = Liquid2.cycles;
			}
			if (Liquid2.quickFall)
			{
				for (int l = num2; l < num3; l++)
				{
					QuickLiquidV2.liquid2[l].delay = 10;
					QuickLiquidV2.liquid2[l].Update();
					Main.tile[QuickLiquidV2.liquid2[l].x, QuickLiquidV2.liquid2[l].y].skipLiquid(false);
				}
			}
			else
			{
				for (int m = num2; m < num3; m++)
				{
					if (!Main.tile[QuickLiquidV2.liquid2[m].x, QuickLiquidV2.liquid2[m].y].skipLiquid())
					{
						QuickLiquidV2.liquid2[m].Update();
					}
					else
					{
						Main.tile[QuickLiquidV2.liquid2[m].x, QuickLiquidV2.liquid2[m].y].skipLiquid(false);
					}
				}
			}
			if (Liquid2.wetCounter >= Liquid2.cycles)
			{
				Liquid2.wetCounter = 0;
				for (int n = Liquid2.numLiquid - 1; n >= 0; n--)
				{
					if (QuickLiquidV2.liquid2[n].kill > 4)
					{
						Liquid2.DelWater(n);
					}
				}
				int num4 = Liquid2.maxLiquid - (Liquid2.maxLiquid - Liquid2.numLiquid);
				if (num4 > LiquidBuffer.numLiquidBuffer)
				{
					num4 = LiquidBuffer.numLiquidBuffer;
				}
				for (int num5 = 0; num5 < num4; num5++)
				{
					Main.tile[Main.liquidBuffer[0].x, Main.liquidBuffer[0].y].checkingLiquid(false);
					Liquid2.AddWater(Main.liquidBuffer[0].x, Main.liquidBuffer[0].y);
					LiquidBuffer.DelBuffer(0);
				}
				if (Liquid2.numLiquid > 0 && Liquid2.numLiquid > Liquid2.stuckAmount - 50 && Liquid2.numLiquid < Liquid2.stuckAmount + 50)
				{
					Liquid2.stuckCount++;
					if (Liquid2.stuckCount >= 10000)
					{
						Liquid2.stuck = true;
						for (int num6 = Liquid2.numLiquid - 1; num6 >= 0; num6--)
						{
							Liquid2.DelWater(num6);
						}
						Liquid2.stuck = false;
						Liquid2.stuckCount = 0;
					}
				}
				else
				{
					Liquid2.stuckCount = 0;
					Liquid2.stuckAmount = Liquid2.numLiquid;
				}
			}
			/*
			if (!Terraria.WorldGen.gen && Main.netMode == 2 && Liquid2._netChangeSet.Count > 0)
			{
				Utils.Swap<HashSet<int>>(ref Liquid2._netChangeSet, ref Liquid2._swapNetChangeSet);
				NetManager.Instance.Broadcast(NetLiquid2Module.Serialize(Liquid2._swapNetChangeSet), -1);
				Liquid2._swapNetChangeSet.Clear();
			}
			*/
		}
	}
}
