﻿using System;

namespace EndlessEscapade.Common.Systems.Net;

/// <summary>Static methods with this attribute are automatically registered to <see cref="Message{T}.OnRecieve" />.</summary>
/// <remarks>The method must return void and contain a single parameter for the packet (<c>static void AnyName(T anyName)</c>)</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class MessageHandlerAttribute : Attribute
{
    public MessageHandlerAttribute(Type targetMessageType) {
        TargetMessageType = targetMessageType;
    }

    public Type TargetMessageType { get; }
}
