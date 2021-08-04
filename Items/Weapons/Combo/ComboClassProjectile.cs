using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Classes;

namespace EEMod.Items.Weapons.Combo
{
    public delegate void Combo();
    public abstract class ComboWeapon : EEProjectile, IComboProjectile
    {
        protected float progression => projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
        protected bool isFinished => progression >= 1;
        protected Player projOwner => Main.player[Projectile.owner];

        private int ComboSelection;
        public void SetCombo(int CurrentCombo) => ComboSelection = CurrentCombo;

        private readonly Dictionary<int, Combo> Combos = new Dictionary<int, Combo>();
        protected void AddCombo(int key, Combo combo)
        {
            if (!Combos.ContainsKey(key))
                Combos.Add(key, combo);
        }
        public override void AI()
        {
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Combos[ComboSelection].Invoke();
        }
    }
}