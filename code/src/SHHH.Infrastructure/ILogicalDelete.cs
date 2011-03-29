using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHHH.Infrastructure
{
    public interface ILogicalDelete
    {
        bool IsDeleted { get; set; }
    }
}
