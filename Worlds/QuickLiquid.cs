using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;


namespace AutoStacker.ModWorld
{
	public class QuickLiquid : Terraria.ModLoader.ModWorld
	{
		public static LiquidBuffer2[] liquidBuffer2 = new LiquidBuffer2[100000];
		//int count=0;
		public static bool quickSwitch = false;
		
		public QuickLiquid()
		{
			for(int i=0;i<liquidBuffer2.Length;i++)
			{
				liquidBuffer2[i]=new LiquidBuffer2();
			}
		}

		public override void PreUpdate()
		{
			/*
			count++;
			if(count >= 100)
			{
				count = 0;
				Main.NewText(Liquid.numLiquid +","+ LiquidBuffer.numLiquidBuffer + "," + LiquidBuffer2.numLiquidBuffer);
			}
			*/
			
			if(quickSwitch)
			{
				Liquid.cycles=1;
				Liquid.panicCounter=0;
				
				while(LiquidBuffer.numLiquidBuffer > 5000 && LiquidBuffer2.numLiquidBuffer != 100000 -1)
				{
					LiquidBuffer2.AddBuffer(Main.liquidBuffer[LiquidBuffer.numLiquidBuffer -1].x,Main.liquidBuffer[LiquidBuffer.numLiquidBuffer -1].y);
					LiquidBuffer.DelBuffer(LiquidBuffer.numLiquidBuffer -1);
				}
				while(LiquidBuffer.numLiquidBuffer < 5000 && LiquidBuffer2.numLiquidBuffer != 0)
				{
					//LiquidBuffer.AddBuffer(liquidBuffer2[LiquidBuffer2.numLiquidBuffer].x,liquidBuffer2[LiquidBuffer2.numLiquidBuffer].y);
					Main.liquidBuffer[LiquidBuffer.numLiquidBuffer].x = liquidBuffer2[LiquidBuffer2.numLiquidBuffer -1].x;
					Main.liquidBuffer[LiquidBuffer.numLiquidBuffer].y = liquidBuffer2[LiquidBuffer2.numLiquidBuffer -1].y;
					LiquidBuffer.numLiquidBuffer++;

					LiquidBuffer2.DelBuffer(LiquidBuffer2.numLiquidBuffer -1);
				}

				Liquid.UpdateLiquid();
			}
		}
	}


	public class LiquidBuffer2 : Terraria.LiquidBuffer
	{
		public new static int numLiquidBuffer { get; set; } = 0;


		public new static void AddBuffer(int x, int y)
		{
			//if (LiquidBuffer2.numLiquidBuffer == 1000000)
			//{
			//	return;
			//}
			//if (Main.tile[x, y].checkingLiquid())
			//{
			//	return;
			//}
			//Main.tile[x, y].checkingLiquid(true);
			QuickLiquid.liquidBuffer2[LiquidBuffer2.numLiquidBuffer].x = x;
			QuickLiquid.liquidBuffer2[LiquidBuffer2.numLiquidBuffer].y = y;
			LiquidBuffer2.numLiquidBuffer++;
		}

		public new static void DelBuffer(int l)
		{
			LiquidBuffer2.numLiquidBuffer--;
			QuickLiquid.liquidBuffer2[l].x = QuickLiquid.liquidBuffer2[LiquidBuffer2.numLiquidBuffer].x;
			QuickLiquid.liquidBuffer2[l].y = QuickLiquid.liquidBuffer2[LiquidBuffer2.numLiquidBuffer].y;
		}
	}
}
