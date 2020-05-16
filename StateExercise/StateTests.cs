using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace StateExercise
{
    public class CombinationLock
    {
        private readonly int[] _combination;
        private const string LockedLabel = "LOCKED";
        private const string OpenLabel = "OPEN";
        private const string ErrorLabel = "ERROR";
        private readonly List<int> _entered = new List<int>();

        public CombinationLock(int[] combination)
        {
            _combination = combination;
            Status = LockedLabel;
        }

        // you need to be changing this on user input
        public string Status;

        public void EnterDigit(int digit)
        {
            _entered.Add(digit);

            if (_combination.IsSameAs(_entered))
            {
                Status = OpenLabel;
            }
            else
            {
                if (_combination.Contains(_entered.ToArray()))
                {
                    Status = string.Join("", _entered);
                }
                else
                {
                    Status = ErrorLabel;
                    _entered.Clear();
                }
            }
        }
    }

    public static class ArrayHelper
    {
        public static bool IsSameAs(this int[] one, List<int> other)
        {
            var areListsEqual = true;

            if (one.Count() != other.Count)
                return false;

            for (var i = 0; i < one.Count(); i++)
            {
                if (other[i] != one[i])
                {
                    areListsEqual = false;
                }
            }

            return areListsEqual;
        }

        public static bool Contains<T>(this T[] array, T[] candidate)
        {
            if (IsEmptyLocate(array, candidate))
                return false;

            if (candidate.Length > array.Length)
                return false;

            for (int a = 0; a <= array.Length - candidate.Length; a++)
            {
                if (array[a].Equals(candidate[0]))
                {
                    int i = 0;
                    for (; i < candidate.Length; i++)
                    {
                        if (false == array[a + i].Equals(candidate[i]))
                            break;
                    }
                    if (i == candidate.Length)
                        return true;
                }
            }
            return false;
        }

        static bool IsEmptyLocate<T>(T[] array, T[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }
    }

    public class StateTests
    {
        CombinationLock _lock;

        [SetUp]
        public void Setup()
        {
            _lock = new CombinationLock(new[] { 1, 2, 3, 4, 5 });
        }

        [Test]
        public void Test_if_lock_is_locked_initially()
        {
            Assert.That(_lock.Status, Is.EqualTo("LOCKED"));
        }

        [Test]
        public void Test_if_status_is_updated_on_first_success()
        {
            _lock.EnterDigit(1);

            Assert.That(_lock.Status, Is.EqualTo("1"));
        }

        [Test]
        public void Test_if_status_is_update_on_correct_sequence()
        {
            _lock.EnterDigit(1);
            _lock.EnterDigit(2);
            _lock.EnterDigit(3);
            _lock.EnterDigit(4);
            _lock.EnterDigit(5);

            Assert.That(_lock.Status, Is.EqualTo("OPEN"));
        }

        [Test]
        public void Test_if_wrong_input_leads_to_lock()
        {
            _lock.EnterDigit(1);
            _lock.EnterDigit(4);

            Assert.That(_lock.Status, Is.EqualTo("ERROR"));
        }
    }
}