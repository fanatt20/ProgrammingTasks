using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominoes
{
    public class Domino 
    {
        protected readonly int _firstNumber;
        protected readonly int _secondNumber;

        public Domino(int firstNumber, int secondNumber)
        {
            _firstNumber = firstNumber;
            _secondNumber = secondNumber;
        }

        protected Domino()
        {
        }

        public int FirstNumber
        {
            get { return  _firstNumber; }
        }

        public int SecondNumber
        {
            get { return _secondNumber; }
        }
    }

    public class DominoNode : Domino
    {
        private bool _linkedByFirstNumber;
        private bool _linkedBySecondNumber;
        private bool _rightNumber;

        public DominoNode()
        {
        }

        public DominoNode(int x, int y)
            : base(x, y)
        {
        }

        public DominoNode(Domino domino)
            : base(domino.FirstNumber, domino.SecondNumber)
        {
        }

        public int RightNumber
        {
            get
            {
                var buf = RightDominoNode;
                return buf._linkedBySecondNumber ? buf._firstNumber : buf._secondNumber;
            }
        }

        public DominoNode RightDominoNode
        {
            get
            {
                var body = this;
                while (body.Next != null)
                {
                    body = body.Next;
                }
                return body;
            }
        }

        public DominoNode LeftDominoNode
        {
            get
            {
                var body = this;
                while (body.Previous != null)
                {
                    body = body.Previous;
                }
                return body;
            }
        }

        public int LeftNumber
        {
            get
            {
                var buf = LeftDominoNode;
                return buf._linkedByFirstNumber ? buf._secondNumber : buf._firstNumber;
            }
        }

        public int NextNumber
        {
            get
            {
                _rightNumber = !_rightNumber;
                return _rightNumber ? RightNumber : LeftNumber;
            }
        }

        public DominoNode Next { get; private set; }
        public DominoNode Previous { get; private set; }

        public bool TryAdd(DominoNode node)
        {
            if (!TryLinkByLeftSide(node))
                if (!TryLinkByRightSide(node))
                    return false;
            return true;
        }

        private bool TryLinkByLeftSide(DominoNode node)
        {
            var leftDominoNode = LeftDominoNode;
            if (!leftDominoNode.TryLinkByFirstNumber(node))
                if (!leftDominoNode.TryLinkBySecondNumber(node))
                    return false;
            leftDominoNode.LeftAdd(node);
            return true;
        }

        private bool TryLinkByRightSide(DominoNode node)
        {
            var rightDominoNode = RightDominoNode;
            if (!rightDominoNode.TryLinkByFirstNumber(node))
                if (!rightDominoNode.TryLinkBySecondNumber(node))
                    return false;
            rightDominoNode.RightAdd(node);
            return true;
        }

        private bool TryLinkByFirstNumber(DominoNode node)
        {
            if (_linkedByFirstNumber) return false;
            if (_firstNumber == node._firstNumber)
            {
                _linkedByFirstNumber = node._linkedByFirstNumber = true;
                return true;
            }

            if (_firstNumber != node._secondNumber) return false;

            _linkedByFirstNumber = node._linkedBySecondNumber = true;
            return true;
        }

        private bool TryLinkBySecondNumber(DominoNode node)
        {
            if (_linkedBySecondNumber) return false;
            if (_secondNumber == node._firstNumber)
            {
                _linkedBySecondNumber = node._linkedByFirstNumber = true;
                return true;
            }

            if (_firstNumber != node._secondNumber) return false;

            _linkedBySecondNumber = node._linkedBySecondNumber = true;
            return true;
        }

        private void LeftAdd(DominoNode node)
        {
            if (Previous == null)
            {
                Previous = node;
                node.Next = this;
            }
        }

        private void RightAdd(DominoNode node)
        {
            if (Next == null)
            {
                Next = node;
                node.Previous = this;
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", _firstNumber, _secondNumber);
        }
    }

    public class DominoStack
    {
        private readonly List<DominoNode> _dominoes;

        public DominoStack()
        {
            _dominoes = new List<DominoNode>();
        }

        public bool IsEmpty
        {
            get { return _dominoes.Count == 0; }
        }

        public void Push(DominoNode domino)
        {
            _dominoes.Add(domino);
        }

        public DominoNode Pop()
        {
            var buff = _dominoes[_dominoes.Count - 1];
            _dominoes.RemoveAt(_dominoes.Count - 1);
            return buff;
        }

        public DominoNode InteligencePop(int number)
        {
            var buff = _dominoes.First(dom => dom.FirstNumber == number || dom.SecondNumber == number);
            _dominoes.Remove(buff);
            return buff;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = Console.ReadLine().Split(' ');
            var stack = new DominoStack();

            foreach (var str in input)
            {
                stack.Push(new DominoNode(int.Parse(str[0].ToString()), int.Parse(str[1].ToString())));
            }
            Domino headDomino = stack.Pop();
            var head = new DominoNode(headDomino);
            var body = head;
            var chanse = false;
            while (!stack.IsEmpty)
            {
                try
                {
                    head.TryAdd(stack.InteligencePop(head.NextNumber));
                    chanse = false;
                }
                catch (InvalidOperationException)
                {
                    if (chanse)
                        break;

                    chanse = !chanse;
                }
            }


            if (head.LeftNumber == head.RightNumber && stack.IsEmpty)
            {
                Console.WriteLine("Цепочку возможно создать");

                body = body.RightDominoNode;
                while (body.Previous != null)
                {
                    Console.WriteLine(body.ToString());
                    body = body.Previous;
                }
                Console.WriteLine(body.ToString());
            }
            else
            {
                Console.WriteLine("Цепочку невозможно создать");
            }

            Console.ReadKey();
        }
    }
}