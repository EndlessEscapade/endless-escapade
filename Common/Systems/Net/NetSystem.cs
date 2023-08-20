using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace EndlessEscapade.Common.Systems.Net;

public class NetSystem : ModSystem
{
    private static readonly Dictionary<int, IMessageHandler> netMessagesHandlers = new Dictionary<int, IMessageHandler>();
    
    public static int MessageCount { get; internal set; }

<<<<<<< HEAD
    /// <summary>Returns the assigned ID for the specified packet or 0 if the packet has not been registered.</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The net ID of the packet.</returns>
    public static int PacketID<T>() where T : INetMessage<T> {
        return Message<T>.ID;
    }
    
    internal static void HandleModRecievedPacket(BinaryReader reader, int fromWho) {
        int messageID = reader.ReadByte();
        if (netMessagesHandlers.TryGetValue(messageID, out var value)) {
            value.Handle(reader, fromWho);
=======
        /// <summary>Returns the assigned ID for the specified packet or 0 if the packet has not been registered.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The net ID of the packet.</returns>
        public static int PacketID<T>() where T : INetMessage<T> {
            return MessageInfo<T>.ID;
>>>>>>> 16c34049c6f66190f06d86ba3d55a46a5cc023b0
        }
        else {
            EndlessEscapade.Instance.Logger.Warn($"A message of type {messageID} has been recieved but no packet is associated with such ID");
        }
    }

    public override void Load() {
        AutoloadNetMessages();
    }

    private void AutoloadNetMessages() {
        const BindingFlags FlagsStatic = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        
        var methodPackageHandlers = new Dictionary<Type, List<MethodInfo>>();
        
        foreach (var type in AssemblyManager.GetLoadableTypes(Mod.Code).OrderBy(t => t.MetadataToken)) {
            if (type.IsGenericType) {
                continue;
            }

            foreach (var method in type.GetMethods(FlagsStatic)) {
                if (method.GetParameters() is not { Length: 1 } parameters || method.ReturnType != typeof(void) || method.GetCustomAttribute<MessageHandlerAttribute>() is not { } attribute) {
                    continue;
                }

                if (parameters[0].ParameterType != attribute.TargetMessageType) {
                    Mod.Logger.Error(
                        $"{nameof(NetSystem)}: Method {type.FullName}::{method.Name} cannot be registered for listening to packets because the method's parameter is not of type {attribute.TargetMessageType}"
                    );
                    continue;
                }

<<<<<<< HEAD
                if (!methodPackageHandlers.TryGetValue(attribute.TargetMessageType, out var list)) {
                    methodPackageHandlers[attribute.TargetMessageType] = list = new List<MethodInfo>();
                }

                list.Add(method);
            }

            // create the messages
            var netMessageGeneric = typeof(INetMessage<>).MakeGenericType(type);
            if (type.IsAbstract || !type.IsAssignableTo(netMessageGeneric)) {
                continue;
=======
                // value types dont require a ctor
                if (!(type.IsValueType || type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes) != null))
                    continue;
                object instance = Activator.CreateInstance(type)!;
                if (!type.IsValueType) // structs cannot be fetched by ContentInstance.GetInstance
                    ContentInstance.Register(instance);
                Type genericMessageType = typeof(MessageInfo<>).MakeGenericType(type);
                genericMessageType.GetMethod("Load", FlagsStatic)!.Invoke(null, new object[] { instance });
                int id = (int)genericMessageType.GetProperty("ID", FlagsStatic)!.GetValue(null)!;

                netMessagesHandlers[id] = (IMessageHandler)Activator.CreateInstance(typeof(MessageInfo<>).MakeGenericType(type))!;
            }
            foreach ((Type messageType, List<MethodInfo> handlers) in methodPackageHandlers) {
                EventInfo onRecieveEvent = typeof(MessageInfo<>).MakeGenericType(messageType).GetEvent("OnRecieve", FlagsStatic)!;
                Type actionType = typeof(Action<>).MakeGenericType(messageType);
                foreach (MethodInfo handler in handlers)
                    onRecieveEvent.AddEventHandler(null, handler.CreateDelegate(messageType));
>>>>>>> 16c34049c6f66190f06d86ba3d55a46a5cc023b0
            }

            // value types dont require a ctor
            if (!(type.IsValueType || type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes) != null)) {
                continue;
            }

            var instance = Activator.CreateInstance(type)!;
            if (!type.IsValueType) // structs cannot be fetched by ContentInstance.GetInstance
            {
                ContentInstance.Register(instance);
            }

            var genericMessageType = typeof(Message<>).MakeGenericType(type);
            genericMessageType.GetMethod("Load", FlagsStatic)!.Invoke(null, new[] { instance });
            var id = (int)genericMessageType.GetProperty("ID", FlagsStatic)!.GetValue(null)!;

            netMessagesHandlers[id] = (IMessageHandler)Activator.CreateInstance(typeof(Message<>).MakeGenericType(type))!;
        }

        foreach ((var messageType, var handlers) in methodPackageHandlers) {
            var onRecieveEvent = typeof(Message<>).MakeGenericType(messageType).GetEvent("OnRecieve", FlagsStatic)!;
            var actionType = typeof(Action<>).MakeGenericType(messageType);
            foreach (var handler in handlers) {
                onRecieveEvent.AddEventHandler(null, handler.CreateDelegate(messageType));
            }
        }
    }

    private class CustomIDMessageHandler : IMessageHandler
    {
        private readonly Action<BinaryReader, int> callback;

        public CustomIDMessageHandler(Action<BinaryReader, int> callback, string? debugName = null) {
            DebugName = debugName;
            this.callback = callback;
        }

        public object? PacketInstance => null;

        public string? DebugName { get; }

        public void Handle(BinaryReader reader, int fromWho) {
            callback(reader, fromWho);
        }
    }
}

internal record struct TestMessage(int Data) : INetMessage<TestMessage>
{
    public readonly TestMessage Read(BinaryReader reader) {
        return new TestMessage(reader.ReadInt32());
    }

    public readonly void Write(BinaryWriter writer) {
        writer.Write(Data);
    }
}

/// <summary>Reserved for internal impl details. Do not use.</summary>
internal interface IMessageHandler
{
    object? PacketInstance { get; }
    string? DebugName { get; }
    void Handle(BinaryReader reader, int fromWho);
}
