using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	public class DataSet
	{
		public string Name { get; set;  }
		public ulong[] Shape { get; }
		public Type Type { get; }
		public void Get<T>(out T value){ throw new NotImplementedException(); }
		public void Set<T>(T value) { }
		public object ValueAsObject { get; set; }
	}

	//public class DataSet<TValue> : DataSet
	//{
	//	public TValue Value { get; set; }

	//	public static implicit operator TValue(DataSet<TValue> ds)
	//	{
	//		return ds.Get<TValue>();
	//	}
	//}


	public class DataSetCollection : IEnumerable<DataSet>
	{
		// bummer: https://stackoverflow.com/questions/494827/why-it-is-not-possible-to-define-generic-indexers-in-net
		public DataSet this[string path]
		{
			get { return default(DataSet); }
			set { }
		}

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
