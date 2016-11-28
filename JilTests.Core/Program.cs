using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JilTests.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var asm = Assembly.GetEntryAssembly();
            var testGroups = asm.GetTypes();

            var ran = 0;
            var passed = 0;

            var failed = new HashSet<string>();

            foreach(var group in testGroups.OrderBy(t => t.Name))
            {
                var isTestClass = group.GetTypeInfo().CustomAttributes.Any(c => c.AttributeType == typeof(Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute));

                if (!isTestClass) continue;

                var cons = group.GetConstructor(Type.EmptyTypes);
                
                var first = true;

                var mtds = group.GetMethods().OrderBy(m => m.Name);
                foreach(var mtd in mtds)
                {
                    var isTestMethod = mtd.CustomAttributes.Any(c => c.AttributeType == typeof(Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute));
                    if (!isTestMethod) continue;
                    
                    if (first)
                    {
                        Console.WriteLine("Starting Group: " + group.Name);
                        first = false;
                    }

                    Console.Write("\t" + mtd.Name + ": ");
                    var res = Run(cons, mtd);

                    Console.Write(res.Runtime.TotalMilliseconds + "ms");
                    Console.WriteLine(res.Passed ? " passed" : " !FAILED!");
                    if (!res.Passed)
                    {
                        Console.WriteLine(res.ErrorMessage);
                        Console.WriteLine(res.StackTrace);
                        failed.Add(group.Name + "." + mtd.Name);
                    }

                    ran++;
                    if (res.Passed) passed++;
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(passed + "/" + ran + " passsed");
            Console.WriteLine();
            if(failed.Count > 0)
            {
                Console.WriteLine("Failed " + failed.Count + ":");
                foreach(var fail in failed.OrderBy(_ => _))
                {
                    Console.WriteLine("\t" + fail);
                }
            }

            Console.ReadKey();
        }

        class TestResult
        {
            public bool Passed => ErrorMessage == null;

            public TimeSpan Runtime { get; set; }
            public string ErrorMessage { get; set; }
            public string StackTrace { get; set; }
        }

        static TestResult Run(ConstructorInfo makeTestClass, MethodInfo mtd)
        {
            var inst = makeTestClass.Invoke(new object[0]);

            var watch = new Stopwatch();
            watch.Start();
            Exception failure;
            try
            {
                mtd.Invoke(inst, new object[0]);
                failure = null;
            }
            catch (Exception e)
            {
                failure = e;
            }
            watch.Stop();

            return
               new TestResult
               {
                   Runtime = watch.Elapsed,
                   ErrorMessage = failure?.Message,
                   StackTrace = failure?.StackTrace
               };
        }
    }
}
