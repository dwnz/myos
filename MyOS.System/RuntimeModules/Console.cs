using System;
using System.Runtime.InteropServices;
using Jurassic;
using Jurassic.Library;

namespace MyOS.System.RuntimeModules
{
    public class ConsoleObject : ObjectInstance
    {
        private readonly ScriptEngine _engine;

        public ConsoleObject(ScriptEngine engine)
            : base(engine)
        {
            _engine = engine;
            PopulateFunctions();
        }

        public ConsoleObject(ObjectInstance prototype)
            : base(prototype)
        {
            PopulateFunctions();
        }

        public ConsoleObject(ScriptEngine engine, ObjectInstance prototype)
            : base(engine, prototype)
        {
            PopulateFunctions();
        }

        [JSFunction(Name = "log")]
        public void Log(string message)
        {
            Console.Write(message);
        }

        [JSFunction(Name = "logline")]
        public void LogLine(string message)
        {
            Console.WriteLine(message);
        }

        [JSFunction(Name = "readline")]
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}