using NUnit.Framework;


namespace Hdf5.utest
{
	[TestFixture]
	public class ScenarioTests
	{
		[Test]
		public void Add_structure_using_object_initializer()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void Add_structure_to_file_directly()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void Add_structure_provided_by_factory_method()
		{
			Group createGroup()
			{
				return new Group
				{
					Name = "Factory Group",
					DataSets =
					{
						{ "Data set", 12d },
						// future work (milestone 3, In Memory)
						//{ new DataSet<double>("aap", 13d)
						//{
						//	Attributes =
						//	{
						//		{ "aap double", 2d },
						//		{"aap string", "aap string" },
						//	}

						//}
						//},
					},
					Attributes =
					{
						{"double", 1d },
						{"int64", (long)12 },
						{"string", "string"},

					},
				};
			};

			var g = createGroup();
			var f = new File("testpath");
			f.Groups.Add(g);

		}

		[Test]
		public void Find_all_datasets_with_certain_name()
		{
		}

		[Test]
		public void Find_all_groups_with_certain_name()
		{
		}

		[Test]
		public void Find_all_datasets_with_certain_attribute_names()
		{
		}

	}
}
