using NUnit.Framework;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ObserverBidirectional
{
    public class Product : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        { 
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Product: {Name}";
        }
    }

    public class Window : INotifyPropertyChanged
    {
        private string _productName;

        public string ProductName
        {
            get => _productName;
            set
            {
                if (value == _productName) return;
                _productName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Window: {ProductName}";
        }
    }

    public class ObserverTests
    {
        [Test]
        public void Test1()
        {
            var p = new Product { Name = "Book" };
            var w = new Window { ProductName = "Book" };
            p.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == "Name")
                {
                    Debug.WriteLine($"Name was changed in product");
                    w.ProductName = p.Name;
                }
            };

            w.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == "ProductName")
                {
                    Debug.WriteLine($"Name was changed in window");
                    p.Name = w.ProductName;
                }
            };

            p.Name = "Smart book";

            Assert.AreEqual("Smart book", w.ProductName);
        }
    }
}