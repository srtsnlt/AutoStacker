using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using AutoStacker.Projectiles;

namespace AutoStacker.Players
{
	public class OreEater : ModPlayer
	{
		public bool oreEaterEnable = false;
		public int type = 0;
		public int index = 0;
		public NPC npc;
		
		public PetBase pet;
		public bool findRoute = false;
		
		public override void ResetEffects()
		{
			oreEaterEnable = false;
		}
		
		public override TagCompound Save()
		{
			
			foreach(var npc in Main.npc.Where( npc => npc.type == this.type ))
			{
				npc.active = false;
			}
			
			return null;
		}
		
	}
	
}
