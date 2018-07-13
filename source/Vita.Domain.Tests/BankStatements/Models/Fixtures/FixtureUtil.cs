using System.IO;
using System.Reflection;

namespace Vita.Domain.Tests.BankStatements.Models.Fixtures
{
	public static class FixtureUtil
	{
		private static readonly string Namespace = typeof(FixtureUtil).Namespace;

		public static string GetString(string fileName)
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{Namespace}.{fileName}"))
			{
				if (stream == null) throw new FileNotFoundException($"Missing embedded resource file {Namespace}.{fileName}");
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}
  }
}