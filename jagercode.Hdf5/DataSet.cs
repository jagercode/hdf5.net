using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using HDF.PInvoke;

namespace jagercode.Hdf5
{
	using Internal;

	// Q: Make it abstract? Unify dataset and attribute approach for consistency.
	// Q: start object based instead of generic? What's convenient? Object type might be more convenient for iterating and so, but feels outdated. 
	// So many decisions to make.... -_-
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


	   /// <summary>
		/// Dataset operations on HDF.PInvoke. 
		/// Q: Move to internal as eg a partial HDF class? Design decision: distribute HDF.PInvoke over the object model or have it as a separate "layer"
		/// </summary>
		internal static class HDF
		{
			internal static void With(Id id, Action action)
			{
				Id.With(id, action, H5D.close);
			}

			/// <summary>
			/// Reads the dataset1 d.
			/// </summary>
			/// <typeparam name="TElem">The type of the elem.</typeparam>
			/// <param name="fileId">The file identifier.</param>
			/// <param name="h5Path">The h5 path.</param>
			/// <param name="array">The array.</param>
			/// <exception cref="InvalidOperationException">
			/// Failed to open dataset at {h5Path}
			/// or
			/// Failed to open data space of dataset at {h5Path}
			/// </exception>
			/// <exception cref="ArgumentException">
			/// Dataset is {rank}D. Expected 1D dataset at {h5Path}
			/// or
			/// Array Element Type {typeof(TElem)} does not match stored data type {type}
			/// </exception>
			internal static void ReadDataset1D<TElem>(Id fileId, string h5Path, out TElem[] array)
			{
				// can easily be scaled up to N-D and scalar. 
				// might not work for TElem = string. (strings are awkward in hdf5)

				// Indirectly tested via Hdf5Editor and public read(..., out[] double) and read(..., out float[])
				Id id = H5D.open(fileId, h5Path, H5P.DEFAULT);
				if (!id.IsValid)
				{
					if (Path.Exists(fileId, h5Path)) throw new ArgumentException();
					throw new ArgumentException($"path not found: {h5Path}");
				}
				using (var dataSetHnd = new SafeIdHandle(id, H5D.close))
				using (var spaceHnd = new SafeIdHandle(H5D.get_space(dataSetHnd.Id), H5S.close))
				{
					if (!dataSetHnd.Id.IsValid)
						throw new InvalidOperationException($"Failed to open dataset at {h5Path}");
					if (!spaceHnd.Id.IsValid)
						throw new InvalidOperationException($"Failed to open data space of dataset at {h5Path}");

					// verify data types and size. 
					Id storageTypeId = H5D.get_type(dataSetHnd.Id);
					TypeMapping.GetMemTypesFromStorageType(storageTypeId, out Type type, out Id memTypeId);
					if (typeof(TElem) != type)
					{
						throw new ArgumentException($"Array Element Type {typeof(TElem)} does not match stored data type {type}", nameof(array));
					}

					int rank = H5S.get_simple_extent_ndims(spaceHnd.Id);
					if (rank != 1) throw new ArgumentException($"Dataset is {rank}D. Expected 1D dataset at {h5Path}");

					ulong[] dims = new ulong[rank];

					H5S.get_simple_extent_dims(spaceHnd.Id, dims, null);
					ulong arrayLength = 1;
					foreach (var l in dims)
					{
						arrayLength *= l;
					}

					// read array (shape may be inferred w/ H5S.get_simple_extent_ndims)
					array = new TElem[arrayLength];
					// Array arr = Array.CreateInstance(typeof(TElem), (int) arrayLength);
					using (var gch = new PinnedGCHandle(array))
					{
						H5D.read(dataSetHnd.Id, /*storageTypeId*/ memTypeId, H5S.ALL, H5S.ALL, H5P.DEFAULT,
							gch.AddressPtr);
					}
				}
			}


			internal static void GetArrayProperties<T>(T array, out ulong[] shape, out TypeMapping typeMapping)
			{
				if (!typeof(T).IsArray)
				{
					typeMapping = TypeMapping.Supported[typeof(T)];
					throw new ArgumentException($"argument type is not array", nameof(array));
				}

				Type elementType;
				Array a = array as Array;

				if (null == a)
				{
					shape = new ulong[0];
					elementType = typeof(T);
				}
				else
				{
					shape = new ulong[a.Rank];
					for (int d = 0; d < shape.Length; d++)
					{
						shape[d] = (ulong)a.GetLength(d);
					}
					elementType = typeof(T).GetElementType() ??
									  throw new ArgumentException("Unknown element type", nameof(array));
				}

				try
				{
					typeMapping = TypeMapping.Supported[elementType];
				}
				catch (KeyNotFoundException)
				{
					throw new NotImplementedException($"array of type {elementType}");
				}

			}


			internal static Id CreateDataset(Id containerId, string name, ulong[] shape, TypeMapping typeMapping, object values)
			{
				if (Path.Exists(containerId, name))
				{
					throw new ArgumentException(name); // todo: argument exception
				}

				var nameBytes = name.ToUtf8Bytes();

				Debug.WriteLine($"Dataset '{name}' utf-8 bytes: [{string.Join(", ", nameBytes.Select(b => b.ToString()))}]");

				var dataspaceId = Id.Invalid;
				try
				{
					dataspaceId = H5S.create_simple(shape.Length, shape, null);
					if (!dataspaceId.IsValid)
					{
						throw new InvalidOperationException($"Failed to create dataspace for dataset name={name}.");
					}

					//Create the dataset.
					Id storageType = typeMapping.StorageType;
					Id datasetId = H5D.create(containerId, nameBytes, storageType, dataspaceId);
					if (!datasetId.IsValid)
					{
						throw new InvalidOperationException($"Failed to create dataset; name={name}");
					}

					// Write the dataset. 
					Id memType = typeMapping.MemoryType;
					using (var pinnedObj = new PinnedGCHandle(values))
					{
						// GCHandle hnd = GCHandle.Alloc(values, GCHandleType.Pinned);
						// try
						// {
						Result res = H5D.write(datasetId, memType, H5S.ALL, H5S.ALL, H5P.DEFAULT,
							pinnedObj.AddressPtr);
						//}
						//finally
						//{
						//	hnd.Free();
						//}
						if (res.IsOk)
						{
							return datasetId;
						}
					}

					// exception
					H5D.close(datasetId);
					throw new InvalidOperationException($"Failed to write dataset; name={name}");
				}
				finally
				{
					// release dataspace.
					if (dataspaceId.IsValid)
					{
						H5S.close(dataspaceId);
					}
				}
			}

		}

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
		public Group Owner { get; }
		internal DataSetCollection(Group owner)
		{
			Owner = owner;
		}

		// bummer: https://stackoverflow.com/questions/494827/why-it-is-not-possible-to-define-generic-indexers-in-net
		public DataSet this[string pathOrName]
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

		//// Q: <TElem> or <TValue>
		// public ChunkedDataSet CreateChunked<T>() { throw new NotImplementedException(); }
		//
		// public StreamDataSet CreateStreamed<T>() => throw new NotImplementedException();
		// 
		// also need the ability to store data first and name and link it later? (anonymous dataset creation) 

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
