using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using HDF.PInvoke;
//
using Hdf5.HdfBridge;

namespace Hdf5
{
	// File : GroupBase : NodeBase : INode
	// Group : GroupBase : NodeBase : INode
	// DataSet : ValueEntryBase : INode // Either re-implement INode logic or re-implement IMultiDimensionalValueEntry (Please allow multiple inheritance!)
	// Attribute : ValueEntryBase
	public class Group : INode, IMultiAccessable
	{
		internal virtual Id Id { get ; set ; }

		// private Id _id = Id.Invalid; 

		internal Group()
		{
			Id = Id.Invalid;
			DataSets = new DataSetList(this);
			Name = string.Empty;
		}
		
		public Group Parent { get; }

		// internal Group(Group parent)
		// {
		//		Parent = parent;
		// }

		public virtual string Name { get ;  set ; } // todo: implement rename
		public virtual string Location { get; set; } = Path.Separator; // todo: build using path from parent. Root starts with PathSeparator and a readonly name.

		// Q: add extra sugar by repeating the collection add-methods of groups and datasets and making the interface less clean?

		public AttributeCollection Attributes { get; }

		public DataSetList DataSets { get; }

		public GroupCollection Groups { get; }

		private ISet<OpenStateHandle> _openStateHandles = new HashSet<OpenStateHandle>();

		/// <summary>
		/// Returns an Id handle to an open group. Clients can dispose it without knowing if 
		/// it is the root group (thus the file itself) or any group in the file. 
		/// If it is the root it is not disposed. Use Open returns a non disposing IDisposable
		/// if it is already open. 
		/// /!\ TODO: This allows also multiple opening of the group as long as any subsequent calls to UseOpen() 
		/// are Disposed BEFORE the first call because the first call will actually close. 
		/// So this does not work in concurrencyscenario's where you don't know which thread will be finished first.
		/// </summary>
		/// <returns></returns>
		internal virtual OpenStateHandle UseOpen()
		{
			return new OpenStateHandle(this);
		}


		void IMultiAccessable.ClaimAccess(OpenStateHandle handle)
		{
			// root is always open. And if it's not in the file yet there's nothing to open.
			if (Parent == null) return;

			// if this is the first claim open the object.
			if (_openStateHandles.Count == 0)
			{
				var utf8Name = Name.ToUtf8Bytes();
				Id = H5G.open(Parent.Id, utf8Name);
				Debug.Assert(Id.IsValid, $"Failed to open group '{Name}'");
			}
			_openStateHandles.Add(handle);
		}

		void IMultiAccessable.DropAccess(OpenStateHandle hanedle)
		{
			// root is always open and if not in file nothing to open.
			if (Parent == null) return;


			_openStateHandles.Remove(hanedle);
			// last access claim dropped: close the object.
			if (_openStateHandles.Count == 0)
			{
				Result res = H5G.close(Id);
				Id = Id.Invalid;
				Debug.Assert(res.IsOk, $"Failed to close group '{Name}'.");
			}
		}
	}

	// >> future work
	//class StoredGroup { }
	//class InMemGroup { }
	// << 

	public class GroupCollection
	{
		// on disk / in mem state machine could be implemented here. Using IGroup or abstract Group as public member and switching between state just alters the collection.
		public Group this[string name] { get => throw new NotImplementedException(); }

		public void Add(Group group) { throw new NotImplementedException(); }

		public void Add(string name) { throw new NotImplementedException(); }

		public Group Create(string name) { throw new NotImplementedException(); }
	}
}
