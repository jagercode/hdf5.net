using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	public class Group : INode
	{
		public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string Location { get; set; }

		// Q: add extra sugar by repeating the collection add-methods of groups and datasets and making the interface less clean?

		public AttributeCollection Attributes { get; }

		public DataSetCollection DataSets { get; }

		public GroupCollection Groups { get; }
	}

	class StoredGroup { }

	class InMemGroup { }

	public class GroupCollection
	{
		// on disk / in mem state machine could be implemented here. Using IGroup or abstract Group as public member and switching between state just alters the collection.
		public Group this[string name] { get => throw new NotImplementedException(); }

		public void Add(Group group) { }

		public void Add(string name) { }

		public Group Create(string name) { throw new NotImplementedException(); }
	}
}
