using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

namespace FourWallsInc.WebUI.Cookies
{
	public class MemoryCacheTicketStore
		: ITicketStore
	{
		private const string KEY_PREFIX = "AuthSessionStore-";
		private readonly IMemoryCache memoryCache;

		public MemoryCacheTicketStore ()
		{
			this.memoryCache = new MemoryCache (new MemoryCacheOptions ());
		}

		public async Task<string> StoreAsync (AuthenticationTicket ticket)
		{
			var guid = Guid.NewGuid ();
			var key = KEY_PREFIX + guid.ToString ();
			await RenewAsync (key, ticket);
			return key;
		}

		public Task RenewAsync (string key, AuthenticationTicket ticket)
		{
			var options = new MemoryCacheEntryOptions ();
			var expiresUtc = ticket.Properties.ExpiresUtc;

			if (expiresUtc.HasValue)
			{
				options.SetAbsoluteExpiration (expiresUtc.Value);
			}

			options.SetSlidingExpiration (TimeSpan.FromHours (1)); // TODO: configurable.

			this.memoryCache.Set (key, ticket, options);

			return Task.FromResult (0);
		}

		public Task<AuthenticationTicket> RetrieveAsync (string key)
		{
			AuthenticationTicket ticket;
			this.memoryCache.TryGetValue (key, out ticket);

			return Task.FromResult (ticket);
		}

		public Task RemoveAsync (string key)
		{
			this.memoryCache.Remove (key);
			return Task.FromResult (0);
		}
	}
}