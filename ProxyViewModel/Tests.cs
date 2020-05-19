using NUnit.Framework;
using System.ComponentModel;
using System.Linq;

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
                RaisePropertyChanged(nameof(FullName));
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
                RaisePropertyChanged(nameof(FullName));
            }
        }

        public string FullName
        {
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                if (value == null)
                {
                    FirstName = LastName = null;
                    return;
                }

                var split = value.Split();
                if (split.Count() > 0)
                {
                    FirstName = split[0];
                } 
                if (split.Count() > 1)
                {
                    LastName = split[1];
                }
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