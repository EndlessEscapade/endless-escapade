using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;

namespace EEMod
{
    // This class stores necessary player info for our custom damage class, such as damage multipliers and additions to knockback and crit.
    public class EEDamagePlayer : ModPlayer
    {
        public static EEDamagePlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<EEDamagePlayer>();
        }

        // Vanilla only really has damage multipliers in code
        // And crit and knockback is usually just added to
        // As a modder, you could make separate variables for multipliers and simple addition bonuses
        public float prophetDamageAdd;
        public float prophetDamageMult = 1f;
        public float prophetKnockback;
        public int prophetCrit;
        public double prophetLifeCostReduce;
        public bool prophetItemHold;

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            prophetDamageAdd = 0f;
            prophetDamageMult = 1f;
            prophetKnockback = 0f;
            prophetCrit = 0;
            prophetLifeCostReduce = 1;
            prophetItemHold = false;
        }
    }
}
