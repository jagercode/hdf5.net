namespace jagercode.Hdf5.utest.Datasets
{
	internal interface ITestShapesOf<T>
	{
		void Test_scalar_value();
		void Test_1d_value();
		void Test_2d_value();
		void Test_3d_value();
		void Test_8d_value();
	}
}
