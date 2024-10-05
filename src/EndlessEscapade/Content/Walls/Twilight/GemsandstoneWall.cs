namespace EndlessEscapade.Content.Walls.Twilight;

public class GemsandstoneWall : ModWall
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(31, 74, 93));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
