using NUnit.Framework;
using System.Diagnostics;

namespace ProxyProtection
{
    public interface ICar
    {
        string Drive();
    }

    public class Car : ICar
    {
        public string Drive()
        {
            return "Car is being driven";
        }
    }

    public class Driver
    {
        public int Age { get; set; }
    }

    public class CarProxy : ICar
    {
        private ICar _car = new Car();
        private Driver _driver;

        public CarProxy(Driver driver)
        {
            _driver = driver;
        }

        public string Drive()
        {
            if (_driver.Age >= 16)
            {
                return _car.Drive();
            }
            else
            {
                return "Too yount!";
            }
        }
    }

    public class ProtectionProxyTests
    {
        [Test]
        public void Test1()
        {
            ICar car = new CarProxy(new Driver { Age = 11 });
            Assert.False(string.IsNullOrEmpty(car.Drive()));
        }
    }
}