using System;

namespace TradeAdvisor.Test
{
    public static class Scoped
    {
        public static Scoped<T> Create<T>(T value, Action dispose) => 
            new Scoped<T>(value, dispose);
    }

    public class Scoped<T> : IDisposable
    {
        public readonly T Value;
        readonly Action dispose;

        public Scoped(T value, Action dispose)
        {
            Value = value;
            this.dispose = dispose;
        }

        public void Dispose() => dispose?.Invoke();

        public static implicit operator T(Scoped<T> value) => value.Value;
    }
}
