using System;

namespace Dominoes
{
    public class DominoNode : Domino, IEquatable<DominoNode>
    {
        private int _count;
        private int _doublicateCounter;
        private bool _innerConnection;
        private bool _linkedBySecondNumber;
        private bool _reverse;

        public DominoNode()
        {
            RightDominoNode = this;
        }

        public DominoNode(char x, char y)
            : base(x, y)
        {
            RightDominoNode = this;
        }

        public DominoNode(DominoNode domino)
            : base(domino)
        {
            RightDominoNode = this;
        }

        public char NextNumber
        {
            get
            {
                if (_reverse)
                {
                    var body = this;
                    while (body.Previous != null)
                    {
                        body = body.Previous;
                    }
                    var result = '\0';
                    if (_isDublicate)
                    {
                        if (!body._linkedBySecondNumber)
                            result = body._secondNumber;
                        if (!body.LinkedByFirstNumber)
                            result = body._firstNumber;
                    }
                    else
                    {
                        if (body._linkedBySecondNumber)
                            result = body._secondNumber;
                        if (body.LinkedByFirstNumber)
                            result = body._firstNumber;
                    }

                    return result;
                }
                else
                {
                    var buf = RightDominoNode;
                    var result = '\0';
                    if (_isDublicate)
                    {
                        if (buf._linkedBySecondNumber)
                            result = buf._secondNumber;
                        if (buf.LinkedByFirstNumber)
                            result = buf._firstNumber;
                    }
                    else
                    {
                        if (!buf._linkedBySecondNumber)
                            result = buf._secondNumber;
                        if (!buf.LinkedByFirstNumber)
                            result = buf._firstNumber;
                    }

                    return result;
                }
            }
        }

        public DominoNode RightDominoNode { get; internal set; }
        public bool LinkedByFirstNumber { get; private set; }
        public DominoNode Next { get; private set; }
        public DominoNode Previous { get; private set; }

        public bool Equals(DominoNode other)
        {
            return _firstNumber == other._firstNumber && _secondNumber == other._secondNumber;
        }

        public void ReverseLink()
        {
            _reverse = true;
            RightDominoNode = this;
            //var localCount = (_count / 2);
            //var endNode = this.RightDominoNode;
            //var begindNode = this;
            //while (localCount-- > 0)
            //{
            //    SwapNodes(begindNode, endNode);
            //    begindNode = begindNode.Next;
            //    endNode = endNode.Previous;
            //}
        }

        private static void SwapNodes(DominoNode thisNode, DominoNode other)
        {
            var bufChar = thisNode._firstNumber;
            thisNode._firstNumber = other._firstNumber;
            other._firstNumber = bufChar;

            bufChar = thisNode._secondNumber;
            thisNode._secondNumber = other._secondNumber;
            other._secondNumber = bufChar;

            var bufBool = thisNode.LinkedByFirstNumber;
            thisNode.LinkedByFirstNumber = other.LinkedByFirstNumber;
            other.LinkedByFirstNumber = bufBool;

            bufBool = thisNode._linkedBySecondNumber;
            thisNode._linkedBySecondNumber = other._linkedBySecondNumber;
            other._linkedBySecondNumber = bufBool;

            var bufInt = thisNode.NumberOfDominos;
            thisNode.NumberOfDominos = other.NumberOfDominos;
            other.NumberOfDominos = bufInt;

            //var bufNode = other.Previous;
            //other.Previous = thisNode.Previous;
            //thisNode.Previous = bufNode;

            //bufNode = other.Next;
            //other.Next = thisNode.Next;
            //thisNode.Next = bufNode;

            //bufNode = other.RightDominoNode;
            //other.RightDominoNode = thisNode.RightDominoNode;
            //thisNode.RightDominoNode = bufNode;
        }

        public bool TryAdd(DominoNode node)
        {
            return TryLink(node);
        }

        public bool TryConnectByValue(int number, DominoNode node)
        {
            var rightDominoNode = RightDominoNode;
            var linkedNode = node.RightDominoNode != node ? node.RightDominoNode : node;
            if (number == _firstNumber && rightDominoNode.TryLinkInnerByFirstNumber(node) ||
                number == _secondNumber && rightDominoNode.TryLinkInnerBySecondNumber(node))
            {
                rightDominoNode.RightAdd(node);
                RightDominoNode = linkedNode;
                return true;
            }
            return false;
        }

