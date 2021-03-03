using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	public abstract class Attribute : INdArray
	{
		public string Name { get; set; }

		// public INdArray Value { get; set; }

		public bool IsScalar => throw new NotImplementedException();

		public ulong[] Shape => throw new NotImplementedException();

		public Type Type => throw new NotImplementedException();

		public object ValueAsObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}

	public sealed class Attribute<T> : Attribute, INdEntry<T>
	{
		public T Value { get; set; }
	}
}
