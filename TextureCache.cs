using EEMod.Autoloading;
using EEMod.Extensions;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;

#pragma warning disable CS0649 // not assigning fields a value
namespace EEMod
{
    internal static class TextureCache
    {
        //public static Texture2D CoralLanternLampGlow;
        //public static Texture2D CoralLanternTileGlow;
        public static Texture2D Chain;

        public static Texture2D Empty;
        public static Texture2D EyeTileGlow;

        //public static Texture2D Stagrel_Glow;
        public static Texture2D BleckScren;

        public static Texture2D DuneShambler;
        public static Texture2D GiantSquid;
        public static Texture2D Clam;
        public static Texture2D Hydros;
        public static Texture2D Seahorse;
        public static Texture2D HydroBeam_Beam;
        public static Texture2D HydroBeam_End;
        public static Texture2D Vine;

        //public static Texture2D GBeam_Beam;
        //public static Texture2D GBeam_End;
        //public static Texture2D Gallagar;
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
        public static Texture2D PufferBig;
        public static Texture2D PufferSmall;
        public static Texture2D CoralReefsSurfaceClose;
        public static Texture2D NotBleckScren;
        public static Texture2D Tentacle;
        public static Texture2D TentacleChain;
        public static Texture2D TentacleChainSmol;
        public static Texture2D TentacleEnd;
        public static Texture2D Oil;
        public static Texture2D VArrow;
        public static Texture2D CB1;
        public static Texture2D GradientEffect;
        public static Texture2D Mask1;
        public static Texture2D LightVine;
        public static Texture2D AHT;
        public static Texture2D BluePuck;
        public static Texture2D RedPuck;
        public static Texture2D Puck;
        public static Texture2D Noise;
        public static Texture2D BlackTex;
        public static Texture2D LensFlare;
        public static Texture2D LensFlare2;
        public static Texture2D SunRing;
        public static Texture2D Leaf;
        public static Texture2D Bob1;
        public static Texture2D Bob2;
        public static Texture2D OceanScreen;
        public static Texture2D Seagulls;

        [TextureInit("Projectiles/Summons/AkumoMinionGlow")]
        public static Texture2D AkumoMinionGlow;

        [TextureInit("NPCs/CoralReefs/GlisteningReefs/BlueringOctopusGlow")]
        public static Texture2D BlueringOctopusGlow;

        [TextureInit("NPCs/CoralReefs/BombFishGlow")]
        public static Texture2D BombFishGlow;

        [TextureInit("NPCs/CoralReefs/ClamGlow")]
        public static Texture2D ClamGlow;

        [TextureInit("NPCs/Bosses/CoralGolem/CoralGolemGlow")]
        public static Texture2D CoralGolemGlow;

        [TextureInit("Star")]
        public static Texture2D Star;

        [TextureInit("Backgrounds/CoralReefsSurfaceFar")]
        public static Texture2D CoralReefsSurfaceFar;

        [TextureInit("Backgrounds/CoralReefsSurfaceMid")]
        public static Texture2D CoralReefsSurfaceMid;

        [TextureInit("InspectIcon")]
        public static Texture2D InspectIcon;

        [TextureInit("Tiles/Furniture/Coral/GlowCoral1Glow")]
        public static Texture2D GlowCoral1Glow;

        [TextureInit("Tiles/Furniture/Coral/GlowCoral2Glow")]
        public static Texture2D GlowCoral2Glow;

        [TextureInit("Tiles/Furniture/Coral/GlowCoral3Glow")]
        public static Texture2D GlowCoral3Glow;

        [TextureInit("Tiles/Furniture/Coral/GlowHangCoral1Glow")]
        public static Texture2D GlowHangCoral1Glow;

        [TextureInit("Tiles/Furniture/Coral/GlowHangCoral2Glow")]
        public static Texture2D GlowHangCoral2Glow;

        [TextureInit("NPCs/CoralReefs/GrebyserGlow")]
        public static Texture2D GrebyserGlow;

        [TextureInit("Tiles/Furniture/Coral/GroundGlowCoralGlow")]
        public static Texture2D GroundGlowCoralGlow;

        [TextureInit("Tiles/Furniture/Coral/GroundGlowCoralGlow2")]
        public static Texture2D GroundGlowCoralGlow2;

        [TextureInit("Tiles/Furniture/Coral/GroundGlowCoralGlow3")]
        public static Texture2D GroundGlowCoralGlow3;

