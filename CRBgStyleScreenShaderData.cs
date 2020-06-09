using Terraria.Graphics.Shaders;

namespace EEMod
{
    public class CRBgStyleScreenShaderData : ScreenShaderData
    {
        private int CRIndex;

        public CRBgStyleScreenShaderData(string passName) : base(passName)
        {
        }

        private void UpdateCRIndex()
        {
            CRIndex = 0;
            return;
        }

        public override void Apply()
        {
            UpdateCRIndex();
            base.Apply();
        }
    }
}
