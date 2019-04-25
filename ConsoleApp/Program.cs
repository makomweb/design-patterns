using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    class Program
    {
        class Document
        {

        }

        interface IMachine
        {
            void Print(Document doc);
            void Scan(Document doc);
            void Fax(Document doc);
        }

        class MultiFunctionPrinter : IMachine
        {
            public void Fax(Document doc)
            {
                throw new NotImplementedException();
            }

            public void Print(Document doc)
            {
                throw new NotImplementedException();
            }

            public void Scan(Document doc)
            {
                throw new NotImplementedException();
            }
        }

        class OldFashionedPrinter : IMachine
        {
            public void Fax(Document doc)
            {
                throw new NotImplementedException();
            }

            public void Print(Document doc)
            {
                // TODO to be implemented
            }

            public void Scan(Document doc)
            {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, world!");
        }
    }
}
