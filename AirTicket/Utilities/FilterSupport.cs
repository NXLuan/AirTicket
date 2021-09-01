using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AirTicket.Utilities
{
    class FilterSupport
    {
        private Dictionary<string, Predicate<object>> filters;
        private CollectionView _filterView;
        public CollectionView FilterView
        {
            get => _filterView;
            set
            {
                _filterView = value;
                _filterView.Filter = FilterCandidates;
            }
        }
        public FilterSupport()
        {
            filters = new Dictionary<string, Predicate<object>>();
        }

        private bool FilterCandidates(object obj)
        {
            return filters.Values
                .Aggregate(true,
                    (prevValue, predicate) => prevValue && predicate(obj));
        }


        public void ClearFilters()
        {
            filters.Clear();
            FilterView.Refresh();
        }

        public void RemoveFilter(string filterName)
        {
            if (filters.Remove(filterName))
            {
                FilterView.Refresh();
            }
        }

        public void AddFilter(string name, Predicate<object> predicate)
        {
            if (filters.ContainsKey(name)) filters[name] = predicate;
            else
                filters.Add(name, predicate);
            FilterView.Refresh();
        }
    }
}
