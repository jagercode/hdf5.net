using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdf5.utest.Datasets
{
	public class Id_
	{
		public long Id { get; }
	}

	internal interface IMultipleOpenableId
	{
		Id_ Id { get; }
		void Open();
		void Close();
		uint OpenCount { get; }
	}

	internal class MultipleOpenIdHandle : IDisposable
	{
		private IMultipleOpenableId _id;

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~MultipleOpenIdHandle() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}

	[TestFixture]
	internal class AccessManagerTests
	{
		[Test]
		public void TestAccessManager()
		{

		}

		[Test]
		public void CanOpenHdf5ObjectTwice()
		{
			Assert.Ignore("todo");
		}
	}
}
