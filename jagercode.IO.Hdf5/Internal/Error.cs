using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5.Internal
{
	using herr_t = System.Int32;

	internal struct Error
	{
		private herr_t _errorCode;

		Error(herr_t errorCode)
		{
			_errorCode = errorCode;
		}

		public string Description
		{
			get => throw new NotImplementedException("todo: look up in H5E");
		}

		public Exception ToException() => new Hdf5Exception(Description);


	}

	public class Hdf5Exception : InvalidOperationException
	{
		internal Hdf5Exception(string message) : base(message) { }
	}
}
