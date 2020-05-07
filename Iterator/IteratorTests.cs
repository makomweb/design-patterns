using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Iterator
{
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value) 
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            Value = value;
            Right = right;
            Left = left;
            left.Parent = right.Parent = this;
        }
    }

    public class InOrderIterator<T>
    {
        private readonly Node<T> root;
        public Node<T> Current { get; set; }
        private bool yieldingStarted;

        public InOrderIterator(Node<T> root)
        {
            this.root = root;
            Current = root;

            while (Current.Left != null)
            {
                Current = Current.Left;
            }

            //     1 <- root
            //    / \
            //   2   3
            //   ^ current
        }

        public bool MoveNext()
        {
            if (!yieldingStarted)
            {
                yieldingStarted = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;

                while (Current.Left != null)
                {
                    Current = Current.Left;
                }

                return true;
            }
            else
            {
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }

                Current = p;
                return Current != null;
            }
        }

        public void Reset()
        {
            Current = root;
            yieldingStarted = false;
        }
    }

    public class BinaryTree<T>
    {
        private Node<T> _root;

        public BinaryTree(Node<T> root)
        {
            _root = root;
        }

#if false
        public IEnumerable<Node<T>> InOrder
        {
            get
            {
                static IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    if (current.Left != null)
                    {
                        foreach (var left in Traverse(current.Left))
                            yield return left;
                    }
                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in Traverse(current.Right))
                            yield return right;
                    }
                }

                foreach (var node in Traverse(_root))
                {
                    yield return node;
                }
            }
        }
#else
        public InOrderIterator<T> GetEnumerator()
        {
            return new InOrderIterator<T>(_root);
        }
#endif
    }

    public class IteratorTests
    {
        //     1
        //    / \
        //   2   3
        //
        // in order: 213
        [Test]
        public void Test1()
        {
            var root = new Node<int>(1,
                new Node<int>(2), new Node<int>(3));

            var it = new InOrderIterator<int>(root);
            while (it.MoveNext())
            {
                Debug.Write(it.Current.Value);
                Debug.Write(',');
            }
        }

#if false
        [Test]
        public void Test2()
        {
            var root = new Node<int>(1,
                new Node<int>(2), new Node<int>(3));

            var tree = new BinaryTree<int>(root);

            var result = string.Join(",", tree.InOrder.Select(o => o.Value));
            Assert.False(string.IsNullOrEmpty(result));
        }
#endif

        [Test]
        public void Test_duck_typing()
        {
            var root = new Node<int>(1,
                new Node<int>(2), new Node<int>(3));

            var tree = new BinaryTree<int>(root);

            foreach (var node in tree)
            {
                Debug.WriteLine(node.Value);
            }
        }
    }
}