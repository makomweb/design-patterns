using NUnit.Framework;
using System.ComponentModel;

namespace ProxyViewModel
{
    // Model
    public class Person
    {
        public string FirstName, LastName;
    }

    // ViewModel is a proxy for the model
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly Person _person;

        public PersonViewModel(Person p)
        {
            _person = p;
        }

        public string FirstName
        {
            get => _person.FirstName;
            set
            {
                if (_person.FirstName == value) return;
                _person.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _person.LastName;
            set
            {
                if (_person.LastName == value) return;
                _person.LastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}