using Terraria.DataStructures;

namespace EndlessEscapade.Core.Graphics;

public interface IPlayerDrawModifier
{
    void ModifyDrawInfo(ref PlayerDrawSet drawInfo);
}
