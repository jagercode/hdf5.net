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
			OutDir = new DirectoryInfo(Path.Combine(root, Default.OutFolder));
			if (!OutDir.Exists) OutDir.Create();
			ResourceDir = new DirectoryInfo(Path.Combine(root, Default.CasesFolder));
			if (!ResourceDir.Exists) throw new DirectoryNotFoundException($"'{ResourceDir.FullName}'");
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
			FileInfo resource;
			try
			{
				resource = ResourceDir.EnumerateFiles(pattern).First();
			}
			catch
			{
				throw new FileNotFoundException(pattern);
			}
			FileInfo outFile = new FileInfo (Path.Combine(OutDir.FullName, pattern));
			if (outFile.Exists) outFile.Delete();
			return outFile.FullName;
		}

		public string CopyResourceToOut(string pattern)
		{
			FileInfo resource;
			try
			{
				resource = ResourceDir.EnumerateFiles(pattern).First();
			}
			catch
			{
				throw new FileNotFoundException(pattern);
			}
			string outPath = Path.Combine(OutDir.FullName, pattern);
			resource.CopyTo(outPath, true);
			return outPath;
		}

		public DirectoryInfo ResourceDir { get; private set; }
		public DirectoryInfo OutDir { get; private set; }
		
		public string ReserveOut(string fileName)
		{
			var file = OutDir.NewFileInfo(fileName);
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
