﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using jagercode.Testing;

namespace jagercode.Hdf5.utest.Datasets
{
	[TestFixture]	
	public class DatasetAddDouble : TestBase, IAddShapesDataset<double>
	{
		private readonly Site _site = Site.AtTypeAssemblyPath(typeof(DatasetAddDouble));

		private File _file;

		[OneTimeSetUp]
		public void PrepareAllTests()
		{
			string fileName = $"{this.GetType().Name}.h5";
			string path = _site.ReserveOut(fileName);
			_file = new File(path);
		}

		[OneTimeTearDown]
		public void CleanUpAllTests()
		{
			// kill that file? If no post tests actions are required, the _file could be made Lazy property. 
		}

		[Test]
		public void Add_1d()
		{
			var arr = new[] { 1e6, 2e6, 3e6 };
			var name = CurrentMethodName();
			_file.DataSets.Add(name, arr);
			var ds = _file.DataSets.Create(name, arr);
			// todo: read back? 
			throw new NotImplementedException();
		}

		[Test]
		public void Add_2d()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Add_3d()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Add_8d()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Add_scalar()
		{

			throw new NotImplementedException();
		}
	}
}