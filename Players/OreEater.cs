using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using AutoStacker.Projectiles;

namespace AutoStacker.Players
{
	public class OreEater : ModPlayer
	{
		public bool oreEater = false;
		public int type = 0;
		public int index = 0;
		
		public Pet pet = new Pet();
		public bool findRoute = false;
		
		public override void ResetEffects()
		{
			//Main.npc[index].StrikeNPCNoInteraction(Main.npc[index].lifeMax, 0f, -Main.npc[index].direction, true);
			oreEater = false;
		}
	}
	
}
