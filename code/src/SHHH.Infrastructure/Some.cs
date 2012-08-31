
namespace SHHH.Infrastructure
{
    public class Some<T> : Option<T> where T: class
    {
        public Some(T obj) 
            : base(obj)
        {
            IsDefined = (obj != null);
        }
    }
}
