using System;

namespace TradeAdvisor.Test
{
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
    }
}
