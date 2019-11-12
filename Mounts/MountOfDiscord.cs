using System;
using Terraria;
using Terraria.ModLoader;

namespace AutoStacker.Mounts
{
	public class MountOfDiscord : ModMountData
	{
		public const float speed = 10f;
		
		public override void SetDefaults()
		{
			mountData.spawnDust = 226;
			mountData.spawnDustNoGravity = true;
			mountData.buff = ModContent.BuffType<Buffs.MountOfDiscord>();
			mountData.heightBoost = 0;
			mountData.flightTimeMax = Int32.MaxValue;
			mountData.fatigueMax = Int32.MaxValue;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 0;
			mountData.dashSpeed = 0;
			mountData.acceleration = 0;
			mountData.swimSpeed = 0;
			mountData.jumpHeight = 0;
			mountData.jumpSpeed = 0;
			//mountData.blockExtraJumps = true;
			mountData.totalFrames = 1;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 0;
			}
			mountData.playerYOffsets = new int[] { 0 };
			mountData.xOffset = 0;
			mountData.bodyFrame = 5;
			mountData.yOffset = 0;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 0;
			mountData.standingFrameDelay = 0;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 0;
			mountData.runningFrameDelay = 0;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 0;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
	}
}