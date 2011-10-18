using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using cleanCore;
using Microsoft.CSharp;

namespace cleanLayer.Library.Scripts
{
    public static class ScriptManager
    {
        static ScriptManager()
        {
            RegistrationQueue = new Queue<Type>();
            ScriptPool = new LinkedList<Script>();
        }

        private static readonly object SynchronizeLock = new object();

        public static string ScriptFolder
        {
            get;
            private set;
        }

        private static Queue<Type> RegistrationQueue
        {
            get;
            set;
        }

        public static LinkedList<Script> ScriptPool
        {
            get;
            private set;
        }

        private static Script CurrentScript
        {
            get;
            set;
        }

        public static void Initialize()
        {
            ScriptFolder = Path.Combine(Program.Directory, @"Scripts\");
            //CompileAsync();
            lock (SynchronizeLock)
            {
                var typeScripts = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where t.IsSubclassOf(typeof(Script))
                                   select t).ToList();
                typeScripts.ForEach(t =>
                {
                    Register(t);
                    //var s = (Script)Activator.CreateInstance(t);
                    //ScriptPool.AddLast(s);
                });
            }
        }

        public static void Pulse()
        {
            if (!Manager.IsInGame)
                return;

            lock (SynchronizeLock)
            {
                while (RegistrationQueue.Count > 0)
                    Register(RegistrationQueue.Dequeue());

                foreach (var script in ScriptPool)
                {
                    CurrentScript = script;
                    script.Tick();
                }

                CurrentScript = null;
            }
        }

        public static void CompileAsync()
        {
            ThreadPool.QueueUserWorkItem((state) => Compile());
        }

        private static void Compile()
        {
            try
            {
                OnCompilerStarted();

                lock (SynchronizeLock)
                    ScriptPool.Clear();

                string[] files = Directory.GetFiles(ScriptFolder, "*.cs", SearchOption.AllDirectories);
                Log.WriteLine("Found {0} files in Scripts folder", files.Length);

                string[] references = { "System.dll", "System.Core.dll", "../WhiteMagic.dll", "../cleanCore.exe", Assembly.GetExecutingAssembly().Location };

                var parameters = new CompilerParameters(references)
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = false,
                    OutputAssembly = "Scripts"
                };

                var provider = new CSharpCodeProvider();
                var results = provider.CompileAssemblyFromFile(parameters, files);

                var errors = results.Errors;

                Log.WriteLine("Errors:");
                foreach (CompilerError error in errors)
                    if (!error.IsWarning)
                        Log.WriteLine("\t{0}", error.ErrorText);
                Log.WriteLine("");

                Log.WriteLine("Warnings:");
                foreach (CompilerError warning in errors)
                    if (warning.IsWarning)
                        Log.WriteLine("\t{0}", warning.ErrorText);
                Log.WriteLine("");

                if (errors.HasErrors)
                {
                    Log.WriteLine("Compiler terminated due to errors");
                    return;
                }

                AnalyzeAssembly(results.CompiledAssembly);

            }
            catch (Exception ex)
            {
                Log.WriteLine("Compiler error: {0}", ex.Message);
            }
            finally
            {
                OnCompilerFinished();
            }
        }

        private static void AnalyzeAssembly(Assembly asm)
        {
            Log.WriteLine("Analyzing compiled assembly {0}", asm.FullName);

            var types = asm.GetTypes();
            Log.WriteLine("Found {0} types", types.Length);
            lock (SynchronizeLock)
            {
                foreach (var type in types)
                {
                    if (!type.IsClass || !type.IsSubclassOf(typeof(Script)))
                    {
                        Log.WriteLine("Ignoring {0}", type.Name);
                        continue;
                    }

                    RegistrationQueue.Enqueue(type);
                }
            }
        }

        private static void Register(Type type)
        {
            //Log.WriteLine("Registering {0} as script", type.Name);
            try
            {
                var ctor = type.GetConstructor(new Type[] { });
                if (ctor == null)
                    throw new Exception("Constructor not found");

                var script = (Script)ctor.Invoke(new object[] { });
                if (script == null)
                    throw new Exception("Unable to instantiate script");

                ScriptPool.AddLast(script);
            }
            catch (Exception ex)
            {
                Log.WriteLine("Failed to compile: {0}", ex.Message);
            }
        }

        private static void OnCompilerStarted()
        {
            if (CompilerStarted != null)
                CompilerStarted(null, new EventArgs());
        }

        private static void OnCompilerFinished()
        {
            if (CompilerFinished != null)
                CompilerFinished(null, new EventArgs());
        }

        public static event EventHandler CompilerStarted;
        public static event EventHandler CompilerFinished;
    }
}
