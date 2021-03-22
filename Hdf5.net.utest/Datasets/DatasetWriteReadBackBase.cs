using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using NUnit.Framework;
//

namespace Hdf5.utest.Datasets
{
	[TestFixture]
	internal abstract class DatasetWriteReadBackTestBase<T> : FileTestsBase, IMultiShapeTestCasesOf<T>
	{
		private string _filePath;

		[OneTimeSetUp]
		public void PrepareAllTests()
		{
			_filePath = $"{this.GetType().Name}.h5";
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
			T[] arr1d = Get1DArrayValue();
			var dsName = $"{typeof(T).Name}_{GetThisMethodName()}";

			using (var f = new File(_filePath))
			{
			}
				throw new NotImplementedException();
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
			var dsName = $"{typeof(T).Name}_{GetThisMethodName()}";
			throw new NotImplementedException();
		}

		internal abstract T[,,] Get3DArrayValue();

		// [Test]
		public void Test_8d_value()
		{
			T[,,,,,,,] arr8d = Get8DArrayValue();
			var dsName = $"{typeof(T).Name}_{GetThisMethodName()}";
			throw new NotImplementedException();
		}

		internal abstract T[,,,,,,,] Get8DArrayValue();

		[Test]
		public void Test_scalar_value()
		{
			T expected = GetScalarValue();
			var dsName = $"{typeof(T).Name}_{GetThisMethodName()}";

			using (var f = new File(this._filePath))
			{
				f.DataSets.Add(dsName, expected);

			}

			T actual = default(T);
			using (var f = new File(_filePath))
			{
				var ds = f.DataSets[dsName];
				actual = ds.Get<T>();
			}

			Assert.AreEqual(expected, actual, dsName);

		}

		internal abstract T GetScalarValue();
	}
}
