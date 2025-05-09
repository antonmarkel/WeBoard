using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Base.Comparers;

namespace WeBoard.Core.Collections
{
    public class ZIndexComponentSortedSet
    {
        private readonly SortedSet<ComponentBase> _sortedSet;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly ComponentBaseComparer _comparer = new ComponentBaseComparer();

        public int Count { get => _sortedSet.Count; }
        public ComponentBase? Last { get =>  _sortedSet.Count == 0 ? null : _sortedSet.Last(); }
        public ComponentBase? First { get => _sortedSet.Count == 0 ? null : _sortedSet.First(); }
        public ZIndexComponentSortedSet()
        {
            _sortedSet = new SortedSet<ComponentBase>(_comparer);
        }

        public void Add(ComponentBase component)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_sortedSet.Add(component))
                {
                    component.ZIndexChanged += OnZIndexChanged;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Remove(ComponentBase component)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_sortedSet.Remove(component))
                {
                    component.ZIndexChanged -= OnZIndexChanged;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private void OnZIndexChanged(ComponentBase component)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_sortedSet.Remove(component))
                {
                    _sortedSet.Add(component);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public IEnumerable<ComponentBase> GetComponentsAscending()
        {
            _lock.EnterReadLock();
            try
            {
                foreach (var component in _sortedSet)
                {
                    yield return component;
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerable<ComponentBase> GetComponentsDescending()
        {
            _lock.EnterReadLock();
            try
            {
                var list = new List<ComponentBase>(_sortedSet);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    yield return list[i];
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

}
