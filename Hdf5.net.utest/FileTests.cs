using System;
using NUnit.Framework;
using System.Linq;
using System.IO;


namespace Hdf5.utest
{
	[TestFixture]
	internal class FilePathTests : FileTestsBase
	{
		[Test]
		public void Path_matches_opened_file_path()
		{
			var path = Site.CopyResourceToOut("file.h5");
			using (var file = new File(path))
			{
				var actual = file.Path;
				Assert.AreEqual(path, actual);
			}
		}

		[Test]
		public void Path_matches_created_file_path()
		{
			var path = Site.ReserveOut("file2.h5");
			using (var file = new File(path))
			{
				var actual = file.Path;
				Assert.AreEqual(path, actual);
			}
		}
	}
	//	// these tests are now redundant since Close isn't public anymore (Use Dispose to close)
	//	// Note that H5F.close is designed to close automatically when the last open id is closed
	//	// for any object that is still open after closing. 
	//	// my intent is to prevent such dangling pointers. Disposed is final.

	//[TestFixture]
	//internal class FileTests : FileTestsBase
	//{
	//	[OneTimeSetUp]
	//	public void OverallSetup()
	//	{
	//	}


	//	[Test]
	//	public void Open_file_succeeds_if_file_exists()
	//	{
	//		var fi =  new FileInfo(Site.CopyResourceToOut("file.h5"));
	//		var file = new File(fi.FullName);
	//		//  test it is locked.
	//		Assert.DoesNotThrow(() => fi.Delete());
	//		file.Dispose();
	//		// test it is freed. 
	//		Assert.DoesNotThrow(() => fi.Delete());
	//	}

	//	[Test]
	//	public void Open_file_succeeds_if_file_does_not_exist()
	//	{
	//		var fi = Site.ReserveOut("non_existent.h5");
	//		var file = new File(fi);
	//		// todo: file exists
	//		file.Dispose();
	//		// todo: file is freed.
	//	}

	//	[Test]
	//	public void Open_file_throws_if_file_already_open()
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

}
