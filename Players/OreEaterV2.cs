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
	public class OreEaterV2 : ModPlayer
	{
		public bool oreEaterV2 = false;
		
		public PetV2 petV2 = new PetV2();
		
		public override void ResetEffects()
		{
			oreEaterV2 = false;
		}
	}
	
}
