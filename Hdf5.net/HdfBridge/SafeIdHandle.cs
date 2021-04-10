using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hdf5.HdfBridge
{
	using hid_t = Int64;

	internal interface IMultiAccessable
	{
		void ClaimAccess(OpenStateHandle handle);
		void DropAccess(OpenStateHandle hanedle);
	}

	/// <summary>
	/// This class only opens upon creation and closes on dispose.
	/// Doesn't track state of client. Clients must count for themselves.
	/// O
	/// </summary>
	internal sealed class OpenStateHandle : IDisposable
	{
		private IMultiAccessable _client;
		public OpenStateHandle(IMultiAccessable client)
		{
			_client = client;
			_client.ClaimAccess(this);
			_isDisposed = false;
		}

		public OpenStateHandle(IMultiAccessable client, OpenStateHandle parentOpenStateHandle) : this(client)
		{
			this.parentOpenStateHandle = parentOpenStateHandle;
		}

		private bool _isDisposed;
		private OpenStateHandle parentOpenStateHandle;

		public void Dispose()
		{
			if (_isDisposed) return;

			_isDisposed = true;
			// drop child first
			_client?.DropAccess(this);
			_client = null;
			// drop parent next
			parentOpenStateHandle?.Dispose();
			parentOpenStateHandle = null;
		}

		~OpenStateHandle()
		{
			if (!_isDisposed) Dispose();
		}
	}


	/// <summary>
	/// Safe handle to an open Hdf5 item id.
	/// Q: can this replace the Hdf5Id class?
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	internal sealed class SafeIdHandle : IDisposable
	{
		private readonly Func<hid_t, int> _closeId;

		/// <summary>
		/// Constructor for keeping the object open upon Dispose. 
		/// Initializes a new instance of the <see cref="SafeIdHandle"/> class.
		/// Note: invalid handles are supported to prevent throwing from a using statement
		/// in order to provide meaningful feedback. 
		/// </summary>
		/// <param name="id">The identifier.</param>
		public SafeIdHandle(Id id)
		{
			Id = id;
			_closeId = null;
		}

		/// <summary>
		/// Contructor for closing the object upon Dispose. 
		/// Initializes a new instance of the <see cref="SafeIdHandle"/> class.
		/// Note: invalid handles are supported to prevent throwing from a using statement
		/// in order to provide meaningful feedback. 
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="closeAction">The close action.</param>
		/// <exception cref="ArgumentNullException">closeAction</exception>
		public SafeIdHandle(Id id, Func<hid_t, int> closeAction)
		{
			Id = id;
			_closeId = closeAction ?? throw new ArgumentNullException(nameof(closeAction));
		}

		/// <summary>
		/// Returns true if this handle refers to a valid object in the hdf5 file.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid => Id.IsValid;

		public static implicit operator Id(SafeIdHandle idHandle)
		{
			return idHandle.Id;
		}

		#region IDisposable implementation

		public Id Id { get; private set; }

		private void ReleaseUnmanagedResources()
		{
			if (null == _closeId)
			{
				// allowed to stay open: Id = Id.Invalid;
				return;
			}
			Debug.WriteLine("//>> SafeHdf5ReleaseHandle.ReleaseUnmanagedResources");
			Debug.Assert(Id.IsValid);
			if (Id.IsValid)
			{
				try
				{
					_closeId(Id);
				}
				catch
				{
					// thou shalt never cause exceptions on disposal of resources (but log them)
#if DEBUG
					throw new NotImplementedException("log failing call. ");
#endif
				}
			}
			Id = Id.Invalid;
			Debug.WriteLine("//<< SafeHdf5ReleaseHandle.ReleaseUnmanagedResources");
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~SafeIdHandle()
		{
			ReleaseUnmanagedResources();
		}

		#endregion
	}
}