        private bool TryLink(DominoNode node)
        {
            var rightDominoNode = RightDominoNode;
            var linkedNode = node.RightDominoNode != node ? node.RightDominoNode : node;
            if (!rightDominoNode.TryLinkByFirstNumber(linkedNode))
                if (!rightDominoNode.TryLinkBySecondNumber(linkedNode))
                    return false;

            rightDominoNode.RightAdd(node);

            RightDominoNode = linkedNode;
            return true;
        }

        private bool TryLinkByFirstNumber(DominoNode node)
        {
            if (LinkedByFirstNumber) return false;
            if (!node.LinkedByFirstNumber && _firstNumber == node._firstNumber)
            {
                LinkedByFirstNumber = true;
                node.LinkedByFirstNumber = true;
                return true;
            }

            if (!node._linkedBySecondNumber && _firstNumber != node._secondNumber) return false;
            LinkedByFirstNumber = true;
            node._linkedBySecondNumber = true;

            return true;
        }

        private bool TryLinkBySecondNumber(DominoNode node)
        {
            if (_linkedBySecondNumber) return false;
            if (!node.LinkedByFirstNumber && _secondNumber == node._firstNumber)
            {
                _linkedBySecondNumber = true;
                node.LinkedByFirstNumber = true;
                return true;
            }

            if (!node._linkedBySecondNumber && _secondNumber != node._secondNumber) return false;
            _linkedBySecondNumber = true;
            node._linkedBySecondNumber = true;
            return true;
        }

        private bool TryLinkInnerByFirstNumber(DominoNode node)
        {
            if (LinkedByFirstNumber &&
                (NumberOfDominos/2 < _count || (FirstNumber == SecondNumber && NumberOfDominos/2 + 1 < _count)))
                return false;
            if (_firstNumber == node._firstNumber)
            {
                node.LinkedByFirstNumber = true;
            }
            else if (_firstNumber == node._secondNumber)
            {
                node._linkedBySecondNumber = true;
            }
            else
                return false;


            LinkedByFirstNumber = true;
            _count++;
            return true;
        }

        private bool TryLinkInnerBySecondNumber(DominoNode node)
        {
            if (LinkedByFirstNumber &&
                (NumberOfDominos/2 < _count || (FirstNumber == SecondNumber && NumberOfDominos/2 + 1 < _count)))
                return false;
            if (!node.LinkedByFirstNumber && _secondNumber == node._firstNumber)
            {
                if (_isDublicate && _doublicateCounter < NumberOfDominos/2)
                {
                    _doublicateCounter++;
                }
                else
                {
                    _linkedBySecondNumber = true;
                }
                if (node._isDublicate && node._doublicateCounter < node.NumberOfDominos/2)
                {
                    node._doublicateCounter++;
                }
                else
                {
                    node.LinkedByFirstNumber = true;
                }
                return true;
            }

            if (!node._linkedBySecondNumber && _secondNumber != node._secondNumber) return false;

            if (_isDublicate && _doublicateCounter < NumberOfDominos/2)
            {
                _doublicateCounter++;
            }
            else
            {
                _linkedBySecondNumber = true;
            }
            if (node._isDublicate && node._doublicateCounter < node.NumberOfDominos/2)
            {
                node._doublicateCounter++;
            }
            else
            {
                node._linkedBySecondNumber = true;
            }
            return true;
        }

        private void RightAdd(DominoNode node)
        {
            if (_reverse) LeftAdd(node);
            else
            {
                if (Next == null)
                {
                    Next = node;
                    node.Previous = this;
                }
            }
        }

        private void LeftAdd(DominoNode node)
        {
            if (Next == null)
            {
                Previous = node;
                node.Next = this;
            }
        }

        public bool TryInnerAdd(DominoNode other, bool connectByFirstNumber)
        {
            if (NumberOfDominos%2 == 1) return false;

            var isConnected = connectByFirstNumber
                ? TryLinkInnerByFirstNumber(other)
                : TryLinkInnerBySecondNumber(other);

            if (isConnected)
            {
                var endNode = new DominoNode(this);
                endNode.SetNumberOfDominos(NumberOfDominos/2);
                NumberOfDominos = NumberOfDominos/2;
                Next.Previous = endNode;
                _count = 1;
                endNode.Next = Next;
                Next = null;
                RightAdd(other);
                other.RightAdd(endNode);
                if (NumberOfDominos == 1)
                {
                    _isDublicate = endNode._isDublicate = false;
                }
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var result = string.Empty;
            for (var i = 0; i < NumberOfDominos; i++)
            {
                result += string.Format("({0},{1})\n", _firstNumber, _secondNumber);
            }
            return result;
        }

        public string DebugToString()
        {
            var result = string.Empty;
            var body = this;
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