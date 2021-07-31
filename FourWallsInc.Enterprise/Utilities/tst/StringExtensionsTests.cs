using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using FourWallsInc.Utilities;

namespace Given_that_a_PascalCased_variable_name_is_available
{
	/// <summary>
	/// Contains tests for the ".ToCamelCase ()" string extension method.
	/// </summary>
	[TestClass]
	public class When_I_call_ToCamelCase_extension_method_on_it
	{
		[TestMethod]
		public void It_should_return_its_camelCased_name ()
		{
			// List down all possibilities.
			var variableNamesDictionary
				= new Dictionary<string, string>
					{
						{ "FileName", "fileName" },
						{ "fileName", "fileName" },
						{ " FileName", "fileName" },
						{ "FileName ", "fileName" },
						{ " FileName ", "fileName" },
						{ " ", String.Empty },
						{ "\t", String.Empty },
						{ "\r", String.Empty },
						{ "\n", String.Empty },
						{ "\r\n", String.Empty },
						{ " \t\r\n", String.Empty },
						{ String.Empty, String.Empty }
					};

			string camelCasedVariableName;

			// Check each one of them.
			foreach (var onePair in variableNamesDictionary)
			{
				camelCasedVariableName = onePair.Key.ToCamelCase ();
				camelCasedVariableName.Should ().Be (onePair.Value);
			}

			// Perform the test with "null" as well.
			string variableName = null;
			camelCasedVariableName = variableName.ToCamelCase ();
			camelCasedVariableName.Should ().Be (String.Empty);
		}
	}
}