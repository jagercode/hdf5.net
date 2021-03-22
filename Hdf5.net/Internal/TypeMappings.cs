using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HDF.PInvoke;


namespace Hdf5.Internal
{
	using Hdf5Id = Id;

	internal class TypeMapping
	{
		// IBM compatible machines are little endian.
		// Stored endianness is resolved by the hdf5 library when reading the file.
		// Note: Whilst the majority of computers have LITTLE endian memory layout
		// network connection data streams / packets are supposed to be BIG endian.
		//
		// https://support.hdfgroup.org/HDF5/doc1.8/RM/PredefDTypes.html
		//
		// integers
		// signed:        unsigned:       bitfield:     
		// H5T_STD_I8BE   H5T_STD_U8BE    H5T_STD_B8BE
		// H5T_STD_I8LE   H5T_STD_U8LE    H5T_STD_B8LE
		// H5T_STD_I16BE  H5T_STD_U16BE   H5T_STD_B16BE
		// H5T_STD_I16LE  H5T_STD_U16LE   H5T_STD_B16LE
		// H5T_STD_I32BE  H5T_STD_U32BE   H5T_STD_B32BE
		// H5T_STD_I32LE  H5T_STD_U32LE   H5T_STD_B32LE
		// H5T_STD_I64BE  H5T_STD_U64BE   H5T_STD_B64BE
		// H5T_STD_I64LE  H5T_STD_U64LE   H5T_STD_B64LE
		// 
		// floats
		// H5T_IEEE_F32BE
		// H5T_IEEE_F32LE
		// H5T_IEEE_F64BE
		// H5T_IEEE_F64LE
		public static IDictionary<Type, TypeMapping> Supported { get; } = new Dictionary<Type, TypeMapping>()
				{
                // ReSharper disable BuiltInTypeReferenceStyle
                {typeof(double), new TypeMapping( H5T.NATIVE_DOUBLE, H5T.IEEE_F64LE )},
					 {typeof(float), new TypeMapping( H5T.NATIVE_FLOAT, H5T.IEEE_F32LE )},
					 {typeof(long), new TypeMapping(H5T.NATIVE_INT64, H5T.STD_I64LE) },
					 {typeof(int), new TypeMapping(H5T.NATIVE_INT32, H5T.STD_I32LE) },
                // ReSharper restore BuiltInTypeReferenceStyle
            };

		internal static bool Hdf5TypeEquals(Hdf5Id storageType, Hdf5Id h5Type)
		{
			return H5T.equal(storageType, h5Type) > 0;
		}

		internal static bool Hdf5TypeEquals(Hdf5Id storageType, Hdf5Id type1, Hdf5Id type2)
		{
			return Hdf5TypeEquals(storageType, type1) || Hdf5TypeEquals(storageType, type2);
		}

		/// <summary>
		/// Gets the type of the memory types from storage.
		/// </summary>
		/// <param name="storageType">Type of the storage.</param>
		/// <param name="type">The type.</param>
		/// <param name="h5MemType">Type of the h5 memory.</param>
		/// <exception cref="NotImplementedException">
		/// NATIVE_FLOAT
		/// or
		/// NATIVE_DOUBLE
		/// or
		/// NATIVE_INT64
		/// or
		/// NATIVE_INT32
		/// or
		/// storage type {storageType}
		/// </exception>
		public static void GetMemTypesFromStorageType(Hdf5Id storageType, out Type type, out Hdf5Id h5MemType)
		{
			// H5T.equal(type, H5T.STD_I16LE);
			if (Hdf5TypeEquals(storageType, H5T.IEEE_F32LE, H5T.IEEE_F32BE))
			{
				type = typeof(float);
				h5MemType = H5T.NATIVE_FLOAT;
				return;
			}
			if (Hdf5TypeEquals(storageType, H5T.IEEE_F64LE, H5T.IEEE_F64BE))
			{
				type = typeof(double);
				h5MemType = H5T.NATIVE_DOUBLE;
				return;
			}
			if (Hdf5TypeEquals(storageType, H5T.STD_I32LE, H5T.STD_I32BE))
			{
				type = typeof(int);
				h5MemType = H5T.NATIVE_INT32;
				return;
			}
			if (Hdf5TypeEquals(storageType, H5T.STD_I64LE, H5T.STD_I64BE))
			{
				type = typeof(long);
				h5MemType = H5T.NATIVE_INT64;
				return;
			}

			if (Hdf5TypeEquals(storageType, H5T.NATIVE_FLOAT))
			{
				throw new NotImplementedException(nameof(H5T.NATIVE_FLOAT));
			}

			if (Hdf5TypeEquals(storageType, H5T.NATIVE_DOUBLE))
			{
				throw new NotImplementedException(nameof(H5T.NATIVE_DOUBLE));
			}

			if (Hdf5TypeEquals(storageType, H5T.NATIVE_INT64))
			{
				throw new NotImplementedException(nameof(H5T.NATIVE_INT64));
			}

			if (Hdf5TypeEquals(storageType, H5T.NATIVE_INT32))
			{
				throw new NotImplementedException(nameof(H5T.NATIVE_INT32));
			}

			throw new NotImplementedException($"storage type {storageType}");
		}

		public TypeMapping(Hdf5Id memoryType, Hdf5Id storageType)
		{
			MemoryType = memoryType;
			StorageType = storageType;
		}

		public Hdf5Id MemoryType
		{
			get;
		}

		public Hdf5Id StorageType { get; }

	}
}
