using System;
using System.Text;

namespace Hdf5.Internal
{
	internal static class Utf8Extensions
	{
		public static byte[] ToUtf8Bytes(this string canBeUtf8)
		{
			if (null == canBeUtf8)
			{
				throw new NullReferenceException(); // can't call methods on null refs.
			}

			return Encoding.UTF8.GetBytes(canBeUtf8);
		}

		public static string FromUtf8Bytes(this byte[] bytes)
		{
			if (null == bytes)
			{
				throw new NullReferenceException(); // can't call methods on null refs.
			}

			return Encoding.UTF8.GetString(bytes);
		}

		//public static byte[] ToAsciiBytes(this string restrictToAscii)
		//{
		//	// todo: add some intelligence to replace é by e etc.
		//	throw new NotImplementedException();
		//	return Encoding.ASCII.GetBytes(restrictToAscii);
		//}

	}
}
