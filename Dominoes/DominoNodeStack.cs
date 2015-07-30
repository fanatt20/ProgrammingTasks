using System.Collections.Generic;
using System.Linq;

namespace Dominoes
{
    public class DominoNodeStack
    {
        private readonly List<DominoNode> _dominos;
        private readonly List<DominoNode> _dublicateDominos;

        public DominoNodeStack()
        {
            _dominos = new List<DominoNode>();
            _dublicateDominos = new List<DominoNode>();
        }

        public bool IsEmpty
        {
            get { return _dominos.Count == 0; }
        }

        public List<DominoNode> GetList()
        {
            return _dominos.Union(_dublicateDominos.AsEnumerable()).ToList();
        }

        public void Push(DominoNode domino)
        {
            _dominos.Add(domino);
        }

        public DominoNode Pop()
        {
            var buff = _dominos[_dominos.Count - 1];
            _dominos.RemoveAt(_dominos.Count - 1);
            return buff;
        }

        public DominoNode InteligencePop(int number)
        {
            var buff = _dominos.First(dom => dom.FirstNumber == number || dom.SecondNumber == number);
            _dominos.Remove(buff);
            return buff;
        }
    }

    public class AnotherDominoNodeStack
    {
        private readonly List<DominoNode> _dominos;
        private readonly List<DominoNode> _dublicateDominos;

        public AnotherDominoNodeStack()
        {
            _dominos = new List<DominoNode>();
            _dublicateDominos = new List<DominoNode>();
        }

        public bool IsEmpty
        {
            get { return (_dominos.Count + _dublicateDominos.Count) == 0; }
        }

        public List<DominoNode> GetList()
        {
            return _dominos.Union(_dublicateDominos.AsEnumerable()).ToList();
        }

        public void Push(DominoNode domino)
        {
            if (domino.FirstNumber == domino.SecondNumber)
                if (_dublicateDominos.Contains(domino))
                    _dublicateDominos.First(dom => dom.FirstNumber == domino.FirstNumber).AddDominos();
                else
                    _dublicateDominos.Add(domino);
            else if (_dominos.Contains(domino))
                _dominos.First(dom => dom.FirstNumber == domino.FirstNumber && dom.SecondNumber == domino.SecondNumber)
                    .AddDominos();
            else
                _dominos.Add(domino);
        }

        public void Sort()
        {
            DominoNode bufferDomino;
            foreach (var domino in _dominos.Where(domino => domino.NumberOfDominos/2 != 0))
            {
                if (domino.NumberOfDominos%2 == 0)
                {
                    domino.SetNumberOfDominos(domino.NumberOfDominos);
                    _dublicateDominos.Add(domino);
                }
                else
                {
                    bufferDomino = new DominoNode(domino.FirstNumber, domino.SecondNumber);
                    bufferDomino.SetNumberOfDominos(domino.NumberOfDominos - 1);
                    _dublicateDominos.Add(bufferDomino);
                    domino.Transform();
                }
            }
            foreach (var dublicateDomino in _dublicateDominos)
            {
                dublicateDomino.SetDominoIsDublicate();
            }
            _dominos.RemoveAll(dom => dom.NumberOfDominos%2 == 0);
        }

        public DominoNode Pop()
        {
            var buff = _dominos[_dominos.Count - 1];
            _dominos.RemoveAt(_dominos.Count - 1);
            return buff;
        }

        public DominoNode InteligencePop(char number)
        {
            DominoNode buff = null;
            if (_dublicateDominos.Any(dom => dom.FirstNumber == number || dom.SecondNumber == number))
            {
                buff = _dublicateDominos.First(dom => dom.FirstNumber == number || dom.SecondNumber == number);
                _dublicateDominos.Remove(buff);

                if (CheckForOtherDublicates(buff, number))
                {
                    while (_dublicateDominos.Any(dom => dom.FirstNumber == number || dom.SecondNumber == number))
                    {
                        var nextDublicate =
                            _dublicateDominos.First(
                                dom => dom.FirstNumber == number || dom.SecondNumber == number);
                        if (buff.TryConnectByValue(number, nextDublicate))
                            _dublicateDominos.Remove(nextDublicate);
                    }

                    var body = buff;
                    do
                    {
                        var connectedByFirstNumber = body.LinkedByFirstNumber;

                        while (body.NumberOfDominos%2 == 0 &&
                               _dublicateDominos.Any(
                                   dom =>
                                       dom.FirstNumber ==
                                       (connectedByFirstNumber ? body.SecondNumber : body.FirstNumber) ||
                                       dom.SecondNumber ==
                                       (connectedByFirstNumber ? body.SecondNumber : body.FirstNumber)))
                        {
                            var innerAdd = _dublicateDominos.First(
                                dom =>
                                    dom.FirstNumber == (connectedByFirstNumber ? body.SecondNumber : body.FirstNumber) ||
                                    dom.SecondNumber == (connectedByFirstNumber ? body.SecondNumber : body.FirstNumber));

                            if (body.TryInnerAdd(innerAdd, !connectedByFirstNumber))
                                _dublicateDominos.Remove(innerAdd);
                            else
                                break;
                        }

                        body = body.Next;
                    } while (body != null && body.Next != null);
                }
            }
            else if (_dominos.Any(dom => dom.FirstNumber == number || dom.SecondNumber == number))
            {
                buff = _dominos.First(dom => dom.FirstNumber == number || dom.SecondNumber == number);
                _dominos.Remove(buff);
            }
            _dominos.RemoveAll(dom => dom == null);
            _dublicateDominos.RemoveAll(dom => dom == null);
            return buff;
        }

        private bool TryInsertDublicate(DominoNode domino, int outerNumber)
        {
            return true;
        }

        private bool CheckForOtherDublicates(DominoNode domino, int number)
        {
            var connectedByFirstNumber = domino.FirstNumber == number;
            return
                _dublicateDominos.Any(
                    dom =>
                        (dom.FirstNumber == number || dom.SecondNumber == number) ||
                        ((connectedByFirstNumber ? dom.SecondNumber : dom.FirstNumber) == domino.SecondNumber));
        }

        private bool SimpleCheckForDublicates(DominoNode domino1, DominoNode domino2, int number)
        {
            var connectedByFirstNumber = domino2.FirstNumber == number;
            return
                (domino1.FirstNumber == number || domino1.SecondNumber == number) ||
                ((connectedByFirstNumber ? domino1.SecondNumber : domino1.FirstNumber) == domino2.SecondNumber);
        }
    }
}