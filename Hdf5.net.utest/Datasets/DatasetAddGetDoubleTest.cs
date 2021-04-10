using System;
using NUnit.Framework;

namespace Hdf5.utest.Datasets
{
	internal class DatasetAddGetDoubleTest : DatasetAddGetValueTestBase<double>
	{
		internal override double[] Get1DArrayValue()
		{
			return new[] { 3d, 2d, 1d };
		}

		internal override double[,,] Get3DArrayValue()
		{
			throw new NotImplementedException();
		}

		internal override double[,,,,,,,] Get8DArrayValue()
		{
			throw new NotImplementedException();
		}

		internal override double GetScalarValue()
		{
			return 99d;
		}
	}
}
