using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	public class DataSet : INode, INdEntry
	{

		#region INode implementation
		public string Name { get; set; }
		public AttributeCollection Attributes {get;}
		public string Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		#endregion

		#region INdEntry implementation
		// approach 1: DataSet IS an INdArray with attributes and a name
		public ulong[] Shape { get; }
		public bool IsScalar => Shape.Length == 0;
		public Type ElementType { get; }
		public T Get<T>(){ throw new NotImplementedException(); }
		public void Set<T>(T value) { }
		public object ValueAsObject { get; set; }
		#endregion
		// approach 2: DataSet HAS a INdArray like it has a Name and Attributes
		//public INdArray Value { get; set; }
	}

	public class DataSet<TValue> : DataSet
	{
		// nameless dataset; note that name must be set before adding to a group. 
		public DataSet(TValue value) { }

		public DataSet(string name, TValue value)
		{ }

		public TValue Value { get => base.Get<TValue>(); set=>base.Set<TValue>(value); }

		public static implicit operator TValue(DataSet<TValue> ds) => ds.Value;
	}


	public class DataSetCollection : IEnumerable<DataSet>
	{
		// bummer: https://stackoverflow.com/questions/494827/why-it-is-not-possible-to-define-generic-indexers-in-net
		public DataSet this[string path]
		{
			get { return default(DataSet); }
			set { }
		}

		public IDictionary<string, object> AsDictionary() { return new Easy.DataSetDictionary(this); }


		public void Add<T>(string name, T value)
		{ }

		public void Add(DataSet dataSet)
		{ }

		public DataSet Create<T>(string name, T value) { throw new NotImplementedException(); }

		public IEnumerator<DataSet> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
