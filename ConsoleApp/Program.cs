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

        interface IPrinter
        {
            void Print(Document doc);
        }

        interface IScanner
        {
            void Scan(Document doc);
        }

        interface IFax
        {
            void Fax(Document doc);
        }

        class MultiFunctionPrinter : IPrinter, IScanner, IFax
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

        class OldFashionedPrinter : IPrinter
        {
            public void Print(Document doc)
            {
                throw new NotImplementedException();
            }
        }

        class PhotoCopier : IPrinter, IScanner
        {
            public void Print(Document doc)
            {
                throw new NotImplementedException();
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
