using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public static string[] Split(string path)
		{
			throw new NotImplementedException();
		}


		internal static bool Exists(Id groupId, string path)
		{
			// 1. group id can be file id.
			// 2. path can be relative: use w.r.t. group id.
			// 3. path can be absolute: find fileId from group id if this isn't the root group already and find path.
			throw new NotImplementedException();
		}
	}
}
