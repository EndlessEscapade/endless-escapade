using Terraria;
using Terraria.ModLoader;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Tweetinvi;

namespace EEMod.Systems
{
	public class MessageListener
	{
		public async static void ListenForUser(string user)
		{
			try
			{
				var appClient = new TwitterClient(
						TwitterAPIKeys.APIKeys["APIKEY"], TwitterAPIKeys.APIKeys["APISecret"],
						TwitterAPIKeys.APIKeys["AccessToken"], TwitterAPIKeys.APIKeys["AccessSecret"]);

				var stream = appClient.Streams.CreateFilteredStream();
				var twitterUser = await appClient.Users.GetUserAsync(user);

				stream.AddFollow(twitterUser);

				stream.MatchingTweetReceived += (sender, eventReceived) =>
				{
					Main.NewText("The tweet: " + eventReceived.Tweet + " was sent by " + user);
				};

				await stream.StartMatchingAnyConditionAsync();
			}
			catch
			{
				
			}
		}
	}
}