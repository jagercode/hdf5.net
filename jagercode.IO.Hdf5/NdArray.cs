using System;

namespace jagercode.Hdf5
{
	// TODO: determine whether this is the way (#2) or that the INdValue is to be implemented by DataSet and Attribute directly (#1). 
	// I think the latter but let's decide by comparing example scripts of the same scenario.
	// You have to remind that approach below requires in memory loading of the data for introspection whilst
	// #1 is more logical for an in memory versus memory mapped or on disk approach for hdf5 objects.
	// #2 can be made independent of the storage. Or serve as frontend to hide the implementation details. 
	//
	// If it is an array, it should have all the array operators. This is not the case. 
	// Array access via Value property. So this is more like a carrier or entry.
	// For this reason it is for now not added to the object model and instead
	// this approach is taken to the Attributes and DataSet individual.

	public interface INdArray
	{
		bool IsScalar { get; }
		ulong[] Shape { get; }
		Type Type { get; }
		object ValueAsObject { get; set; }
	}

	public interface INdArray<T> : INdArray
	{
		T Value { get; set; }
	}


	public class NdArray<T> : INdArray<T>
	{
		private readonly INdArray<T> _ndArray;

		public NdArray(T value)
		{
			try
			{
				_ndArray = new Array<T>(value);
			}
			catch (ArgumentException)
			{
				_ndArray = new Scalar<T>(value);
			}
		}

		public T Value { get => _ndArray.Value; set => _ndArray.Value = value; }

		public bool IsScalar => _ndArray.IsScalar;

		public ulong[] Shape => _ndArray.Shape;

		public Type Type => _ndArray.Type;

		public object ValueAsObject { get => _ndArray.ValueAsObject; set => _ndArray.ValueAsObject = value; }
	}

	internal class Scalar<T> : INdArray<T>
	{
		internal Scalar(T scalarValue)
		{
			Array a = scalarValue as Array;
			if (a != null)
			{
				throw new ArgumentException($"{nameof(scalarValue)}: expected scalar value");
			}

			Value = scalarValue;
		}

		public T Value { get; set; }

		public bool IsScalar => true;

		public ulong[] Shape => new ulong[0];

		public Type Type => Value.GetType();

		public object ValueAsObject { get => Value; set => Value = (T)value; }
	}

	internal class Array<T> : INdArray<T>
	{

		internal Array(T arrayValue)
		{
			Array array = arrayValue as Array;
			if (null == array)
			{
				throw new ArgumentException($"{nameof(arrayValue)}: expected array type");
			}

			Value = arrayValue;
			ulong[] shape = new ulong[array.Rank];
			for (int u = 0; u < array.Rank; u++)
			{
				shape[u] = (ulong)array.GetLength(u);
			}

			Shape = shape;
			Type = array.GetType().GetElementType();
		}

		public T Value { get; set; }

		public bool IsScalar => false;

		public ulong[] Shape { get; }

		public Type Type { get; }

		public object ValueAsObject { get => Value; set => Value = (T)value; }
	}

}

