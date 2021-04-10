using System;
//
using HDF.PInvoke;

namespace Hdf5.HdfBridge
{
	/// <summary>
	/// Path formatting, building and checking.
	/// Does not support checking for attributes. Use attribute class for that. 
	/// Todo: add to public api
	/// </summary>
	internal class Path
	{
		public Path(string path)
		{
			// todo: check for validity. (argument exception)
		}

		/// <summary>
		/// Returns a new instance from the current path and the provided elements.
		/// </summary>
		/// <param name="parts"></param>
		/// <returns></returns>
		public Path Append(params string[] parts)
		{
			throw new NotImplementedException();
		}

		public static string Combine(params string[] parts)
		{
			throw new NotImplementedException();
		}

		internal static bool IsValidName(string name)
		{
			if (string.IsNullOrEmpty(name)) return false;
			if (name == Dot) return false;
			return true; // whitespace only highly discouraged but not an error.
		}

		public const string Dot = ".";

		public static string[] Split(string path)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Verifies whether the provided object already contains an item
		/// with the specified name.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Failed: H5L.exists({objectId},{pathBytes})</exception>
		internal static bool Exists(Id groupId, string path)
		{
			// 1. group id can be file id.
			// 2. path can be relative: use w.r.t. group id.
			// 3. path can be absolute: find fileId from group id if this isn't the root group already and find path.
			// Note: H5L.exists supports paths, not only names of items in a group.
			byte[] pathBytes = path.ToUtf8Bytes();
			var res = H5L.exists(groupId, pathBytes, 0);
			if (res < 0)
			{
				throw new InvalidOperationException($"Failed: H5L.exists({groupId},{pathBytes})");
			}

			return Convert.ToBoolean(res);
		}
		public const string Separator = "/";

		internal static bool IsNameOnly(string name)
		{
			return (!name.Contains(Path.Separator));
		}
	}
}
