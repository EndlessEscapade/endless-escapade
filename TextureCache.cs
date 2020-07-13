using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod
{
    internal static class TextureCache
    {
        public static Texture2D CoralLanternLampGlow;
        public static Texture2D CoralLanternTileGlow;
        public static Texture2D Chain;
        public static Texture2D Empty;
        public static Texture2D EyeTileGlow;
        public static Texture2D Stagrel_Glow;
        public static Texture2D BleckScren;
        public static Texture2D DuneShambler;
        public static Texture2D GiantSquid;
        public static Texture2D Clam;
        public static Texture2D Hydros;
        public static Texture2D Seahorse;
        public static Texture2D HydroBeam_Beam;
        public static Texture2D HydroBeam_End;
        public static Texture2D GBeam_Beam;
        public static Texture2D GBeam_End;
        public static Texture2D Gallagar;
        public static Texture2D Outline;
        public static Texture2D ShipHelth;
        public static Texture2D ShipMount;
        public static Texture2D DuneShamblerDig;
        public static Texture2D DruidsVin_Beam;
        public static Texture2D DruidsVin_End;
        public static Texture2D AkumoFeather;
        public static Texture2D Akumo;
        public static Texture2D Terraria_LogoTexture;
        public static Texture2D Terraria_Logo2Texture;
        public static Texture2D Terraria_SunTexture;
        public static Texture2D Terraria_Sun2Texture;
        public static Texture2D Terraria_Sun3Texture;
        public static Texture2D KrakenTentacles;
        public static Texture2D KrakenGlowMask;

        public static Texture2D CoralReefsSurfaceClose;

        [LoadingMethod(Loadmode = LoadingMode.Client)]
        public static void Load()
        {
            Mod mod = EEMod.instance;
            CoralLanternLampGlow = mod.GetTexture("Tiles/Furniture/CoralLanternLampGlow");
            Chain = mod.GetTexture("NPCs/CoralReefs/Chain");
            Empty = mod.GetTexture("Empty");
            EyeTileGlow = mod.GetTexture("Tiles/Furniture/EyeTileGlow");
            Stagrel_Glow = mod.GetTexture("NPCs/Bosses/Stagrel/Stagrel_Glow");
            BleckScren = mod.GetTexture("BleckScren");
            DuneShambler = mod.GetTexture("NPCs/DuneShambler");
            GiantSquid = mod.GetTexture("LoadingScreenImages/GiantSquid");
            Clam = mod.GetTexture("LoadingScreenImages/Clam");
            Hydros = mod.GetTexture("LoadingScreenImages/Hydros");
            Seahorse = mod.GetTexture("LoadingScreenImages/Seahorse");
            HydroBeam_Beam = mod.GetTexture("NPCs/Bosses/Hydros/HydroBeam_Beam");
            HydroBeam_End = mod.GetTexture("NPCs/Bosses/Hydros/HydroBeam_End");
            GBeam_Beam = mod.GetTexture("NPCs/Bosses/Gallagar/GBeam_Beam");
            GBeam_End = mod.GetTexture("NPCs/Bosses/Gallagar/GBeam_End");
            Gallagar = mod.GetTexture("NPCs/Bosses/Gallagar/Gallagar");
            Outline = mod.GetTexture("Outline");
            ShipHelth = mod.GetTexture("ShipHelth");
            ShipMount = mod.GetTexture("ShipMount");
            DuneShamblerDig = mod.GetTexture("NPCs/DuneShamblerDig");
            DruidsVin_Beam = mod.GetTexture("Projectiles/Mage/DruidsVin_Beam");
            DruidsVin_End = mod.GetTexture("Projectiles/Mage/DruidsVin_End");
            CoralLanternTileGlow = mod.GetTexture("Tiles/Furniture/CoralLanternTileGlow");
            AkumoFeather = mod.GetTexture("NPCs/Bosses/Akumo/AkumoFeather");
            Akumo = mod.GetTexture("NPCs/Bosses/Akumo/Akumo");
            KrakenTentacles = mod.GetTexture("NPCs/Bosses/Kraken/KrakenTentacles");
            Terraria_LogoTexture = ModContent.GetTexture("Terraria/Logo");
            Terraria_Logo2Texture = ModContent.GetTexture("Terraria/Logo2");
            Terraria_SunTexture = ModContent.GetTexture("Terraria/Sun");
            Terraria_Sun2Texture = ModContent.GetTexture("Terraria/Sun2");
            Terraria_Sun3Texture = ModContent.GetTexture("Terraria/Sun3");
            KrakenGlowMask = mod.GetTexture("NPCs/Bosses/Kraken/KrakenHeadGlowMask");
            CoralReefsSurfaceClose = mod.GetTexture("Backgrounds/CoralReefsSurfaceClose");
        }

        [UnloadingMethod]
        public static void Unload()
        {
            CoralLanternLampGlow = null;
            Chain = null;
            Empty = null;
            EyeTileGlow = null;
            Stagrel_Glow = null;
            BleckScren = null;
            DuneShambler = null;
            GiantSquid = null;
            Clam = null;
            Hydros = null;
            Seahorse = null;
            HydroBeam_Beam = null;
            HydroBeam_End = null;
            GBeam_Beam = null;
            GBeam_End = null;
            Gallagar = null;
            Outline = null;
            ShipHelth = null;
            ShipMount = null;
            DuneShamblerDig = null;
            DruidsVin_Beam = null;
            DruidsVin_End = null;
            CoralLanternTileGlow = null;
            AkumoFeather = null;
            Akumo = null;
            Terraria_LogoTexture = null;
            Terraria_Logo2Texture = null;
            Terraria_SunTexture = null;
            Terraria_Sun2Texture = null;
            Terraria_Sun3Texture = null;
            KrakenTentacles = null;
            CoralReefsSurfaceClose = null;
            KrakenGlowMask = null;
        }
    }
}
