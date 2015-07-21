using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Configuration;

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
            get { return _firstNumber; }
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
        private bool _side;

        public DominoNode()
        {
            LeftDominoNode = RightDominoNode = this;
        }

        public DominoNode(int x, int y)
            : base(x, y)
        {
            LeftDominoNode = RightDominoNode = this;
        }

        public DominoNode(Domino domino)
            : base(domino.FirstNumber, domino.SecondNumber)
        {
            LeftDominoNode = RightDominoNode = this;
        }

        public int RightNumber
        {
            get
            {
                var buf = RightDominoNode;
                var result = 0;
                if (!buf._linkedBySecondNumber)
                    result = buf._secondNumber;
                if (!buf._linkedByFirstNumber)
                    result = buf._firstNumber;

                return result;
            }
        }

        public DominoNode RightDominoNode { get; private set; }

        public DominoNode LeftDominoNode { get; private set; }

        public int LeftNumber
        {
            get
            {
                var buf = LeftDominoNode;
                var result = 0;

                if (!buf._linkedByFirstNumber)
                    result = buf._firstNumber;
                if (!buf._linkedBySecondNumber)
                    result = buf._secondNumber;
                return result;
            }
        }

        public int CurrentNumber
        {
            get { return _side ? RightNumber : LeftNumber; }
        }

        public DominoNode Next { get; private set; }
        public DominoNode Previous { get; private set; }

        public int GetNextNumber()
        {
            _side = !_side;
            return _side ? RightNumber : LeftNumber;
        }

        public bool TryAdd(DominoNode node)
        {
            return _side ? TryLinkByRightSide(node) : TryLinkByLeftSide(node);
            //if (!TryLinkByLeftSide(node))
            //    if (!TryLinkByRightSide(node))
            //        return false;
            //    return true;
        }

        private bool TryLinkByLeftSide(DominoNode node)
        {
            var leftDominoNode = LeftDominoNode;
            if (!leftDominoNode.TryLinkByFirstNumber(node))
                if (!leftDominoNode.TryLinkBySecondNumber(node))
                    return false;
            leftDominoNode.LeftAdd(node);
            LeftDominoNode = node;
            return true;
        }

        private bool TryLinkByRightSide(DominoNode node)
        {
            var rightDominoNode = RightDominoNode;
            if (!rightDominoNode.TryLinkByFirstNumber(node))
                if (!rightDominoNode.TryLinkBySecondNumber(node))
                    return false;
            rightDominoNode.RightAdd(node);
            RightDominoNode = node;
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

            if (_secondNumber != node._secondNumber) return false;

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

    public class DominoNodeStack
    {
        private readonly List<DominoNode> _dominoes;

        public DominoNodeStack()
        {
            _dominoes = new List<DominoNode>();
        }

        public List<DominoNode> GetList()
        {
            return _dominoes;
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
        private static bool QuickCheck(List<DominoNode> list)
        {
            var query1 = list.Select(dmn => dmn.FirstNumber).Distinct().ToList();
            var query2 = list.Select(dmn => dmn.SecondNumber).Distinct().ToList();
            if (query1.Count() != query2.Count())
                return false;
            query2.Sort();
            query1.Sort();
            if (query1.Where((el, i) => el != query2[i]).Count() != 0)
                return false;


            Dictionary<int, bool> dict = query1.ToDictionary(i => i, i => false);
            foreach (var dominoNode in list)
            {
                dict[dominoNode.FirstNumber] = !dict[dominoNode.FirstNumber];
                dict[dominoNode.SecondNumber] = !dict[dominoNode.SecondNumber];

            }
            return !dict.Any(b => b.Value);
        }

        private static void Main(string[] args)
        {
            var initChain= "34 54 32 12 64 45";
            var dominoBuilder = new DominoBuilder(initChain);

            Debug.Assert(dominoBuilder.Dominos.Count == 6);

            dominoBuilder.Sort();
            Debug.Assert(dominoBuilder.Dominos.First().FirstNumber == 1 || dominoBuilder.Dominos.Last().SecondNumber == 1);
            Debug.Assert(dominoBuilder.ToString() == "12 23 34 46 64 45");


            var rnd = new Random();
            var input = Console.ReadLine().Split(' ');
            var stack = new DominoNodeStack();

            foreach (var str in input)
            {
                stack.Push(new DominoNode(int.Parse(str[0].ToString()), int.Parse(str[1].ToString())));
            }
            if (QuickCheck(stack.GetList()))
            {
                var head = new DominoNode(stack.Pop());

                var chanse = false;
                while (!stack.IsEmpty)
                {
                    try
                    {
                        if (head.TryAdd(stack.InteligencePop(head.GetNextNumber())))
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
                    var body = head;
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

            }
            else
            {
                Console.WriteLine("Цепочку невозможно создать");
            }
            Console.ReadKey();
        }
    }
}