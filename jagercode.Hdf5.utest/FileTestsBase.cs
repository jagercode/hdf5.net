using jagercode.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.Hdf5.utest
{
	internal abstract class FileTestsBase : TestBase
	{
		protected FileTestsBase()
		{
			Site = Site.AtTypeAssemblyPath(this.GetType());
		}

		protected Site Site { get; }
	}
}
