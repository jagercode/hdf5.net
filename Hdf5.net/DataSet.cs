using HDF.PInvoke;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hdf5
{
	using HdfBridge;

	// Q: Make it abstract? Unify dataset and attribute approach for consistency.
	// Q: start object based instead of generic? What's convenient? Object type might be more convenient for iterating and so, but feels outdated. 
	// So many decisions to make.... -_-
	public class DataSet : INode, INdEntry, IMultiAccessable
	{
		public Group Group { get; }


		/// <summary>
		/// Creates an instance for a dataset.
		/// </summary>
		/// <param name="location"></param>
		/// <param name="name"></param>
		internal DataSet(Group location, string name)
		{
			Group = location ?? throw new ArgumentNullException(nameof(location));
			if (!Path.IsValidName(Name = name))
			{
				throw new ArgumentException($"Invalid name: '{name}'", nameof(name));
			}

			Id = Id.Invalid;

			// todo: this approach delegates the responsibility to create / add a dataset to a group to the DataSetList.
			// Wouldn't it be better to let this constructor do the work and just instantiate a dataset that vanishes
			// again in the DataSetList.Add() method? 
		}

		#region INode implementation
		public string Name
		{
			get; set;
			// set should rename if called from outside. It should be settable for the first time.
			// Moving the add-to-group responsibility to the constructor also removes the need for that.
		}

		public AttributeCollection Attributes { get; }

		public string Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		#endregion


		#region INdEntry implementation

		// approach 1: DataSet IS an INdArray with attributes and a name
		public ulong[] Shape => throw new NotImplementedException();
		public bool IsScalar => Shape.Length == 0;
		public Type ElementType => throw new NotImplementedException();

		public T GetValue<T>()
		{
			// Don't want to know here if I can open the group or not. 

			// using (SafeIdHandle openGroupHandle = Group.UseOpen())
			using (OpenStateHandle openDatasetHnd = UseOpen())
			{
				ReadValue(Id, out T values);
				return values;
			}

		}

		public void SetValue<T>(T value) { throw new NotImplementedException(); }

		public object ValueAsObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		#endregion
		// approach 2: DataSet HAS a INdArray like it has a Name and Attributes
		//public INdArray Value { get; set; }


		#region HDF.PInvoke wrappers

		internal Id Id { get; private set; }

		private int AccessorsCount = 0;

		// Id IMultiAccessable.Id => throw new NotImplementedException();

		internal OpenStateHandle UseOpen()
		{
			return new OpenStateHandle(this, Group.UseOpen());
		}

		/// <summary>
		/// Reads the dataset1 d.
		/// </summary>
		/// <typeparam name="TElem">The type of the elem.</typeparam>
		/// <param name="locationId">The file identifier.</param>
		/// <param name="path">The h5 path.</param>
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
		internal static void ReadValue<T>(Id datasetId, out T value)
		{
			// can easily be scaled up to N-D and scalar. 
			// might not work for TElem = string. (strings are awkward in hdf5)

			using (SafeIdHandle spaceHnd = DataSpace.Open(datasetId))
			{
				if (!spaceHnd.Id.IsValid)
				{
					throw new InvalidOperationException($"Failed to open data space");
				}

				// TODO
				//var bufferHandle = DataSpace.CreateBuffer();
				//using (bufferHandle)
				//{
				//	H5D.read(,,,,, bufferHandle.BufferPtf);
				//	value = (T)bufferHandle.Value;
				//}

				// verify data types and size. 
				Id storageTypeId = H5D.get_type(datasetId);
				TypeMapping.GetMemTypesFromStorageType(storageTypeId, out Type elementType, out Id memTypeId);

				Type requesstedType = typeof(T);
				Type requestedElementType;
				int requestedRank;
				if (requesstedType.IsArray)
				{
					requestedElementType = requesstedType.GetElementType();
					requestedRank = requesstedType.GetArrayRank();
				}
				else
				{
					requestedElementType = requesstedType;
					requestedRank = 0;
				}
				if (elementType != requestedElementType)
				{
					throw new ArgumentException($"Dataset is of type'{elementType}' and not '{requestedElementType}'");
				}

				int rank = H5S.get_simple_extent_ndims(spaceHnd.Id);

				if (rank != requestedRank)
				{
					throw new ArgumentException($"Dataset has rank {rank} and not {requestedRank}");
				}

				// rank == 0: scalar, not an array.

				ulong[] dims = new ulong[rank];

				H5S.get_simple_extent_dims(spaceHnd.Id, dims, null);

				// read array (shape may be inferred w/ H5S.get_simple_extent_ndims)
				object buffer;
				if (requestedRank == 0)
				{
					// scalar
					buffer = default(T);
				}
				else
				{
					buffer = CreateArray<T>(elementType, dims);
				}
				// Array arr = Array.CreateInstance(typeof(TElem), (int) arrayLength);
				using (PinnedGCHandle<object> gch = new PinnedGCHandle<object>(buffer))
				{
					H5D.read(datasetId, /*storageTypeId*/ memTypeId, H5S.ALL, H5S.ALL, H5P.DEFAULT,
						gch.AddressPtr);
				}
				value = (T)buffer;
			}
		}

		private static T CreateArray<T>(Type elementType, ulong[] dims)
		{
			long[] shape = dims.Select(Convert.ToInt64).ToArray();
			object array = Array.CreateInstance(elementType, shape);
			return (T)array;
		}

		//internal static void GetValueProperties<TValue>(TValue value, out ulong[] shape, out TypeMapping typeMapping)
		//{
		//	//if (!typeof(TValue).IsArray)
		//	//{
		//	//	typeMapping = TypeMapping.Supported[typeof(TValue)];
		//	//	throw new ArgumentException($"argument type is not array", nameof(array));
		//	//}

		//	Type elementType;
		//	Array a = value as Array;

		//	// TODO: Compound Type will go bad.
		//	// TODO: how to handle 

		//	if (null == a)
		//	{
		//		// scalar
		//		shape = new ulong[0];
		//		elementType = typeof(TValue);
		//	}
		//	else
		//	{
		//		// array
		//		shape = new ulong[a.Rank];
		//		for (int d = 0; d < shape.Length; d++)
		//		{
		//			shape[d] = (ulong)a.GetLength(d);
		//		}
		//		elementType = typeof(TValue).GetElementType() ??
		//						  throw new ArgumentException("Unknown element type", nameof(value));
		//	}

		//	try
		//	{
		//		typeMapping = TypeMapping.Supported[elementType];
		//	}
		//	catch (KeyNotFoundException)
		//	{
		//		throw new NotImplementedException($"array of type {elementType}");
		//	}

		//}

		internal static void AddSimple<T>(Id groupId, string name, T value)
		{
			// AddSimple = CreateSimple + close Id.

			if (Path.Exists(groupId, name))
			{
				throw new ArgumentException($"Group already contains object named '{name}'", nameof(name)); // todo: argument exception
			}

			byte[] nameBytes = name.ToUtf8Bytes();
			Debug.WriteLine($"Dataset '{name}' utf-8 bytes: [{string.Join(", ", nameBytes.Select(b => b.ToString()))}]");

			try
			{
				using (SafeIdHandle openSpaceHandle = DataSpace.CreateFor(value))
				{
					TypeMapping typeMapping = TypeMapping.GetFor(value); // TypeMapping.Supported[ElementType];

					//Create the dataset.
					Id storageType = typeMapping.StorageType;
					using (SafeIdHandle datasetId = new SafeIdHandle(H5D.create(groupId, nameBytes, storageType, openSpaceHandle.Id), H5D.close))
					{
						if (!datasetId.IsValid)
						{
							throw new InvalidOperationException($"Failed to create dataset; name={name}");
						}

						// Write the dataset. 
						Id memType = typeMapping.MemoryType;
						using (PinnedGCHandle<T> pinnedObj = new PinnedGCHandle<T>(value))
						{
							Result res = H5D.write(datasetId.Id, memType, H5S.ALL, H5S.ALL, H5P.DEFAULT,
								pinnedObj.AddressPtr);
							if (res.HasFailed)
							{
								throw new InvalidOperationException("Failed to write values to created dataset.");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to create dataset '{name}' at {{Parent.Path}}", ex);
			}
		}



		void IMultiAccessable.ClaimAccess(OpenStateHandle handle)
		{
			// now, how to ensure that the group is also maintained open until access is dropped? 
			// As long as the Dataset is open the group should also be open.
			// Support nesting? --> factory method on class, release method on interface.
			// this is getting too complex. But maintaining a group open untill the last dataset is disposed or disposing all 
			// datasets on disposal of the group is also complex. 
			// Always use abs path and the always open file Id? 

			if (AccessorsCount == 0)
			{
				Id locationId = Group.Id;
				string name = Name;
				Id = H5D.open(locationId, name, H5P.DEFAULT);
				Debug.Assert(Id.IsValid, $"Failed to open Dataset '{Name}'.");
			}
			AccessorsCount++;
		}

		void IMultiAccessable.DropAccess(OpenStateHandle hanedle)
		{
			AccessorsCount--;
			if (AccessorsCount == 0)
			{
				H5D.close(Id);
				Id = Id.Invalid;
			}
		}






		//internal static Id CreateSimple(Id groupId, string name, ulong[] shape, TypeMapping typeMapping, object values)
		//{
		//	if (Path.Exists(groupId, name))
		//	{
		//		throw new ArgumentException($"Group already contains object named '{name}'", nameof(name)); // todo: argument exception
		//	}

		//	byte[] nameBytes = name.ToUtf8Bytes();

		//	Debug.WriteLine($"Dataset '{name}' utf-8 bytes: [{string.Join(", ", nameBytes.Select(b => b.ToString()))}]");

		//	Id dataspaceId = Id.Invalid;
		//	try
		//	{
		//		dataspaceId = H5S.create_simple(shape.Length, shape, null);
		//		if (!dataspaceId.IsValid)
		//		{
		//			throw new InvalidOperationException($"Failed to create dataspace for dataset name={name}.");
		//		}

		//		//Create the dataset.
		//		Id storageType = typeMapping.StorageType;
		//		Id datasetId = H5D.create(groupId, nameBytes, storageType, dataspaceId);
		//		if (!datasetId.IsValid)
		//		{
		//			throw new InvalidOperationException($"Failed to create dataset; name={name}");
		//		}

		//		// Write the dataset. 
		//		Id memType = typeMapping.MemoryType;
		//		using (PinnedGCHandle pinnedObj = new PinnedGCHandle(values))
		//		{
		//			// GCHandle hnd = GCHandle.Alloc(values, GCHandleType.Pinned);
		//			// try
		//			// {
		//			Result res = H5D.write(datasetId, memType, H5S.ALL, H5S.ALL, H5P.DEFAULT,
		//				pinnedObj.AddressPtr);
		//			//}
		//			//finally
		//			//{
		//			//	hnd.Free();
		//			//}
		//			if (res.IsOk)
		//			{
		//				return datasetId;
		//			}
		//		}

		//		// exception
		//		H5D.close(datasetId);
		//		throw new InvalidOperationException($"Failed to write dataset; name={name}");
		//	}
		//	finally
		//	{
		//		// release dataspace.
		//		if (dataspaceId.IsValid)
		//		{
		//			H5S.close(dataspaceId);
		//		}
		//	}
		//}

		#endregion

	}


	public class DataSet<TValue> : DataSet
	{
		// nameless dataset; note that name must be set before adding to a group. 
		// public DataSet(TValue value) { }

		internal DataSet(Group location, string name, TValue value) : base(location, name)
		{ }

		public TValue Value { get => base.GetValue<TValue>(); set => base.SetValue<TValue>(value); }

		public static implicit operator TValue(DataSet<TValue> ds)
		{
			return ds.Value;
		}
	}


	public sealed class DataSetList : IEnumerable<DataSet>
	{
		private Group Owner { get; }
		internal DataSetList(Group owner)
		{
			Owner = owner ?? throw new ArgumentNullException(nameof(owner));
		}

		// bummer: https://stackoverflow.com/questions/494827/why-it-is-not-possible-to-define-generic-indexers-in-net
		public DataSet this[string name]
		{
			get
			{
				// https://support.hdfgroup.org/HDF5/doc/_topic/loc_id+name_obj.htm
				// https://support.hdfgroup.org/HDF5/doc1.6/UG/09_Groups.html
				if (!Path.IsNameOnly(name))
				{
					throw new ArgumentException($"name '{name}' is a (partial) path. Please provide a name only.", nameof(name));
				}

				using (Owner.UseOpen())
				{
					if (Path.Exists(Owner.Id, name))
					{
						return new DataSet(Owner, name);
					}
					throw new InvalidOperationException($" DataSet '{name}' not found.");
				}
			}
			// set => throw new NotImplementedException("Scheduled.");
		}


		public void Add<T>(string name, T value)
		{
			// Don't want to know here if I can open the group or not. 
			using (Owner.UseOpen())
			{
				try
				{
					//DataSet.HDF.GetValueProperties(value, out ulong[] shape, out TypeMapping typeMapping);
					//var dsId = DataSet.HDF.CreateSimple(ownerId, name, shape, typeMapping, value);
					DataSet.AddSimple(Owner.Id, name, value);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException($"Failed to add dataset {name} at {{Parent.Path}}", ex);
				}
			}
		}



		// >> future work

		// public DataSet Create<T>(string name) { throw new NotImplementedException(); }

		// public IDictionary<string, object> AsDictionary() { return new Easy.DataSetDictionary(this); }

		// public void Add(DataSet dataSet)
		// { }

		// public ChunkedDataSet CreateChunked<T>() { throw new NotImplementedException(); }

		// public StreamDataSet CreateStreamed<T>() => throw new NotImplementedException();

		// public Table CreateTable<T>() => throw new NotImplementedException();

		// public PacketTable CreatePacketTable<T>() => throw new NotImplementedException();

		// 
		// also need the ability to store data first and name and link it later? (anonymous dataset creation) 
		// <<

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
