using EndlessEscapade.Common.Systems.Generation;
using EndlessEscapade.Content.NPCs.Shipyard;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Cinematics;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Films;

public sealed class ShipyardFilm : Film
{
    private NPC sailor;
    
    public ShipyardFilm() {
        AppendKeyFrame(FindSailor);
        AppendKeyFrame(JumpSailor);
        AppendSequence(120, EmoteSailor);
    }

    private void FindSailor(FrameEventData evt) {
        var index = NPC.FindFirstNPC(ModContent.NPCType<Sailor>());

        if (index == -1) {
            return;
        }
        
        sailor = Main.npc[index];
        sailor.immortal = true;
        sailor.dontTakeDamage = true;
        sailor.knockBackResist = 0f;
        sailor.takenDamageMultiplier = 0f;
    }

    private void JumpSailor(FrameEventData evt) {
        if (sailor == null) {
            return;
        }
        
        sailor.velocity.Y = -4f;
    }
    
    private void EmoteSailor(FrameEventData evt) {
        if (sailor == null) {
            return;
        }

        EmoteBubble.NewBubble(EmoteID.EmotionAlert, new WorldUIAnchor(sailor), evt.Duration);
    }
}
