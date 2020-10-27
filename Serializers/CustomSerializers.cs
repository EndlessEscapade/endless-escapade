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
	public class EmptyTileEntitySerializer : TagSerializer<EmptyTileDrawEntity, TagCompound>
	{
		public override TagCompound Serialize(EmptyTileDrawEntity value)
		{
			if (value.GetType().Name == "BigCrystal")
			{
				return new TagCompound
				{
					["position"] = value.position,
					["texture"] = value.tex,
					["glowBC"] = (value as BigCrystal).glowPath,
				};
			}
			if (value.GetType().Name == "Crystal")
			{
				return new TagCompound
				{
					["position"] = value.position,
					["texture"] = value.tex,
					["glowC"] = (value as Crystal).glowPath,
				};
			}
			return new TagCompound
			{
				["position"] = value.position,
				["texture"] = value.tex,
			};
		}

		public override EmptyTileDrawEntity Deserialize(TagCompound tag)
		{ 
            if (tag.ContainsKey("glowBC"))
                return new BigCrystal(tag.Get<Vector2>("position"), tag.GetString("texture"), tag.GetString("glowBC"));
            else if (tag.ContainsKey("glowC"))
                return new Crystal(tag.Get<Vector2>("position"), tag.GetString("texture"), tag.GetString("glowC"));
			else
				return new EmptyTileDrawEntity(tag.Get<Vector2>("position"), tag.GetString("texture"));
		}
	}
}