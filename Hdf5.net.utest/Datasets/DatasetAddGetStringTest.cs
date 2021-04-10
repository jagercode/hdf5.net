using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdf5.utest.Datasets
{
	internal class DatasetAddGetStringTest : DatasetAddGetValueTestBase<string>
	{
		internal override string[] Get1DArrayValue()
		{
			// todo: use utf-8 characters.
			return new[] { "one", "two", "three four five" };
		}

		internal override string[,,] Get3DArrayValue()
		{
			throw new NotImplementedException();
		}

		internal override string[,,,,,,,] Get8DArrayValue()
		{
			throw new NotImplementedException();
		}

		internal override string GetScalarValue()
		{
			return "Brother moon and sister Woods";
		}
	}
}
