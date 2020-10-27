using EEMod.Tiles.EmptyTileArrays;
using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
	public class CrystalSerializer : TagSerializer<Crystal, TagCompound>
	{
		public override TagCompound Serialize(Crystal value) => new TagCompound
		{
			["position"] = value.position,
			["texture"] = value.tex,
			["glowmask"] = value.glow
		};

		public override Crystal Deserialize(TagCompound tag) => new Crystal(tag.Get<Vector2>("position"), tag.GetString("texture"), tag.GetString("glowmask"));
	}
	public class BigCrystalSerializer : TagSerializer<BigCrystal, TagCompound>
	{
		public override TagCompound Serialize(BigCrystal value) => new TagCompound
		{
			["position"] = value.position,
			["texture"] = value.tex,
			["glowmask"] = value.glow
		};

		public override BigCrystal Deserialize(TagCompound tag) => new BigCrystal(tag.Get<Vector2>("position"), tag.GetString("texture"), tag.GetString("glowmask"));
	}
}