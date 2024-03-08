using System.Collections.Generic;

namespace ProjectManager.Domain.Utils.Expressions
{
    public sealed class FilterGroup : IFilter
    {
        public List<Filter> Filters { get; set; }
    }
}
