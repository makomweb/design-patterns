using NUnit.Framework;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ObserverRx
{
    public class Market : INotifyPropertyChanged
    {
        private float _volatility;

        public float Volatility
        {
            get => _volatility;
            set
            {
                if (value.Equals(_volatility)) return;

                _volatility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ObserverRxTests
    {
        [Test]
        public void Test1()
        {
            var m = new Market();
            m.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Volatility")
                {
                    // TODO Handle!
                }
            };
        }
    }
}