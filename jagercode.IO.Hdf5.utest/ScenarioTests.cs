﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace jagercode.IO.Hdf5.utest
{
	[TestClass]
	public class ScenarioTests
	{
		[TestMethod]
		public void Add_structure_using_object_initializer()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void Add_structure_to_file_directly()
		{
			
		}

		[TestMethod]
		public void Add_structure_provided_by_factory_method()
		{
			Group createGroup ()
				{
				return new Group
				{
					Name = "Factory Group",
					//{ "Data set", 12d },
					//{ new DataSet("aap", 13d) },
					Attributes =
					{
						{"double", 1d },
						{"int64", (long)12 },
						{"string", "string"},

					},
				};
			};
			
		}

		[TestMethod]
		public void Find_all_datasets_with_certain_name()
		{
		}

		[TestMethod]
		public void Find_all_groups_with_certain_name()
		{
		}

		[TestMethod]
		public void Find_all_datasets_with_certain_attribute_names()
		{
		}

	}
}