using System;

namespace FourWallsInc.Utilities
{
	public static class ObjectExtensions
	{
		public static bool IsAPrimitiveType (this Type typeToCheck)
		{
			return
				typeToCheck.In
					(
						typeof (short),
						typeof (int),
						typeof (long),
						typeof (float),
						typeof (double),
						typeof (decimal),
						typeof (char),
						typeof (string),
						typeof (DateTime)
					);
		}

		public static bool IsPrimitiveTypeDefaultValue (this object instance)
		{
			return
			   instance.Equals (default (short))
				|| instance.Equals (default (int))
				|| instance.Equals (default (long))
				|| instance.Equals (default (float))
				|| instance.Equals (default (double))
				|| instance.Equals (default (decimal))
				|| instance.Equals (default (char))
				|| instance.Equals (default (string))
				|| instance.Equals (default (DateTime));
		}
	}
}
