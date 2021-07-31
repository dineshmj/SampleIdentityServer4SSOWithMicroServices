using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace CMS.Quotes.App.Middleware
{
	public class CustomResponseHeaderMiddleware
	{
		private readonly RequestDelegate requestDelegate;
		private readonly CustomResponseHeadersPolicy policy;

		public CustomResponseHeaderMiddleware
			(
				RequestDelegate requestDelegate,
				CustomResponseHeadersPolicy policy
			)
		{
			this.requestDelegate = requestDelegate;
			this.policy = policy;
		}

		public async Task Invoke (HttpContext context)
		{
			IHeaderDictionary headers = context.Response.Headers;

			foreach (var headerValuePair in this.policy.SetHeaders)
			{
				headers [headerValuePair.Key] = headerValuePair.Value;
			}

			foreach (var header in this.policy.RemoveHeaders)
			{
				headers.Remove (header);
			}

			await this.requestDelegate (context);
		}
	}
}