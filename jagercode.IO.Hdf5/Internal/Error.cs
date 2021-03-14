using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.Hdf5.Internal
{
	using herr_t = System.Int32;

	internal struct Result
	{
		private herr_t _errorCode;

		Result (herr_t errorCode)
		{
			_errorCode = errorCode;
		}

		public string Description
		{
			get => throw new NotImplementedException("todo: look up in H5E");
		}

		public Exception ToException() => new Hdf5Exception(Description);

		public bool IsOk => _errorCode == 0;

		public bool HasFailed => !IsOk;

		public static implicit operator Result(herr_t returnCode) => new Result(returnCode);
	}

	public class Hdf5Exception : InvalidOperationException
	{
		internal Hdf5Exception(string message) : base(message) { }
	}
}