        [TextureInit("Masks/Extra_49")]
        public static Texture2D Extra_49;

        [TextureInit("Projectiles/Enemy/MechanicalLureChain")]
        public static Texture2D MechanicalLureChain;

        [TextureInit("NPCs/CoralReefs/MechanicalReefs/MechanicalEelGlow")]
        public static Texture2D MechanicalEelGlow;

        [TextureInit("Projectiles/Enemy/MechanicalMissileGlow")]
        public static Texture2D MechanicalMissileGlow;

        [TextureInit("NPCs/CoralReefs/MechanicalReefs/MechanicalSharkGlow")]
        public static Texture2D MechanicalSharkGlow;

        [TextureInit("NPCs/funny/FleshChain")]
        public static Texture2D FleshChain;

        [TextureInit("Projectiles/SailorsClaspChain")]
        public static Texture2D SailorsClaspChain;

        [TextureInit("Tiles/Furniture/ThermalVentGlow")]
        public static Texture2D ThermalVentGlow;

        [TextureInit("Tiles/Furniture/Coral/WideBulbousCoralGlow")]
        public static Texture2D WideBulbousCoralGlow;

        [TextureInit("Items/Zipline")]
        public static Texture2D Zipline;

        [LoadingMethod(LoadMode.Client)]
        public static void Load()
        {
            Mod mod = EEMod.instance;

            Seagulls = mod.GetTexture("Seagulls");
            OceanScreen = mod.GetTexture("OceanScreen");
            Bob1 = mod.GetTexture("Bob1");
            Bob2 = mod.GetTexture("Bob2");
            Leaf = mod.GetTexture("Leaf");
            Noise = mod.GetTexture("noise");
            LensFlare = mod.GetTexture("ShaderAssets/LensFlare");
            LensFlare2 = mod.GetTexture("ShaderAssets/LensFlare2");
            SunRing = mod.GetTexture("ShaderAssets/SunRing");
            LightVine = mod.GetTexture("Projectiles/Light");
            Vine = mod.GetTexture("Projectiles/Vine");
            BluePuck = mod.GetTexture("BlueAirHockeyThing");
            RedPuck = mod.GetTexture("RedAirHockeyThing");
            Puck = mod.GetTexture("AirHockeyPuck");
            BlackTex = mod.GetTexture("NoiseSurfacingTest");
            //CoralLanternLampGlow = mod.GetTexture("Tiles/Furniture/Coral/CoralLanternLampGlow");
            Chain = mod.GetTexture("NPCs/CoralReefs/MechanicalReefs/DreadmineChain");
            Empty = mod.GetTexture("Empty");
            Mask1 = mod.GetTexture("Masks/Extra_49");
            //EyeTileGlow = mod.GetTexture("Tiles/Furniture/Coral/EyeTileGlow");
            //Stagrel_Glow = mod.GetTexture("NPCs/Bosses/Stagrel/Stagrel_Glow");
            BleckScren = mod.GetTexture("BleckScreen");
            NotBleckScren = mod.GetTexture("NotBleckScren");
            DuneShambler = mod.GetTexture("NPCs/DuneShambler");
            GiantSquid = mod.GetTexture("LoadingScreenImages/GiantSquid");
            Clam = mod.GetTexture("LoadingScreenImages/Clam");
            Hydros = mod.GetTexture("LoadingScreenImages/Hydros");
            Seahorse = mod.GetTexture("LoadingScreenImages/Seahorse");
            HydroBeam_Beam = mod.GetTexture("NPCs/Bosses/Hydros/HydroBeam_Beam");
            HydroBeam_End = mod.GetTexture("NPCs/Bosses/Hydros/HydroBeam_End");
            AHT = mod.GetTexture("AirHockeyTable");
            //GBeam_Beam = mod.GetTexture("NPCs/Bosses/Gallagar/GBeam_Beam");
            //GBeam_End = mod.GetTexture("NPCs/Bosses/Gallagar/GBeam_End");
            //Gallagar = mod.GetTexture("NPCs/Bosses/Gallagar/Gallagar");
            Outline = mod.GetTexture("Outline");
            ShipHelth = mod.GetTexture("ShipHelthSheet");
            ShipMount = mod.GetTexture("ShipMount");
            DuneShamblerDig = mod.GetTexture("NPCs/DuneShamblerDig");
            DruidsVin_Beam = mod.GetTexture("Projectiles/Mage/DruidsVin_Beam");
            DruidsVin_End = mod.GetTexture("Projectiles/Mage/DruidsVin_End");
            //CoralLanternTileGlow = mod.GetTexture("Tiles/Furniture/Coral/CoralLanternTileGlow");
            AkumoFeather = mod.GetTexture("NPCs/Bosses/Akumo/AkumoFeather");
            Akumo = mod.GetTexture("NPCs/Bosses/Akumo/Akumo");
            KrakenTentacles = mod.GetTexture("NPCs/Bosses/Kraken/KrakenTentacles");
            Tentacle = mod.GetTexture("NPCs/Bosses/Kraken/Tentacle");
            PufferBig = mod.GetTexture("NPCs/CoralReefs/ToxicPuffer");
            PufferSmall = mod.GetTexture("NPCs/CoralReefs/ToxicPufferSmall");
            Terraria_LogoTexture = ModContent.GetTexture("Terraria/Logo");
            Terraria_Logo2Texture = ModContent.GetTexture("Terraria/Logo2");
            Terraria_SunTexture = ModContent.GetTexture("Terraria/Sun");
            Terraria_Sun2Texture = ModContent.GetTexture("Terraria/Sun2");
            Terraria_Sun3Texture = ModContent.GetTexture("Terraria/Sun3");
            KrakenGlowMask = mod.GetTexture("NPCs/Bosses/Kraken/KrakenHeadGlowMask");
            CoralReefsSurfaceClose = mod.GetTexture("Backgrounds/CoralReefsSurfaceClose");
            TentacleChain = mod.GetTexture("NPCs/Bosses/Kraken/TentacleChain");
            TentacleChainSmol = mod.GetTexture("NPCs/Bosses/Kraken/ChainSmol");
            TentacleEnd = mod.GetTexture("NPCs/Bosses/Kraken/EndOfSmol");
            Oil = mod.GetTexture("NPCs/Bosses/Kraken/Oil");
            VArrow = mod.GetTexture("Projectiles/VolleyballArrow");
            CB1 = mod.GetTexture("Backgrounds/CoralReefsSurfaceClose");
            GradientEffect = mod.GetTexture("Masks/GradientEffect");
            ReflInit(mod.GetTexture);
        }

