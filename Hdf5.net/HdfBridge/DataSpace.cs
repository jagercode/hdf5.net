using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using HDF.PInvoke;
//

namespace Hdf5.HdfBridge
{
	internal static class DataSpace
	{
		// common data space methods go here.
		private static SafeIdHandle CreateSimple(ulong[] shape)
		{
			Id dataspaceId = H5S.create_simple(shape.Length, shape, null);
			if (!dataspaceId.IsValid)
			{
				throw new InvalidOperationException($"Failed to create dataspace.");
			}
			return new SafeIdHandle(dataspaceId, H5S.close);
		}

		public static SafeIdHandle CreateFor<T>(T value)
		{
			var shape = GetShapeOf(value);
			return CreateSimple(shape);
		}

		public static void GetShapeAndElementTypeOf<T>(T value, out Type elementType, out ulong[] shape)
		{
			shape = GetShapeOf(value);

			Array a = value as Array;
			if (null == a)
			{
				// scalar or compound.
				elementType = typeof(T); 
				return;
			}
			// array
			elementType = typeof(T).GetElementType() ?? throw new ArgumentException("Unknown element type", nameof(value));
		}

		private static ulong[] GetShapeOf<T>(T value)
		{
			Array a = value as Array;

			if (null == a)
			{
				// scalar
				return new ulong[0];
			}

			// array
			ulong[] shape = new ulong[a.Rank];
			for (int d = 0; d < shape.Length; d++)
			{
				shape[d] = (ulong)a.GetLength(d);
			}
			return shape;
		}

		internal static SafeIdHandle Open(Id datasetId)
		{
			return new SafeIdHandle(H5D.get_space(datasetId), H5S.close);
		}
	}
}
