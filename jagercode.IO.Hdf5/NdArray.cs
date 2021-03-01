using System;

namespace jagercode.IO.Hdf5
{
	// TODO: determine whether this is the way or that the INdValue is to be implemented by DataSet and Attribute directly. 
	// I think the latter but let's decide by comparing example scripts of the same scenario.

	public interface INdArray
	{
		bool IsScalar { get; }
		ulong[] Shape { get; }
		Type Type { get; }
		object ValueAsObject { get; }
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

		public object ValueAsObject => _ndArray.ValueAsObject;
	}

	class Scalar<T> : INdArray<T>
	{
		internal Scalar(T scalarValue)
		{
			var a = scalarValue as Array;
			if (a != null) throw new ArgumentException($"{nameof(scalarValue)}: expected scalar value");
			Value = scalarValue;
		}

		public T Value { get; set; }

		public bool IsScalar => true;

		public ulong[] Shape => new ulong[0];

		public Type Type => Value.GetType();

		public object ValueAsObject => Value;
	}


	 class Array<T> : INdArray<T>
	{

		internal Array(T arrayValue)
		{
			var array = arrayValue as Array;
			if (null == array) throw new ArgumentException($"{nameof(arrayValue)}: expected array type");
			Value = arrayValue;
			var shape = new ulong[array.Rank];
			for (var u = 0; u < array.Rank; u++)
				shape[u] = (ulong) array.GetLength(u);
			Shape = shape;
			Type = array.GetType().GetElementType();
		}

		public T Value { get; set; }

		public bool IsScalar => false;

		public ulong[] Shape { get; }

		public Type Type { get; }

		public object ValueAsObject => Value;
	}

}

