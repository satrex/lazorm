using System;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;

namespace LazormTest
{
	public class BoilerplateTest : IDisposable
    {
		public BoilerplateTest()
		{
		}

        [Fact]
        public void RegexMatches()
        {
            var newText = Regex.Replace("builder.Services.AddFluxor(options => options.ScanAssemblies(currentAssembly));\nbuilder.Services.AddSingleton<WeatherForecastService>();\n\nvar app = builder.Build();\n", "^.*builder.Build.*$", "hoge\n$&", RegexOptions.Multiline);

            Console.Clear();
            Console.WriteLine(newText);
        }

        void IDisposable.Dispose()
        {
        }
    }
}

