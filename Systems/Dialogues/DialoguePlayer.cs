using EEMod.UI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class DialoguePlayer : EEPlayer
    {
        public bool HasTalkedToSailor;

        //TODO: TagCompound this data later
        public override void SetControls()
        {
            base.SetControls();
            if (EEMod.UI.IsActive("DialogueInterface") && DialogueUI.CurrentDialogueSystem.LockPlayerMovement)
            {
                Player.controlDown = false;
                Player.controlHook = false;
                Player.controlJump = false;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlUp = false;
                Player.controlThrow = false;
                Player.controlTorch = false;
                Player.controlSmart = false;
                Player.controlMount = false;

                if (Player.controlInv)
                {
                    DialogueUI.CurrentDialogueSystem.CloseDialogue();
                    DialogueUI.CurrentDialogueSystem.AssociatedNPC = -1;
                    DialogueUI.CurrentDialogueSystem = null;
                }
            }
        }
    }
}
