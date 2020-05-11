using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ObserverPropertyDependencies
{
    public class PropertyNotificationSupport : INotifyPropertyChanged
    {
        private readonly Dictionary<string, HashSet<string>> _affectedBy
            = new Dictionary<string, HashSet<string>>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));

            foreach (var affected in _affectedBy.Keys)
            {
                if (_affectedBy[affected].Contains(propertyName))
                {
                    OnPropertyChanged(affected);
                }
            }
        }

        protected Func<T> property<T>(string name, Expression<Func<T>> expr)
        {
            Debug.WriteLine($"Creating computed property for expression {expr}");

            var visitor = new MemberAccessVisitor(GetType());
            visitor.Visit(expr);

            if (visitor.PropertyNames.Any())
            {
                if (!_affectedBy.ContainsKey(name))
                    _affectedBy.Add(name, new HashSet<string>());

                foreach (var propName in visitor.PropertyNames)
                    if (propName != name)
                        _affectedBy[name].Add(propName);
            }

            return expr.Compile();
        }

        private class MemberAccessVisitor : ExpressionVisitor
        {
            private readonly Type declaringType;
            public readonly IList<string> PropertyNames = new List<string>();

            public MemberAccessVisitor(Type declaringType)
            {
                this.declaringType = declaringType;
            }

            public override Expression Visit(Expression expr)
            {
                if (expr != null && expr.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpr = (MemberExpression)expr;
                    if (memberExpr.Member.DeclaringType == declaringType)
                    {
                        PropertyNames.Add(memberExpr.Member.Name);
                    }
                }

                return base.Visit(expr);
            }
        }
    }

    public class Person : PropertyNotificationSupport
    {
        private int _age;
        private bool _citizen;

        public int Age
        {
            get => _age;
            set
            {
                if (value == _age) return;
                _age = value;
                OnPropertyChanged();
            }
        }

        public bool Citizen
        {
            get => _citizen;
            set
            {
                if (value == _citizen) return;
                _citizen = value;
                OnPropertyChanged();
            }
        }

        private readonly Func<bool> canVote;

        public bool CanVote => canVote();

        public Person()
        {
            canVote = property(nameof(CanVote), () => Age >= 16 && Citizen);
        }
    }

    public class ObserverTests
    {
        [Test]
        public void Test1()
        {
            var p = new Person();
            p.PropertyChanged += (sender, eventArgs) =>
            {
                Debug.WriteLine($"{eventArgs.PropertyName} has changed!");
            };

            p.Age = 15;
            p.Citizen = true;
        }
    }
}