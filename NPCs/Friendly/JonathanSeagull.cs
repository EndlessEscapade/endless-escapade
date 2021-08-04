using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System;
using EEMod.Items.Placeables.Furniture;
using System.Collections.Generic;
using Terraria.Utilities;
using EEMod.Systems;

namespace EEMod.NPCs.Friendly
{    
    class JonathanSeagull : ModNPC
    {
        public bool IntroductionDialogue;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jonathan the Cool Seagull");
        }

        public override void SetDefaults()
        {
            npc.friendly = true;
            npc.width = 36;
            npc.height = 30;
            npc.lifeMax = 1000000;
            npc.dontTakeDamage = true;
        }
        public override void AI()
        {
            if (!IntroductionDialogue && Vector2.DistanceSquared(npc.position, Main.LocalPlayer.position) < 15000)
            {
               new JonathanSeagullIntroduction().StartDialogueRequiringNPC(npc.whoAmI);
            }
        }
    }
}