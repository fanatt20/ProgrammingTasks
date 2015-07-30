using System;

namespace Dominoes
{
    public class Domino : IEquatable<Domino>
    {
        protected char _firstNumber;
        protected bool _isDublicate;
        protected char _secondNumber;

        public Domino(char firstNumber, char secondNumber)
        {
            if (firstNumber >= secondNumber)
            {
                _firstNumber = firstNumber;
                _secondNumber = secondNumber;
            }
            else
            {
                _firstNumber = secondNumber;
                _secondNumber = firstNumber;
            }
            NumberOfDominos = 1;
        }

        protected Domino(Domino domino)
        {
            _firstNumber = domino._firstNumber;
            _secondNumber = domino._secondNumber;
            _isDublicate = domino._isDublicate;
            NumberOfDominos = domino.NumberOfDominos;
        }

        protected Domino()
        {
        }

        public char FirstNumber
        {
            get { return _firstNumber; }
        }

        public char SecondNumber
        {
            get { return _secondNumber; }
        }

        public int NumberOfDominos { get; protected set; }

        public bool Equals(Domino other)
        {
            return FirstNumber == other.FirstNumber && SecondNumber == other.SecondNumber;
        }

        public void AddDominos()
        {
            NumberOfDominos++;
        }

        public void Transform()
        {
            NumberOfDominos = NumberOfDominos%2;
        }

        public void SetNumberOfDominos(int num)
        {
            NumberOfDominos = num;
        }

        public void SetDominoIsDublicate()
        {
            _isDublicate = true;
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
    }

    public class SpecialDublicateDomino : Domino
    {
        public SpecialDublicateDomino(char firstNumber, char secondNumber) : base(firstNumber, secondNumber)
        {
        }
    }
}