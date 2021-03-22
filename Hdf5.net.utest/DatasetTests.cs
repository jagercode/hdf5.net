// native usings
using System;
using System.Linq;
// third party usings
using NUnit.Framework;
// solution usings


namespace jagercode.Hdf5.utest
{
	using jagercode.Hdf5;
	using jagercode.Testing;

	[TestFixture]
	internal class DatasetTests : FileTestsBase
	{
		private Site _site = Site.AtTypeAssemblyPath(typeof(DatasetTests));

		[Test]
		public void Add_double_succeeds()
		{
			//var fpath = this
			var f = new File("");
			f.DataSets["double"].Set(2.33d);
			f.Dispose();
		}

		[Test]
		public void Add_double_succeeds_using_AsDictionary()
		{
			var f = new File("");
			var dict = f.DataSets.AsDictionary();
			dict["double"] = 2.33d;
			f.Dispose();
		}

		[Test]
		public void Read_double_succeeds()
		{
			var f = new File("");
			double d = f.DataSets["double"].Get<double>();
			f.Dispose();
		}

		[Test]
		public void Read_double_using_linq()
		{
			var f = new File("");
			//
			double d = f.DataSets.Where(ds => ds.Name == "double").First().Get<double>();

			// move to Node tests:
			// d = f.Select(n => n.Name == "double").First();

			f.Dispose();
		}

		[Test]
		public void Read_double_using_AsDictionary()
		{
			var f = new File("");
			var dict = f.DataSets.AsDictionary();
			double d = (double)dict["double"];
			
			// missing the attributes here. 

		}

	}
}
