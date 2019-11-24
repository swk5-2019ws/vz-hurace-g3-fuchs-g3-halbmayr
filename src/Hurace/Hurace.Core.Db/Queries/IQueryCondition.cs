using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public interface IQueryCondition
    {
        void Build(StringBuilder queryBuilder);
    }
}
