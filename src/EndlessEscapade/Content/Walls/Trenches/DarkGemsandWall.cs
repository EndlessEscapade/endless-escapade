namespace EndlessEscapade.Content.Walls.Trenches;

public class DarkGemsandWall : ModWall
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.wallHouse[Type] = true;

        AddMapEntry(new Color(64, 70, 116));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
