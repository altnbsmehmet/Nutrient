using System;

namespace Model
{

    public struct TwoDecimalPlaces
    {
        private decimal _value;

        public TwoDecimalPlaces(decimal value)
        {
            _value = Math.Round(value, 2);
        }

        public decimal Value
        {
            get => _value;
        }

        public static implicit operator TwoDecimalPlaces(decimal value)
        {
            return new TwoDecimalPlaces(value);
        }

        public static implicit operator decimal(TwoDecimalPlaces twoDecimalPlaces)
        {
            return twoDecimalPlaces._value;
        }

        public override string ToString()
        {
            return _value.ToString("F2");
        }
    }

}