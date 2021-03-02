using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	public class Attribute : INdArray
	{
		public string Name { get; set; }

		// public INdArray Value { get; set; }

		public bool IsScalar => throw new NotImplementedException();

		public ulong[] Shape => throw new NotImplementedException();

		public Type Type => throw new NotImplementedException();

		public object ValueAsObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
