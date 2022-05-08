using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AutoStacker.Projectiles
{
	public class ReflectionField : GlobalProjectile
	{
        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
			if(Main.LocalPlayer.armor.Count( item => item.type == ModContent.ItemType<Items.ReflectionField>()) >= 1)
			{
				return false;
			}else{
	            return base.CanHitPlayer(projectile, target);
			}
        }
	}
}
