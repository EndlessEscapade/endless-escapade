using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
#nullable enable

namespace EndlessEscapade.Common.Systems.Net
{
    public class NetSystem : ModSystem
    {
        public static int MessageCount { get; internal set; }
        private static Dictionary<int, IMessageHandler> netMessagesHandlers = new();

        /// <summary>Returns the assigned ID for the specified packet or 0 if the packet has not been registered.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The net ID of the packet.</returns>
        public static int PacketID<T>() where T : INetMessage<T> {
            return MessageInfo<T>.ID;
        }
        /*/// <summary>Allows registering for a custom action under a net ID.</summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int RegisterCustomID(Action<BinaryReader, int> callback, string? debugName) {

        }*/
        internal static void HandleModRecievedPacket(BinaryReader reader, int fromWho) {
            int messageID = reader.ReadByte();
            if (netMessagesHandlers.TryGetValue(messageID, out var value))
                value.Handle(reader, fromWho);
            else
                EndlessEscapade.Instance.Logger.Warn($"A message of type {messageID} has been recieved but no packet is associated with such ID");
        }
        
        public override void Load() {
            base.Load();

            Stopwatch sw = Stopwatch.StartNew();
            AutoloadNetMessages();
            sw.Stop();
            Mod.Logger.Info($"Net messages loading time: {sw.Elapsed.TotalMilliseconds}ms");
        }

        void AutoloadNetMessages() {
            Dictionary<Type, List<MethodInfo>> methodPackageHandlers = new();
            const BindingFlags FlagsStatic = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            foreach (Type type in AssemblyManager.GetLoadableTypes(Mod.Code).OrderBy(t=>t.MetadataToken)) {
                // get listener methods
                // specifically methods from generic types cannot be invoked without a type arg
                if (type.IsGenericType)
                    continue;
                foreach (MethodInfo method in type.GetMethods(FlagsStatic)) {
                    if (method.GetParameters() is not { Length: 1 } parameters || method.ReturnType != typeof(void) || method.GetCustomAttribute<MessageHandlerAttribute>() is not { } attribute)
                        continue;
                    if (parameters[0].ParameterType != attribute.TargetMessageType) {
                        Mod.Logger.Error($"{nameof(NetSystem)}: Method {type.FullName}::{method.Name} cannot be registered for listening to packets because the method's parameter is not of type {attribute.TargetMessageType}");
                        continue;
                    }
                    if (!methodPackageHandlers.TryGetValue(attribute.TargetMessageType, out var list))
                        methodPackageHandlers[attribute.TargetMessageType] = list = new();
                    list.Add(method);

                }

                // create the messages
                Type netMessageGeneric = typeof(INetMessage<>).MakeGenericType(type);
                if (type.IsAbstract || !type.IsAssignableTo(netMessageGeneric))
                    continue;

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
            }
        }

        class CustomIDMessageHandler : IMessageHandler
        {
            public object? PacketInstance => null;

            public string? DebugName { get; set; }

            Action<BinaryReader, int> callback;
            public CustomIDMessageHandler(Action<BinaryReader, int> callback, string? debugName = null) {
                DebugName = debugName;
                this.callback = callback;
            }
            public void Handle(BinaryReader reader, int fromWho) {
                callback(reader, fromWho);
            }
        }
    }

    record struct TestMessage(int Data) : INetMessage<TestMessage>
    {
        public readonly TestMessage Read(BinaryReader reader) {
            return new(reader.ReadInt32());
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
}
