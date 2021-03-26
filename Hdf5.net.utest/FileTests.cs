using System;
using NUnit.Framework;
using System.Linq;
using System.IO;


namespace Hdf5.utest
{
	[TestFixture]
	internal class FileTests : FileTestsBase
	{
		[OneTimeSetUp]
		public void OverallSetup()
		{
		}

		[Test]
		public void Open_file_succeeds_if_file_exists()
		{
			var fi =  new FileInfo(Site.CopyResourceToOut("file.h5"));
			var file = new File(fi.FullName);
			//  test it is locked.
			Assert.DoesNotThrow(() => fi.Delete());
			file.Close();
			// test it is freed. 
			Assert.DoesNotThrow(() => fi.Delete());
		}

		[Test]
		public void Open_file_succeeds_if_file_does_not_exist()
		{
			var fi = Site.ReserveOut("non_existent.h5");
			var file = new File(fi);
			// todo: file exists
			file.Close();
			// todo: file is freed.
		}

		[Test]
		public void Open_file_throws_if_file_already_open()
		{
			throw new NotImplementedException();
		}


	}
}
