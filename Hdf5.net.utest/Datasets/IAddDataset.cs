namespace Hdf5.utest.Datasets
{
	internal interface IMultiShapeTestCasesOf<T>
	{
		void Test_scalar_value();
		void Test_1d_value();
		// void Test_2d_value(); if it works for 3d it works for 2d and 8d.
		void Test_3d_value();
		void Test_8d_value(); // leave it here to beat the fortran limit?
	}
}
