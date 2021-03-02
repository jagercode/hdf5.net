using System;

namespace jagercode.IO.Hdf5
{
	public interface INdEntry
	{
		bool IsScalar { get; }
		ulong[] Shape { get; }
		Type Type { get; }
		object ValueAsObject { get; set; }
	}

	public interface INdEntry<T>
	{
		T Value { get; set; }
	}
}
