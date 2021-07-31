using System;

namespace FourWallsInc.Utilities
{
	public static class StringExtensions
	{
		// Converts a string to camel casing.
		public static string ToCamelCase (this string variable)
		{
			// Trim extra spaces.
			variable = (variable == null) ? String.Empty : variable.Trim ();

			// Is it empty?
			if (String.IsNullOrEmpty (variable) || String.IsNullOrWhiteSpace (variable.Trim ()))
			{
				return String.Empty;
			}

			// Camel-case it.
			var firstChar = variable [0];
			return firstChar.ToString ().ToLower () + variable.Substring (1);
		}

		// Gets a CORS (Cross-origin Resource Sharing) URI from the specified URI. For example, if the specified URI
		// is http://www.example.com/index, then the corresponding CORS URI would be http://www.example.com.
		public static string GetCorsUri (this string uri)
		{
			// Is the URI empty or NULL?
			if (String.IsNullOrEmpty (uri) || String.IsNullOrWhiteSpace (uri.Trim ()))
			{
				return String.Empty;
			}

			// Get the "//" location
			var doubleSlashLocation = uri.IndexOf ("//");

			if (doubleSlashLocation == -1)
			{
				return String.Empty;
			}

			// Get the next appearance of "/" in the URI.
			var nextSlashLocation = uri.IndexOf ("/", doubleSlashLocation + 2);

			if (nextSlashLocation == -1)
			{
				return uri;
			}

			// Get the CORS URI.
			var corsUri = uri.Substring (0, nextSlashLocation);

			return corsUri;
		}
	}
}
