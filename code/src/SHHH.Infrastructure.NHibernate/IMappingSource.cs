using System.Collections.Generic;
using System.Reflection;

namespace SHHH.Infrastructure.NHibernate
{
    public interface IMappingSource
    {
        IEnumerable<Assembly> MappingSources();
    }
}
