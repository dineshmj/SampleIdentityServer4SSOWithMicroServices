using System.Linq;

namespace FourWallsInc.Utilities
{
	/// <summary>
	/// Contains extension methods for lists.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Checks if the specified item is present among the match values.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="matchValues">The match values.</param>
		/// <returns></returns>
		public static bool In<TType> (this TType item, params TType [] matchValues)
		{
			return matchValues.Any (mv => (mv == null && item == null) || (mv != null && mv.Equals (item)));
		}

		/// <summary>
		/// Checks if the specified item is not present among the match values.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="matchValues">The match values.</param>
		/// <returns></returns>
		public static bool NotIn<TType> (this TType item, params TType [] matchValues)
		{
			return ! matchValues.Any (mv => (mv == null && item == null) || (mv != null && mv.Equals (item)));
		}
	}
}