using System;

namespace Hdf5
{
	/// <summary>
	/// Interface for entries containing zero to n dimensional data of a single element type. 
	/// Generic on the getter and setter method; Using a nice indexer requires a generic type which makes iterating over datasets of different types difficult.
	/// </summary>
	public interface INdEntry
	{
		bool IsScalar { get; }
		ulong[] Shape { get; }
		Type ElementType { get; }
		object ValueAsObject { get; set; }
		T Get<T>();
		void Set<T>(T value);
		// todo: TResult GetSlice(params Slice[] slices), get a subset of the data of the entry. Same element type but maybe different shape (dim N-1).
		// todo: ToEntry<T>(), convert to interface with indexer
	}

	public interface INdEntry<T> : INdEntry
	{
		T Value { get; set; }
		// T this[params Slice[] slices] { get; } Get a subset by slicing of the same shape. 
	}
}
