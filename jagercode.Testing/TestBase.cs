using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace jagercode.Testing
{
	public abstract class TestBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public string CurrentMethodName()
		{
			StackTrace st = new StackTrace();
			StackFrame sf = st.GetFrame(1);

			return sf.GetMethod().Name;
		}
	}
}
