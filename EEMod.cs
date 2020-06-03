using Terraria.ModLoader;

namespace EEMod
{
	public class EEMod : Mod
	{
		public static EEMod instance;

		public override void Load()
        {
            instance = this;
        }

		public override void Unload()
        {
            instance = null;  
        }
	}
}