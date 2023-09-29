using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience;

public abstract class AmbienceTrack : ModType
{
    // TODO: Note to self - Naka - Figure out a way for me to be satisfied with this code.
    private SlotId loopSlot;
    private SlotId soundSlot;

    public SoundStyle Loop { get; protected set; }

    public virtual int PlaybackRate { get; protected set; } = 100;

    protected sealed override void Register() {
        Initialize();
        
        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    internal void Update() {
        UpdateLoop();
        UpdateSounds();
    }

    private void UpdateLoop() {

    }

    private void UpdateSounds() {

    }

    protected abstract void Initialize();
}
