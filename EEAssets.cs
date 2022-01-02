using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace EEMod
{
    internal class EEAssets : ModSystem
    {
        public static T Get<T>(string path, bool immediateLoad = false) where T : class
        {
            return GetAsset<T>(path, immediateLoad).Value;
        }

        public static Asset<T> GetAsset<T>(string path, bool immediateLoad = true) where T : class
        {
            return GetAsset<T>(path, immediateLoad ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad);
        }

        public static Asset<T> GetAsset<T>(string path, AssetRequestMode mode) where T : class
        {
            return EEMod.Instance.Assets.Request<T>(path, mode);
        }

        public static Texture2D GetTexture(string path, bool immediateLoad = true)
        {
            return Get<Texture2D>(path, immediateLoad);
        }

        public static Asset<Texture2D> GetTextureAsset(string path, bool immeidateLoad = true)
        {
            return GetAsset<Texture2D>(path, immeidateLoad);
        }
    }
}
