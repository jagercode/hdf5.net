//
using NUnit.Framework;
using System;
//

namespace Hdf5.utest.Datasets
{
	[TestFixture]
	internal abstract class DatasetAddGetValueTestBase<T> : FileTestsBase, IMultiShapeTestCasesOf<T>
	{
		private string _filePath;

		[OneTimeSetUp]
		public void PrepareAllTests()
		{
			_filePath = $"{GetType().Name}.h5";
			_filePath = Site.ReserveOut(_filePath);
		}

		[OneTimeTearDown]
		public void CleanUpAllTests()
		{
			// kill that file? If no post tests actions are required, the _file could be made Lazy property. 
		}

		[Test]
		public void Test_1d_value()
		{
			T[] expected = Get1DArrayValue();
			string dsName = $"{typeof(T).Name}_{GetThisMethodName()}";

			using (File f = new File(_filePath))
			{
				f.DataSets.Add(dsName, expected);
			}

			T[] actual;
			using (File f = new File(_filePath))
			{
				DataSet ds = f.DataSets[dsName];
				actual = ds.GetValue<T[]>();
			}
			for (int i = 0; i < expected.Length; i++)
			{
				T exp = expected[i];
				T act = actual[i];
				Assert.AreEqual(exp, act, $"{dsName} index {i}: Expected {exp} == Actual {act}");
			}
		}

		internal abstract T[] Get1DArrayValue();

		//// [Test]
		//public void Test_2d_value()
		//{
		//	T[] arr1d = Get3DArrayValue();
		//	var dsName = $"{typeof(T).Name}_1D_array";
		//	throw new NotImplementedException();
		//}

		// [Test]
		public void Test_3d_value()
		{
			T[,,] arr3d = Get3DArrayValue();
			string dsName = $"{typeof(T).Name}_{GetThisMethodName()}";
			throw new NotImplementedException();
		}

		internal abstract T[,,] Get3DArrayValue();

		// [Test]
		public void Test_8d_value()
		{
			T[,,,,,,,] arr8d = Get8DArrayValue();
			string dsName = $"{typeof(T).Name}_{GetThisMethodName()}";
			throw new NotImplementedException();
		}

		internal abstract T[,,,,,,,] Get8DArrayValue();

		[Test]
		public void Test_scalar_value()
		{
			T expected = GetScalarValue();
			string dsName = $"{typeof(T).Name}_{GetThisMethodName()}";

			using (File f = new File(_filePath))
			{
				f.DataSets.Add(dsName, expected);

			}

			T actual = default(T);
			using (File f = new File(_filePath))
			{
				DataSet ds = f.DataSets[dsName];
				actual = ds.GetValue<T>();
			}

			Assert.AreEqual(expected, actual, dsName);

		}

		internal abstract T GetScalarValue();
	}
}
