using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
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
                if (value == _name) return; // change guard prevents infinite recursion
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
                if (value == _productName) return; // change guard prevents infinite recursion
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

    public sealed class BidirectionalBinding : IDisposable
    {
        private bool _disposed;

        public BidirectionalBinding(
            INotifyPropertyChanged first,
            Expression<Func<object>> firstProperty, 
            INotifyPropertyChanged second,
            Expression<Func<object>> secondProperty)
        {
            if (firstProperty.Body is MemberExpression firstExpr &&
                secondProperty.Body is MemberExpression secondExpr)
            {
                if (firstExpr.Member is PropertyInfo firstProp &&
                    secondExpr.Member is PropertyInfo secondProp)
                {
                    first.PropertyChanged += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            secondProp.SetValue(second, firstProp.GetValue(first));
                        }
                    };

                    second.PropertyChanged += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            firstProp.SetValue(first, secondProp.GetValue(second));
                        }
                    };
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }

    public class ObserverTests
    {
        [Test]
        public void Test1()
        {
            var p = new Product { Name = "Book" };
            var w = new Window { ProductName = "Book" };

#if false
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
#else
            using var binding = new BidirectionalBinding(
                p, () => p.Name,
                w, () => w.ProductName);
#endif

            p.Name = "Smart book";
            Assert.AreEqual("Smart book", w.ProductName);

            w.ProductName = "Really smart book";
            Assert.AreEqual("Really smart book", p.Name);
        }
    }
}