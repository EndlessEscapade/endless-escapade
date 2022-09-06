using Terraria;
using Terraria.ModLoader;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace EEMod.Systems
{
	public class TwitterAPIKeys
    {
        public static ConcurrentDictionary<string, string> APIKeys { get; set; }

		public static void LoadAPIKeys()
		{
			APIKeys = new ConcurrentDictionary<string, string>();

			APIKeys.TryAdd("APIKEY", "You");
			APIKeys.TryAdd("APISecret", "MothaFucking");

			APIKeys.TryAdd("AccessToken", "Thought-Bitch");
			APIKeys.TryAdd("AccessSecret", "Get off");

			APIKeys.TryAdd("BearerToken", "My Swamp");
		}
    }
}