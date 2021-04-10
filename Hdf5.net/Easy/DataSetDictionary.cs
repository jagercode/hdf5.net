using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdf5.Easy
{

	// needed? syntactic sugar? how to use generic method from object? 
	public class DataSetDictionary : IDictionary<string, object>
	{
		private DataSetList _collection;

		internal DataSetDictionary(DataSetList collection)
		{
			_collection = collection;
		}

		// public object this[string name] { get;set; }
		public object this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public ICollection<string> Keys => throw new NotImplementedException();

		public ICollection<object> Values => throw new NotImplementedException();

		public int Count => throw new NotImplementedException();

		public bool IsReadOnly => throw new NotImplementedException();

		public void Add(string key, object value)
		{
			throw new NotImplementedException();
		}

		public void Add(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public bool ContainsKey(string key)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public bool Remove(string key)
		{
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue(string key, out object value)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
