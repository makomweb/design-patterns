using NUnit.Framework;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ObserverPropertyDependencies
{
    public class Person : INotifyPropertyChanged
    {
        private int _age;

        public int Age
        {
            get => _age;
            set
            {
                if (value == _age) return;
                _age = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanVote));
            }
        }

        public bool CanVote => Age >= 16;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ObserverTests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}