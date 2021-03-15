using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace jagercode.Testing
{
    public class Site
    {
		public class Default
		{
			public const string OutFolder = "out";

			public const string CasesFolder = "test files";
		}

		private Site(string root)
		{
			Out = new DirectoryInfo(Path.Combine(root, Default.OutFolder));

		}

		public static Site AtTypeAssemblyPath(Type type)
		{
			var path = type.Assembly.CodeBase;
			path = Path.GetDirectoryName(path);
			return new Site(path);

		}

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
