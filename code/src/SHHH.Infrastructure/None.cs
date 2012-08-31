
namespace SHHH.Infrastructure
{
    public class None<T>: Option<T> where T: class
    {
        public None() : base(default(T)) { IsDefined = false; }

        public bool IsDefined { get; set; }
    }
}
