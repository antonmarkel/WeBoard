using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Base.Comparers;

namespace WeBoard.Core.Collections
{
    public class ZIndexComponentSortedSet<TComponent>
        where TComponent : ComponentBase
    {
        private readonly SortedSet<TComponent> _sortedSet;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly ComponentBaseComparer _comparer = new ComponentBaseComparer();

        public int Count { get => _sortedSet.Count; }
        public TComponent? Last { get => _sortedSet.Count == 0 ? null : _sortedSet.Last(); }
        public TComponent? First { get => _sortedSet.Count == 0 ? null : _sortedSet.First(); }
        public ZIndexComponentSortedSet()
        {
            _sortedSet = new SortedSet<TComponent>(_comparer);
        }

        public void Add(TComponent component)
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

        public void Remove(TComponent component)
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
                if (_sortedSet.Remove((TComponent)component))
                {
                    _sortedSet.Add((TComponent)component);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public IEnumerable<TComponent> GetComponentsAscending()
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

        public IEnumerable<TComponent> GetComponentsDescending()
        {
            _lock.EnterReadLock();
            try
            {
                var list = new List<TComponent>(_sortedSet);
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
