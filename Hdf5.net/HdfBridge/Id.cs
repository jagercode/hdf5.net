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

		public static implicit operator hid_t(Id id) => id._id; 

		public static implicit operator Id(hid_t id) => new Id(id);

		// alternative for using? 
		internal static void With(Id id, Action action, Func<hid_t, herr_t> closeFunc)
		{

		}
	}
}
