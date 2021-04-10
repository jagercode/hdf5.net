using System;
using System.Linq;

namespace Hdf5.HdfBridge
{
	internal static class Shape
	{
		public static string ToString(ulong[] shape)
		{
			if (null == shape)
			{
				return string.Empty;
			}

			return $"[{string.Join(",", shape.Select(ul => ul.ToString()))}]";
		}

	}
}
