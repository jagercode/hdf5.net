namespace jagercode.Hdf5.utest.Datasets
{
	internal interface IAddShapesDataset<T>
	{
		void Add_scalar();
		void Add_1d();
		void Add_2d();
		void Add_3d();
		void Add_8d();
	}
}
