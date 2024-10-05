namespace EndlessEscapade.Content.Walls.Shallows;

public class LightGemsandWall : ModWall
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(53, 87, 83));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
