using System;

namespace EndlessEscapade.Common.Systems.Net;

/// <summary>Static methods with this attribute are automatically registered to <see cref="Message{T}.OnRecieve" />.</summary>
/// <remarks>The method must return void and contain a single parameter for the packet (<c>static void AnyName(T anyName)</c>)</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class MessageHandlerAttribute : Attribute
{
<<<<<<< HEAD
    public MessageHandlerAttribute(Type targetMessageType) {
        TargetMessageType = targetMessageType;
=======
    /// <summary>Static methods with this attribute are automatically registered to <see cref="MessageInfo{T}.OnRecieve"/>.</summary>
    /// <remarks>The method must return void and contain a single parameter for the packet (<c>static void AnyName(T anyName)</c>)</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MessageHandlerAttribute : Attribute
    {
        public Type TargetMessageType { get; }
        public MessageHandlerAttribute(Type targetMessageType) {
            TargetMessageType = targetMessageType;
        }
>>>>>>> 16c34049c6f66190f06d86ba3d55a46a5cc023b0
    }

    public Type TargetMessageType { get; }
}
