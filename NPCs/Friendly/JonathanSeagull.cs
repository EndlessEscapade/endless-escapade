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
    class JonathanSeagull : EENPC
    {
        public bool IntroductionDialogue;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jonathan the Cool Seagull");
        }

        public override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.width = 36;
            NPC.height = 30;
            NPC.lifeMax = 1000000;
            NPC.dontTakeDamage = true;
        }
        public override void AI()
        {
            if (!IntroductionDialogue && Vector2.DistanceSquared(NPC.position, Main.LocalPlayer.position) < 15000)
            {
               new JonathanSeagullIntroduction().StartDialogueRequiringNPC(NPC.whoAmI);
            }
        }
    }
}