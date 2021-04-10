using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io = System.IO;
using System.Diagnostics;
//
using HDF.PInvoke;

namespace Hdf5
{
	using HdfBridge;

	public class File : Group, IDisposable
	{

	   /// <summary>
		/// Opens file if exists; Creates otherwise
		/// </summary>
		/// <param name="path"></param>
		public File(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException($"Is null or whitespace", nameof(path));

			Path = path;
			// set always existing rootgroup here.

			if (io.File.Exists(path))
			{
				// open
				_id = H5F.open(path, H5F.ACC_RDWR);
				if (!_id.IsValid) throw new InvalidOperationException($"Unable to open file '{path}' for read/write.");
				return;
			}

			// create from scratch
			_id = H5F.create(path, H5F.ACC_EXCL);
			if (!_id.IsValid)
			{
				// check some trivialities
				var dir = io.Path.GetDirectoryName(path);
				if (null == dir)
					throw new io.DirectoryNotFoundException($"No folder specified; path='{path}'");

				if (!io.Directory.Exists(dir))
					throw new io.DirectoryNotFoundException($"Folder does not exist; path='{path}'");

				throw new InvalidOperationException($"Unable to create file at '{path}'.");
			}
		}

		// <decision> If keeping open of file until last referenced object id has been closed
		// or object garbage collected then implement methods below.
		//public void SoftClose() => throw new NotImplementedException();

		//public void Reopen() => throw new NotImplementedException();
		// </decision>

		/// <summary>
		/// Hard closing file, including all open objects. 
		/// Client code still using them should expect ObjectDisposedException upon
		/// accessing objects from a closed file
		/// </summary>
		internal void Close()
		{
			if (_id.IsValid)
			{
				// Close keeps the file open while there are still open file objects.
				// --> Force closing of all remainging open objects.
				CloseOpenObjects();

				// close flushes implicitly; no need to H5F.flush(Id, H5F.scope_t.LOCAL);
				var res = (Result)H5F.close(_id);
				
				Debug.Assert(res.IsOk, $"H5F.close(Id) must succeed on Dispose; File: '{Path}'");
				//if (res.HasFailed)
				//{
				//	// todo: throw or log error. ($"Hdf5File.Dispose, H5F.close FAILED; File: '{Path}'.");
				//	// Notifications.Error("Hdf5File.Dispose, H5F.close:  FAILED", "file: " + FilePath);
				//}
				_id = Id.Invalid;

				
			}

		}

		internal void CloseOpenObjects()
		{
			// close all open objects 
			var allButFile = H5F.OBJ_ATTR | H5F.OBJ_DATASET | H5F.OBJ_DATATYPE | H5F.OBJ_GROUP;
			IntPtr countPtr = H5F.get_obj_count(_id, allButFile);
			int objCount = countPtr.ToInt32();

			var objIds = Id.CreateLowLevelArray(objCount);

			using (var gch = new PinnedGCHandle(objIds))
			{
				H5F.get_obj_ids(_id,allButFile, countPtr, gch.AddressPtr);

				// Note, the H5F.get_obj_ids(...) unit test did it like so:
				// IntPtr buf = H5.allocate_memory(new IntPtr(objCount * _id.H5AllocSize), 0);
				// H5F.get_obj_ids(_id, H5F.OBJ_ALL, countPtr, buf);
			   // H5.free_memory(buf)
				// I think that's not right. The idea is to keep the values after calling the method
				// so it must be C#'s memory and not H5's.
			}

			foreach (var objId in objIds)
			{
				Result res =  H5O.close(objId);
				Debug.Assert(res.IsOk, $"close open object id {objId}");
			}
		}

		public string Path { get; }

		

		#region IDisposable Support

		// fields managed by Dispose
		private Id _id;
		internal override Id Id { get => _id; set => _id = value; }

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// dispose managed state (managed objects).
				}

				// free unmanaged resources (unmanaged objects) and override a finalizer below.
				// set large fields to null.
				Close();

				disposedValue = true;
			}
		}

		// override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~File() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		/// <summary>
		/// Close and flush all objects and release file lock. 
		/// </summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
