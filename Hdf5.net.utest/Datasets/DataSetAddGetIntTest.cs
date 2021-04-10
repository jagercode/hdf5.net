using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdf5.utest.Datasets
{
	internal class DataSetAddGetIntTest : DatasetAddGetValueTestBase<int>
	{
		internal override int[] Get1DArrayValue()
		{
			return new[] { 4, 3, 5, 12486 };
		}

		internal override int[,,] Get3DArrayValue()
		{
			throw new NotImplementedException();
		}

		internal override int[,,,,,,,] Get8DArrayValue()
		{
			throw new NotImplementedException();
		}

		internal override int GetScalarValue()
		{
			return -12345;
		}
	}
}
