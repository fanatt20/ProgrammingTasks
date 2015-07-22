using System;

namespace Dominoes
{
    internal class DominoBuilder
    {
        private readonly string _initChain;
        private readonly AnotherDominoNodeStack _nodeStack;

        public DominoBuilder(string initChain)
        {
            _initChain = initChain;
            var input = _initChain.Split(' ');
            _nodeStack = new AnotherDominoNodeStack();

            foreach (var str in input)
            {
                if (str[0] >= '0' || str[0] <= '6' || str[1] >= '0' || str[1] <= '6')

                    _nodeStack.Push(new DominoNode(str[0], str[1]));
            }
        }

        public DominoNode Dominos { get; private set; }

        internal void Sort()
        {
            _nodeStack.Sort();

            var head = new DominoNode(_nodeStack.Pop());

            var chanse = false;
            var oneTimeReverse = true;
            DominoNode buffer = null;

            while (!_nodeStack.IsEmpty)
            {
                buffer = _nodeStack.InteligencePop(head.NextNumber);
                if (buffer != null)
                {
                    if (head.TryAdd(buffer))
                        chanse = false;
                }
                else
                {
                    if (chanse)
                        break;
                    chanse = true;
                    if (oneTimeReverse)
                    {
                        Dominos = head;
                        Console.WriteLine("Before Reverse:\n{0}", this);

                        head.ReverseLink();
                        Console.WriteLine("After Reverse:\n{0}", this);
                        oneTimeReverse = false;
                    }
                }
            }
            Dominos = head;
        }

        public override string ToString()
        {
            if (Dominos == null)
                return "Dominoes.DominoBuilder";
            var result = string.Empty;
            var body = Dominos;
            while (body.Previous != null)
            {
                body = body.Previous;
            }
            while (body.Next != null)
            {
                result += body + "\n";
                body = body.Next;
            }
            result += body + "\n";
            return result;
        }
    }
}