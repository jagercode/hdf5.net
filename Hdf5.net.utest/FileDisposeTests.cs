using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdf5.utest
{
	[TestFixture]
	internal class FileDisposeTests : FileTestsBase
	{
		public File DisposedFile { get; set; }

		public DataSet DatasetFromDisposedFile { get; set; }

		public Group GroupFromDisposedFile { get; set; }

		public DataSetCollection DatasetCollectioniFromDisposedFile { get; set; }

		public GroupCollection GroupCollectionFromDisposedFile { get; set; }

		[OneTimeTearDown]
		public void OverallCleanup()
		{ }

		[OneTimeSetUp]
		public void OverallSetup()
		{

		}
	}
}
