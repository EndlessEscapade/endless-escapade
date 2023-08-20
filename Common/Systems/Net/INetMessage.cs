using System.IO;

namespace EndlessEscapade.Common.Systems.Net;

/// <summary>Defines a net message.</summary>
/// <typeparam name="TSelf"></typeparam>
public interface INetMessage<TSelf>
{
    void Write(BinaryWriter writer);
    TSelf Read(BinaryReader reader);
}
