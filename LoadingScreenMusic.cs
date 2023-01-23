using Terraria.ModLoader;

namespace EEMod
{
    public class LoadingScreenMusic : ModMenu
    {
        public override int Music => (ModContent.GetInstance<EEMod>().leftBound != ModContent.GetInstance<EEMod>().rightBound ?
            MusicLoader.GetMusicSlot(ModContent.GetInstance<EEMod>(), "Assets/Music/goodman") : 
            base.Music);
    }
}