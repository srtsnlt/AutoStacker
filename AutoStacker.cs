using Terraria.ModLoader;

namespace AutoStacker
{
	class AutoStacker : Mod
	{
		
		internal static AutoStacker instance;
		public static Mod modMagicStorage = null;
		// public static Mod modMagicStorageExtra = null;
		
		// public AutoStacker()
		// {
		// 	Properties = new ModProperties()
		// 	{
		// 		Autoload = true,
		// 	};
		// }
		
		public override void Load()
		{
			instance = this;
			// modMagicStorage = ModLoader.GetMod("MagicStorage");
			ModLoader.TryGetMod("MagicStorage",out modMagicStorage);
			// modMagicStorageExtra = ModLoader.GetMod("MagicStorageExtra");
		}

		public override void Unload()
		{
			instance = null;
		}
	}
}
