using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace IteratorExercise
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
            Left = left;
            Right = right;

            if (left != null)
            {
                left.Parent = this;
            }

            if (right != null)
            {
                right.Parent = this;
            }
        }

        public IEnumerable<T> PreOrder
        {
            get
            {
                var tree = new BinaryTree<T>(this);
                return tree.PreOrder.Select(node => node.Value);
            }
        }
    }

    public class BinaryTree<T>
    {
        private Node<T> _root;

        public BinaryTree(Node<T> root)
        {
            _root = root;
        }

        public IEnumerable<Node<T>> PreOrder
        {
            get
            {
                IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    yield return current;

                    if (current.Left != null)
                    {
                        foreach (var left in Traverse(current.Left))
                            yield return left;
                    }

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
    }

    public class IteratorExerciseTests
    {
        [Test]
        public void Test_preorder_traversal()
        {
            var root = new Node<char>('A', 
                new Node<char>('B', 
                    new Node<char>('D'), 
                    new Node<char>('E', 
                        new Node<char>('F'),
                        null)), 
                new Node<char>('C', 
                    null, 
                    new Node<char>('G', 
                        new Node<char>('H'), 
                        null)));
            
            var result = root.PreOrder;
            Assert.That(new char[] { 'A', 'B', 'D', 'E', 'F', 'C', 'G', 'H' }, Is.EquivalentTo(result));
        }
    }
}