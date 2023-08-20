﻿using System;
using System.IO;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Net;

/// <summary>Contains information specific to a net message.</summary>
/// <typeparam name="T"></typeparam>
public class Message<T> : IMessageHandler where T : INetMessage<T>
{
    private static object? boxedSingleton;

    /// <summary>The ID that was given to this net message.</summary>
    public static int ID { get; internal set; }

    /// <summary>The registered singleton of this message type.</summary>
    /// <remarks>Can also be fetched with <see cref="ModContent.GetInstance{T}" /> (only if its not a <see langword="struct" />).</remarks>
    public static T Singleton { get; private set; } = default!;

    string? IMessageHandler.DebugName => typeof(T).Name;

    object? IMessageHandler.PacketInstance => boxedSingleton;

    void IMessageHandler.Handle(BinaryReader reader, int fromWho) {
        OnRecieve?.Invoke(Singleton.Read(reader), fromWho);
    }

    /// <summary>Invoked when a message of type T is recieved.</summary>
    public static event Action<T, int>? OnRecieve;

    internal static void Load(T instance) {
        Singleton = instance;
        boxedSingleton = instance;
        ID = ++NetSystem.MessageCount;
    }
}
