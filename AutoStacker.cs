using Terraria;
using Terraria.ModLoader;

namespace AutoStacker
{
	class AutoStacker : Mod
	{
		
		internal static AutoStacker instance;
		internal static AutoSender autoSender;
		
		public AutoStacker()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
			};
		}
		
		public override void Load()
		{
			instance = this;
			autoSender = (AutoSender)GetGlobalItem("AutoSender");
		}

		public override void Unload()
		{
			instance = null;
		}
	}
}
