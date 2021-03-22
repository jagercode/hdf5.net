using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace jagercode.Testing
{
	public abstract class TestBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetThisMethodName()
		{
			StackTrace st = new StackTrace();
			StackFrame sf = st.GetFrame(1);

			return sf.GetMethod().Name;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetThisClassAndMethodName()
		{
			StackTrace st = new StackTrace();
			StackFrame sf = st.GetFrame(1);

			var fn = sf.GetMethod().Name;
			var cn = GetType().Name;

			return $"{cn}.{fn}";
		}

	}

}
