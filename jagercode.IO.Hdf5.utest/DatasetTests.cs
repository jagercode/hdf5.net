using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace jagercode.IO.Hdf5.utest
{
	using jagercode.IO.Hdf5;
	using jagercode.Testing.IO;

	[TestClass]
	public class DatasetTests
	{
		private Site _site = Site.AtTypeAssemblyPath(typeof(DatasetTests));

		[TestMethod]
		public void Add_double_succeeds()
		{
			var f = new File("");
			f.DataSets["double"].Set(2.33d);
			f.Dispose();
		}

		[TestMethod]
		public void Read_double_succeeds()
		{
			var f = new File("");
			f.DataSets["double"].Get(out double d);
			f.Dispose();
		}

		[TestMethod]
		public void Read_double_using_linq()
		{
			var f = new File("");
			//
			f.DataSets.Where(ds => ds.Name == "double").First().Get(out double d);

			// move to Node tests:
			// d = f.Select(n => n.Name == "double").First();

			f.Dispose();
		}

		[TestMethod]
		public void Read_double_using_AsDictionary()
		{
			var f = new File("");
			var dict = f.DataSets.AsDictionary();
			double d = (double)dict["double"];
			
			// missing the attributes here. 

		}

	}
}
