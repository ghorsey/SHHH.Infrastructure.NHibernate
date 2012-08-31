
namespace SHHH.Infrastructure
{
    public abstract class Option<T> where T : class
    {
        public Option(T obj)
        {
            this.Object = obj;
        }

        public T Object { get; private set; }
        public bool IsDefined { get; protected set; }

        public static Option<T> MakeOption(T obj)
        {
            if (default(T) == obj)
                return new None<T>();
            else
                return new Some<T>(obj);
        }

        public static implicit operator Option<T>(T obj)
        {
            return new Some<T>(obj);
        }
    }
}
