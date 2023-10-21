using EndlessEscapade.Common.Systems.Generation;
using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Content.Tiles.Shipyard;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.Cinematics;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Films;

public sealed class ShipyardFilm : Film
{
    private int currentTrigger;
    
    private NPC sailor;
    
    private Vector2 originalCenter;
    
    public ShipyardFilm() {
        AppendKeyFrame(FindSailor);
        AppendKeyFrame(CacheSailor);
        AppendSequence(60, JumpSailor);
        AppendKeyFrame(LockCamera);        
        AppendSequence(480, MoveSailor);
        AppendKeyFrame(ResetSailor);
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
    
    private void CacheSailor(FrameEventData evt) {
        if (sailor == null) {
            return;
        }

        originalCenter = sailor.Center;
    }
    
    private void JumpSailor(FrameEventData evt) {
        if (sailor == null ||!evt.IsFirstFrame) {
            return;
        }
        
        sailor.velocity.Y = -4f;
    }
        
    private void LockCamera(FrameEventData evt) {
        if (sailor == null) {
            return;
        }

        var offset = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
        var position = new Vector2(ShipyardSystem.X, ShipyardSystem.Y) * 16f - offset;

    }
        
    private void MoveSailor(FrameEventData evt) {
        if (sailor == null) {
            return;
        }

        sailor.noGravity = true;
    }
    
    private void ResetSailor(FrameEventData evt) {
        if (sailor == null) {
            return;
        }

        sailor.Center = originalCenter;
        sailor.SetDefaults(ModContent.NPCType<Sailor>());
    }
}
