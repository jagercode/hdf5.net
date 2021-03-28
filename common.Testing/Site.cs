using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Common.Testing
{
    public class Site
    {
		public class Default
		{
			public const string OutFolder = "out";

			public const string CasesFolder = "resources";
		}

		private Site(string root)
		{
			Out = new DirectoryInfo(Path.Combine(root, Default.OutFolder));
			if (!Out.Exists) Out.Create();
			Resources = new DirectoryInfo(Path.Combine(root, Default.CasesFolder));
			if (!Resources.Exists) throw new DirectoryNotFoundException($"'{Resources.FullName}'");
		}

		public static Site AtTypeAssemblyPath(Type type)
		{
			var uripath = new Uri( type.Assembly.CodeBase);
			var path = uripath.LocalPath;
			path = Path.GetDirectoryName(path);
			return new Site(path);
		}

		public string ReserveOutForResource(string pattern)
		{
			throw new NotImplementedException();
		}

		public string CopyResourceToOut(string pattern)
		{
			throw new NotImplementedException();
		}

		public DirectoryInfo Resources { get; private set; }
		public DirectoryInfo Out { get; private set; }
		
		public string ReserveOut(string fileName)
		{
			var file = Out.NewFileInfo(fileName);
			if (file.Exists) file.Delete();
			return file.FullName;
		}
    }

	internal static class IOExtensions
	{
		public static FileInfo NewFileInfo(this DirectoryInfo dir, string fileName)
		{
			return new FileInfo(Path.Combine(dir.FullName, fileName));
		}
	}

}
