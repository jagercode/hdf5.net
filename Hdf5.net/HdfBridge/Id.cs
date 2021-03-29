using HDF.PInvoke;
using System;

namespace Hdf5.HdfBridge
{
	using hid_t =
#if H5_1_8
		System.Int32
#elif H5_1_10
		System.Int64
#else
		System.Int16 // to invoke compilation errors
#endif
		;
	using herr_t = System.Int32;

	internal struct Id
	{
		internal static readonly Id Invalid = -1;

		private hid_t _id;

		Id(hid_t id)
		{
			_id = id;
		}

		public bool IsValid => _id >= 0;

		/// <summary>
		/// sizeof(Id) would return wrapper size, not the size H5 should allocate.
		/// </summary>
		internal int H5AllocSize => sizeof(hid_t);

		public static implicit operator hid_t(Id id) => id._id; 

		public static implicit operator Id(hid_t id) => new Id(id);

		// alternative for using? 
		internal static void With(Id id, Action<Id> action, Func<hid_t, herr_t> closeFunc)
		{

		}

		/// <summary>
		/// Creates an array of low level Id data type. Initialized with the CLR default value.
		/// </summary>
		/// <param name="objCount"></param>
		/// <returns></returns>
		internal static hid_t[] CreateLowLevelArray(int objCount)
		{
			return new hid_t[objCount];
		}
	}
}
