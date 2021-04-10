using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using HDF.PInvoke;
//
using Hdf5.HdfBridge;

namespace Hdf5
{
	public class Group : INode
	{
		internal virtual Id Id { get ; set ; }

		// private Id _id = Id.Invalid; 

		internal Group()
		{
			Id = Id.Invalid;
			DataSets = new DataSetList(this);
		}

		public Group Parent { get; }

		// internal Group(Group parent)
		// {
		//		Parent = parent;
		// }

		public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string Location { get; set; }

		// Q: add extra sugar by repeating the collection add-methods of groups and datasets and making the interface less clean?

		public AttributeCollection Attributes { get; }

		public DataSetList DataSets { get; }

		public GroupCollection Groups { get; }

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
		internal SafeIdHandle UseOpen()
		{
			// problem: if this is the file (root) it is already open and must stay so. /!\ No closing allowed. 
			// Mixing File object and Root group leads to code like this:
			// If already open, return for use and don't close.
			// If not open yet, return open object with close on disposal.
			// Solved by abusing the OpenIdHandle by allowing it to have a dummy Dispose operation.
			// Maybe this is a good reason for a separate OpenGroupHandle.
			if (Id.IsValid)
			{
				return new SafeIdHandle(Id); // no closing upon dispose. 
			}
			
			// in case this is the root group parent is null but since it is always open until disposed
			// we should never get here for the root only if it Disposed. 
			// Is this a good reason to propagate the File to all objects? Requiring the File.Id should throw
			// ObjectDisposedException. 
			// ??? public bool IsRoot => Parent != null; => string.IsNullOrWhiteSpace(Name); => Name == "/";
			System.Diagnostics.Debug.Assert(Parent != null, "Parent != null");
			
			// anyway, 
			var utf8Name = Name.ToUtf8Bytes();
			Id = H5G.open(Parent.Id, utf8Name);
			return new SafeIdHandle(Id, H5G.close);
		}

		internal static class Hdf
		{
			
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
