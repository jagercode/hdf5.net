using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.Hdf5.Internal
{
	internal static class Shape
	{
		public static string ToString(ulong[] shape)
		{
			if (null == shape) return string.Empty;
			return $"[{string.Join(",", shape.Select(ul => ul.ToString()))}]";
		}
	}
}
