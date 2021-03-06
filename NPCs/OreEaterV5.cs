using Terraria;
using Terraria.ModLoader;

namespace AutoStacker.NPCs
{
	[AutoloadHead]
	public class OreEaterV5 : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "AutoStacker/NPCs/OreEaterV5";
			}
		}
		
		public override bool Autoload(ref string name)
		{
			name = "Ore Eater Ver.5";
			return true;
		}

		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ore Eater Ver.5");
		}
		
		
		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.aiStyle=0;
			npc.friendly = true;
			npc.hide=true;
			npc.homeless = true;
			npc.width = 0;
			npc.height = 0;
			npc.defense = 10000000;
			npc.lifeMax = 10000000;
			npc.noGravity=true;
			npc.noTileCollide = true;
		}
		
		public override string[] AltTextures
		{
			get
			{
				return new string[0];
			}
		}
		
		public override string BossHeadTexture
		{
			get
			{
				return "";
			}
		}
		
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