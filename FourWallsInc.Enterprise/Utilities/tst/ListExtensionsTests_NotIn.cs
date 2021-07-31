using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using FourWallsInc.Utilities;

namespace Given_that_a_list_of_values_and_a_match_string_are_available
{
	/// <summary>
	/// Contains tests for lists extensions.
	/// </summary>
	[TestClass]
	public class When_I_call_NotIn_method_to_check_if_match_string_is_present
	{
		[TestMethod]
		public void It_should_return_correct_abscence_status ()
		{
			// List down all possibilities.
			var matchDictionaryForInCheck
				= new Dictionary<KeyValuePair<string, string []>, bool>
					{
						{ new KeyValuePair<string, string []> ("a", new [] { "a", "b", "c" }), false },
						{ new KeyValuePair<string, string []> ("a", new [] { "a" }), false },
						{ new KeyValuePair<string, string []> ("c", new [] { "a", "b" }), true },
						{ new KeyValuePair<string, string []> ("a", new [] { String.Empty, null }), true },
						{ new KeyValuePair<string, string []> (String.Empty, new [] { String.Empty, null }), false },
						{ new KeyValuePair<string, string []> (null, new [] { String.Empty, null }), false },
						{ new KeyValuePair<string, string []> (" ", new [] { " ", null }), false },
					};

			bool matchStringIsPresent;

			// Check each one of them.
			foreach (var onePair in matchDictionaryForInCheck)
			{
				matchStringIsPresent = onePair.Key.Key.NotIn (onePair.Key.Value);
				matchStringIsPresent.Should ().Be (onePair.Value);
			}
		}
	}
}