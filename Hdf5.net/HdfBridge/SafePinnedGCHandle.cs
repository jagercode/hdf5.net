using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Hdf5.HdfBridge
{
	// <using stuff here gets found first. Prevents naming conflicts>

	/// <summary>
	/// Helper class that wraps <see cref="GCHandle"/> with <see cref="GCHandleType.Pinned"/>
	/// for the provided instance of an object. It is Free-ed upon disposal.
	/// In order to free the object, call Dispose. Inpired by: https://www.codeproject.com/Articles/29534/IDisposable-What-Your-Mother-Never-Told-You-About
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public class PinnedGCHandle : IDisposable
	{
		private GCHandle _gcHandle;

		public PinnedGCHandle(object objectToPin)
		{
			_gcHandle = GCHandle.Alloc(objectToPin, GCHandleType.Pinned);
		}

		public IntPtr AddressPtr => _gcHandle.AddrOfPinnedObject();

		private bool _isDisposed = false;  // never trust a default.
		public void Dispose()
		{
			Dispose(true);
			// Suppress finalization. (because of having a destructor and unmanaged resources)
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (_isDisposed)
			{
				return;
			}

			// not applicable yet: Free managed resources.
			//if (isDisposing)
			//{
			//	//  dispose managed state (managed objects)
			//}

			// free unmanaged resources (unmanaged objects) and override a finalizer below.
			// set large fields to null: Not applicable.
			if (_gcHandle.IsAllocated)
			{
				_gcHandle.Free();
			}

			_isDisposed = true;

		}
		~PinnedGCHandle()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}
	}
}
