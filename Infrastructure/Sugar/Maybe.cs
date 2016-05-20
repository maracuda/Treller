using System;
using System.Collections.Generic;

namespace SKBKontur.Infrastructure.Sugar
{
    public struct Maybe<T>
    {
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(value);
        }

        private readonly T value;

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new ArgumentNullException();
                }
                return value;
            }
        }

        public bool HasValue => value != null;
        public bool HasNoValue => !HasValue;

        private Maybe(T value)
        {
            this.value = value;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
                return false;

            var maybe = (Maybe<T>)obj;

            if (HasNoValue && maybe.HasNoValue)
                return true;

            if (HasNoValue || maybe.HasNoValue)
                return false;

            return Value.Equals(maybe.Value);
        }
    }
}