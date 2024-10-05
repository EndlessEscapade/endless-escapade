namespace EndlessEscapade.Content.Walls.Shallows;

public class LightGemsandstoneWall : ModWall
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(48, 74, 78));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
