using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Players
{
	public class MinionHouse : ModPlayer
	{
		public override void OnEnterWorld(Player player)
		{
			Worlds.MinionHouse minionHouse = mod.GetModWorld<Worlds.MinionHouse>();
			minionHouse.init();
		}
	}
}
