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
		
		public Pet pet = new Pet();
		
		public override void ResetEffects()
		{
			oreEater = false;
		}
	}
	
}
