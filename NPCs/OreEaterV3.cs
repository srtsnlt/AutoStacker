using Terraria;
using Terraria.ModLoader;

namespace AutoStacker.NPCs
{
	[AutoloadHead]
	public class OreEaterV3 : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "AutoStacker/NPCs/OreEaterV3";
			}
		}
		
		// public override bool Autoload(ref string name)
		// {
		// 	name = "Ore Eater Ver.3";
		// 	return true;
		// }

		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.3");
		}
		
		
		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.aiStyle=0;
			NPC.friendly = true;
			NPC.hide=true;
			NPC.homeless = true;
			NPC.width = 0;
			NPC.height = 0;
			NPC.defense = 10000000;
			NPC.lifeMax = 10000000;
			NPC.noGravity=true;
			NPC.noTileCollide = true;
		}
		
		// public override string[] AltTextures
		// {
		// 	get
		// 	{
		// 		return new string[0];
		// 	}
		// }
		
		// public override string BossHeadTexture
		// {
		// 	get
		// 	{
		// 		return "";
		// 	}
		// }
		
		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return false;
		}
		
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;
		}
		
		public override bool CheckDead()
		{
			return true;
		}
		
		public override string GetChat()
		{
			return "";
		}
		
		public override bool CheckActive()
		{
			return false;
		}

	}
}