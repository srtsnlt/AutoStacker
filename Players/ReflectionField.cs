using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AutoStacker.Players
{
	public class ReflectionField : GlobalNPC
	{
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
			if(Main.LocalPlayer.armor.Count( item => item.type == ModContent.ItemType<Items.ReflectionField>()) >= 1)
			{
				return false;
			}else{
	            return base.CanHitPlayer(npc, target, ref cooldownSlot);
			}
        }
	}
}
