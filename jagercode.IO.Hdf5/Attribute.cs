using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	public abstract class Attribute : INdEntry
	{
		public string Name { get; set; }

		public Type ElementType { get; }

		public T Get<T>() => throw new NotImplementedException();

		public void Set<T>(T value)
		{
			throw new NotImplementedException();
		}

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

	public sealed class AttributeCollection : IEnumerable<Attribute>
	{
		public Attribute this[string name] { get=>throw new NotImplementedException();set => throw new NotImplementedException(); }
		public void Add<T>(string name, T value) { }
		public Attribute Create<T>(string name, T value) { throw new NotImplementedException(); }

		public IEnumerator<Attribute> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
