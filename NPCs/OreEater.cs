using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AutoStacker.NPCs
{
	[AutoloadHead]
	public class OreEater : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "AutoStacker/NPCs/OreEater";
			}
		}
		
		public override bool Autoload(ref string name)
		{
			name = "Ore Eater";
			return true;
		}

		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.friendly = true;
			npc.hide=true;
			npc.homeless = true;
			npc.width = 30;
			npc.height = 30;
			npc.aiStyle = 0;
			npc.defense = 1000000;
			npc.lifeMax = 1000000;
			animationType = 0;
		}
	}
}