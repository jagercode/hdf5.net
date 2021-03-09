using HDF.PInvoke;

namespace jagercode.IO.Hdf5.Internal
{
	using hid_t = System.Int64;

	internal struct Id
	{
		private hid_t _id;
		Id(hid_t id)
		{
			_id = id;
		}

		public bool IsValid => _id >= 0;

		public static implicit operator hid_t(Id id) => id._id; 

		public static implicit operator Id(hid_t id) => new Id(id);
	}
}
