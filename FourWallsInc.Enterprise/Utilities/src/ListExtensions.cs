using System.Linq;

namespace FourWallsInc.Utilities
{
	// Contains extension methods for lists.
	public static class ListExtensions
	{
		// Checks if the specified item is present among the match values.
		public static bool In<TType> (this TType item, params TType [] matchValues)
		{
			return matchValues.Any (mv => (mv == null && item == null) || (mv != null && mv.Equals (item)));
		}

		// Checks if the specified item is not present among the match values.
		public static bool NotIn<TType> (this TType item, params TType [] matchValues)
		{
			return ! matchValues.Any (mv => (mv == null && item == null) || (mv != null && mv.Equals (item)));
		}
	}
}
