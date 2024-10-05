namespace EndlessEscapade.Content.Walls.Tropical;

public class TropicalWoodWall : ModWall
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.wallHouse[Type] = true;

        AddMapEntry(new Color(158, 106, 74));

        HitSound = SoundID.Dig;
        DustType = DustID.WoodFurniture;
    }
}
