//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using NUnit.Framework;

//using jagercode.Testing;

//namespace jagercode.Hdf5.utest.Datasets
//{
//	[TestFixture]
//	internal class DatasetAddDouble : FileTestsBase, IMultiShapeTestCasesOf<double>
//	{
//		// private readonly Site _site = Site.AtTypeAssemblyPath(typeof(DatasetAddDouble));

//		private File _file;

//		[OneTimeSetUp]
//		public void PrepareAllTests()
//		{
//			string fileName = $"{this.GetType().Name}.h5";
//			string path = Site.ReserveOut(fileName);
//			_file = new File(path);
//		}

//		[OneTimeTearDown]
//		public void CleanUpAllTests()
//		{
//			// kill that file? If no post tests actions are required, the _file could be made Lazy property. 
//		}

//		[Test]
//		public void Test_1d_value()
//		{
//			var arr = new[] { 1e6, 2e6, 3e6 };
//			var name = GetThisMethodName();
//			_file.DataSets.Add(name, arr);
//			var ds = _file.DataSets.Create(name, arr);
//			// todo: read back? 
//			throw new NotImplementedException();
//		}

//		[Test]
//		public void Test_2d_value()
//		{
//			throw new NotImplementedException();
//		}

//		[Test]
//		public void Test_3d_value()
//		{
//			throw new NotImplementedException();
//		}

//		[Test]
//		public void Test_8d_value()
//		{
//			throw new NotImplementedException();
//		}

//		[Test]
//		public void Test_scalar_value()
//		{

//			throw new NotImplementedException();
//		}
//	}
//}
