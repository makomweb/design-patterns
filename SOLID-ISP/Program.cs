using System;

namespace SOLID_ISP
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

    interface IMultiFunctionDevice : IPrinter, IScanner, IFax { }

    class MultiFunctionPrinter : IMultiFunctionDevice
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

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