        [UnloadingMethod]
        public static void Unload()
        {
            Seagulls = null;
            OceanScreen = null;
            Bob1 = null;
            Bob2 = null;
            LensFlare2 = null;
            Leaf = null;
            SunRing = null;
            LensFlare = null;
            BlackTex = null;
            Noise = null;
            Puck = null;
            BluePuck = null;
            RedPuck = null;
            AHT = null;
            LightVine = null;
            Vine = null;
            Mask1 = null;
            GradientEffect = null;
            CB1 = null;
            VArrow = null;
            Oil = null;
            TentacleEnd = null;
            TentacleChainSmol = null;
            TentacleChain = null;
            Tentacle = null;
            //CoralLanternLampGlow = null;
            Chain = null;
            Empty = null;
            EyeTileGlow = null;
            //Stagrel_Glow = null;
            BleckScren = null;
            DuneShambler = null;
            GiantSquid = null;
            Clam = null;
            Hydros = null;
            Seahorse = null;
            HydroBeam_Beam = null;
            HydroBeam_End = null;
            /*GBeam_Beam = null;
            GBeam_End = null;
            Gallagar = null;*/
            Outline = null;
            ShipHelth = null;
            ShipMount = null;
            DuneShamblerDig = null;
            DruidsVin_Beam = null;
            DruidsVin_End = null;
            //CoralLanternTileGlow = null;
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
            NotBleckScren = null;
        }

        private static void ReflInit(Func<string, Texture2D> textureGetter)
        {
            foreach (var field in typeof(TextureCache).GetFields(Helpers.FlagsStatic))
            {
                if (field.TryGetCustomAttribute(out TextureInitAttribute attribute))
                {
                    field.SetValue(null, textureGetter(attribute.TexturePath));
                }
            }
        }

        /*[UnloadingMethod]
        public static void Unload() // they're claimed by the unloader
        {
            TentacleChain = null;
            Tentacle = null;
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
            NotBleckScren = null;
        }*/

        [AttributeUsage(AttributeTargets.Field)]
        private class TextureInitAttribute : Attribute
        {
            public string TexturePath { get; private set; }

            /// <param name="texturePath">Folders/TextureName</param>
            public TextureInitAttribute(string texturePath) => TexturePath = texturePath;
        }
    }
}