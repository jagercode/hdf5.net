using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hdf5.utest
{
	[TestFixture]
	internal class FileDisposeTests : FileTestsBase
	{
		public File DisposedFile { get; set; }

		public DataSet DatasetFromDisposedFile { get; set; }

		public Group GroupFromDisposedFile { get; set; }

		public DataSetList DatasetCollectioniFromDisposedFile { get; set; }

		public GroupCollection GroupCollectionFromDisposedFile { get; set; }

		//[OneTimeTearDown]
		//public void OverallCleanup() { }

		//[OneTimeSetUp]
		//public void OverallSetup() { }

		[Test]
		public void Open_file_close_file()
		{
			var fi = new FileInfo(Site.CopyResourceToOut("file.h5"));
			using (var file = new File(fi.FullName))
			{
				Assert.True(fi.Exists, "file should exist.");
				//  test it is locked.
				Assert.Throws<IOException>(() => fi.Delete());
			}
			// test it is freed. 
			Assert.DoesNotThrow(() => fi.Delete());
		}

		[Test]
		public void Create_file_close_file()
		{
			var fi = new FileInfo(Site.ReserveOut("file.h5"));
			using (var file = new File(fi.FullName))
			{
				Assert.True(fi.Exists, "file should exist.");
				//  test it is locked.
				Assert.Throws<IOException>(() => fi.Delete());
			}
			// test it is freed. 
			Assert.DoesNotThrow(() => fi.Delete());
		}

		[Test]
		public void Cannot_open_file_twice_for_write()
		{

			var path = Site.CopyResourceToOut("file.h5");
			
			using (var file = new File(path))
			{
				//  test it is locked.
				Assert.DoesNotThrow(() => { var file2 = new File(path);
					file2.Dispose();
				}, "Open twice");
			}
			// test it is freed. 
			Assert.DoesNotThrow(() => System.IO.File.Delete(path),"Delete file");
			Assert.Inconclusive("Why can we open the same file twice?");
		}

		[Test]
		public void Dangling_objects_throw_ObjectDisposed_if_accessed_after_disposal()
		{
			Assert.Inconclusive("Todo: implement when groups, datasets and attributes can be obtained from the file.");
		}

	}
}
