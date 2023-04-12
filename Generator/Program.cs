using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Lazorm 
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly thisAssem = typeof(Program).Assembly;
            AssemblyName thisAssemName = thisAssem.GetName();
            Version ver = thisAssemName.Version;

            Console.WriteLine("Lazorm (c)satrex");
            Console.WriteLine($"assembly {thisAssemName.Name} version {ver}");
            Console.WriteLine();

            Array.ForEach(args, a => Debug.WriteLine(a)); 
            //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            //Trace.AutoFlush = true;

            CliParser.Parse(args);
  
        }
    }
}
