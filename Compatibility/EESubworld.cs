using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Compatibility
{
    public class EESubworld : ModWorld
    {
        public static bool enteredWorld = false;

        public override void Initialize()
        {
            enteredWorld = false;
        }

        public override void PreUpdate()
        {
            //This hook also runs in the subworld if we choose to specify ExclusiveWorld for the "Register"
        }

        public override void PostUpdate()
        {
            //This hook also runs in the subworld if we choose to specify ExclusiveWorld for the "Register"
            if (!(EESub.IsActive(EESub.mySubworldID) ?? false)) return; //No point executing the code below if we aren't in the subworld we want

            if (!enteredWorld)
            {
                enteredWorld = true;
                string message = $"Hey, We successfully entered '{EESub.mySubworldID}' and only the '{Name}' will update here!";
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), Color.Orange);
                }
                else
                {
                    Main.NewText(message, Color.Orange);
                }
            }
        }
    }
}
