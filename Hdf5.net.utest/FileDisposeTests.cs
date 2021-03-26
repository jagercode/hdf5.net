﻿using NUnit.Framework;
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

		public DataSetCollection DatasetCollectioniFromDisposedFile { get; set; }

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
	}
}
