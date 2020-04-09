using System;
using System.Collections.Generic;
using System.Text;

namespace ExcerciseBuilder
{
    public class CodeBuilder
    {
        private string _className;

        private Dictionary<string, string> _fields = new Dictionary<string, string>();

        public CodeBuilder(string className)
        {
            _className = className;
        }

        public CodeBuilder AddField(string name, string type)
        {
            _fields.Add(name, type);
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"public class {_className}");
            sb.AppendLine("{");

            foreach (var pair in _fields)
            {
                sb.AppendLine($"  public {pair.Value} {pair.Key};");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cb = new CodeBuilder("Person").AddField("Name", "string").AddField("Age", "int");

            Console.WriteLine(cb);
        }
    }
}
