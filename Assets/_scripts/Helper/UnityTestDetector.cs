using System;

namespace Gianni.Helper
{
	/// <summary>
	/// Detect if we are running as part of a nUnit unit test.
	/// This is DIRTY and should only be used if absolutely necessary 
	/// as its usually a sign of bad design.
	/// </summary>    
	/// <remarks>https://stackoverflow.com/questions/3167617/determine-if-code-is-running-as-part-of-a-unit-test</remarks>
	static class UnitTestDetector
	{
		static UnitTestDetector()
		{
			foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
			{
				// Can't do something like this as it will load the nUnit assembly
				// if (assem == typeof(NUnit.Framework.Assert))

				if (assem.FullName.ToLowerInvariant().StartsWith("nunit.framework"))
				{
					IsRunningFromNUnit = true;
					break;
				}
			}
		}

		public static bool IsRunningFromNUnit { get; } = false;
	}
}
