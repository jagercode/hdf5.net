using HDF.PInvoke;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace jagercode.Hdf5
{
	using Internal;

	public abstract class Attribute : INdEntry
	{
		public string Name { get; set; }

		public Type ElementType { get; }

		public T Get<T>()
		{
			throw new NotImplementedException();
		}

		public void Set<T>(T value)
		{
			throw new NotImplementedException();
		}

		// public INdArray Value { get; set; }

		public bool IsScalar => throw new NotImplementedException();

		public ulong[] Shape => throw new NotImplementedException();

		public Type Type => throw new NotImplementedException();

		public object ValueAsObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		internal static class HDF
		{

			/// <summary>
			/// Expects an already OPEN hdf5 node object. Returns the requested value.  
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="nodeId"></param>
			/// <param name="attrName"></param>
			/// <returns></returns>
			internal static T Read<T>(Id nodeId, string attrName)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Gets the specified data from the attribute at the specified location.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="fileId">The file path.</param>
			/// <param name="h5Path">The HDF5 path.</param>
			/// <param name="attrName">Name of the attribute.</param>
			/// <returns></returns>
			/// <exception cref="System.NotImplementedException"></exception>
			internal static T Read<T>(Id fileId, string h5Path, string attrName)
			{
				if (!fileId.IsValid)
				{
					throw new ArgumentException("File Id is not valid", nameof(fileId));
				}

				Id id = H5O.open(fileId, h5Path, H5P.DEFAULT);
				if (!id.IsValid)
				{
					throw new ArgumentException($"Failed to open dataset or group at {h5Path}");
				}

				// Indirectly tested via Hdf5Editor and public read(..., out[] double) and read(..., out float[])
				using (SafeIdHandle nodeHnd = new SafeIdHandle(id, H5O.close)) // can be dataset or group; irrelevant for attributes.
				{
					id = H5A.open(nodeHnd.Id, attrName);
					if (!id.IsValid)
					{
						throw new ArgumentException($"Failed to open attribute '{attrName}' at '{h5Path}'", nameof(attrName));
					}

					using (SafeIdHandle attrHnd = new SafeIdHandle(id, H5A.close))
					{
						Type returnType = typeof(T);
						Type returnElementType = returnType.IsArray ? returnType.GetElementType() : returnType;
						int returnRank = returnType.IsArray ? returnType.GetArrayRank() : 0;

						// verify data types and size.
						long storageTypeId = H5A.get_type(attrHnd.Id);

						// todo: get it workses for ascii and utf8 strings.
						// See: https://stackoverflow.com/questions/51978853/how-to-read-hdf5-variable-length-string-attributes-in-c-sharp-net

						// (the following code is quite generic: Could be extracted to Hdf5DataSpace.Read() if attributes also have data spaces.)
						Type storedType;
						Id memType;
						try
						{
							TypeMapping.GetMemTypesFromStorageType(storageTypeId, out storedType, out memType);
						}
						catch (NotImplementedException notImplemented)
						{
							throw new InvalidOperationException($"MemType not found for attribute '{attrName}' of element type {{{returnElementType}}} at '{h5Path}' ", notImplemented);
						}

						if (returnElementType != storedType)
						{
							throw new InvalidOperationException(
								$"Array Element Type {{{returnElementType}}} does not match stored data type {{{storedType}}}");
						}

						ulong[] h5Dims;
						id = H5A.get_space(attrHnd.Id);
						if (!id.IsValid)
						{
							throw new InvalidOperationException(
								$"Failed to open data space of attribute '{attrName}' at '{h5Path}'");
						}

						// <extract method> space.read<T>()?

						using (SafeIdHandle spaceHnd = new SafeIdHandle(id, H5S.close))
						{

							int storedRank = H5S.get_simple_extent_ndims(spaceHnd.Id); // H5T.get_array_ndims(attrHnd.Id);
							if (storedRank != returnRank)
							{
								throw new InvalidOperationException(
									$"Expected {returnRank}D value but got {storedRank}D for attribute {attrName} at '{h5Path}'");
							}

							// type and rank match. Now create a scalar or array dynamically.

							h5Dims = new ulong[storedRank];
							H5S.get_simple_extent_dims(spaceHnd.Id, h5Dims, null);
							//H5T.get_array_dims(attrHnd.Id, h5Dims); //H5S.get_simple_extent_dims(spaceHnd.Id, h5Dims, null);
						}

						// ulong arrayLength = 1;
						// create a scalar or array of the found size.
						long[] arrayDims;
						try
						{
							arrayDims = h5Dims.Select(Convert.ToInt64).ToArray();
						}
						catch (OverflowException)
						{
							// such large arrays aren't expected on attributes anyway... 
							throw new NotSupportedException(
								$"One or more dimensions of the data exceeds Int64.MaxValue and cannot be used for dynamic array creation. Shape: {Internal.Shape.ToString(h5Dims)}");
						}

						object toPin = returnRank == 0
							? (object)default(T)
							: Array.CreateInstance(returnElementType, arrayDims);

						// read scalar or array (shape may be inferred w/ H5S.get_simple_extent_ndims)
						using (PinnedGCHandle gch = new PinnedGCHandle(toPin))
						{
							H5A.read(attrHnd.Id, memType, gch.AddressPtr);
						}

						return (T)toPin;
						// </extract method> 
					}
				}
			}

		}
	}

	public sealed class Attribute<T> : Attribute, INdEntry<T>
	{
		public T Value { get; set; }
	}

	public sealed class AttributeCollection : IEnumerable<Attribute>
	{
		public Attribute this[string name] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
