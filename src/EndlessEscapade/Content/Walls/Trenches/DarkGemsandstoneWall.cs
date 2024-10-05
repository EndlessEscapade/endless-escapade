namespace EndlessEscapade.Content.Walls.Trenches;

public class DarkGemsandstoneWall : ModWall
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(50, 56, 102));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
