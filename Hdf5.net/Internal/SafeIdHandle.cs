using System;
using System.Diagnostics;

namespace jagercode.Hdf5.Internal
{
	using Hdf5Id = Id;
	using hid_t = Int64;

	/// <summary>
	/// Safe handle to an open Hdf5 item id.
	/// Q: can this replace the Hdf5Id class?
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	internal sealed class SafeIdHandle : IDisposable
	{
		private readonly Func<hid_t, int> _closeId;

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeIdHandle"/> class.
		/// Note: invalid handles are supported to prevent throwing from a using statement
		/// in order to provide meaningful feedback. 
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="closeAction">The close action.</param>
		/// <exception cref="ArgumentNullException">closeAction</exception>
		public SafeIdHandle(Hdf5Id id, Func<hid_t, int> closeAction)
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

		public Hdf5Id Id { get; private set; }

		private void ReleaseUnmanagedResources()
		{
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
			Id = Hdf5Id.Invalid;
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
	}
}
