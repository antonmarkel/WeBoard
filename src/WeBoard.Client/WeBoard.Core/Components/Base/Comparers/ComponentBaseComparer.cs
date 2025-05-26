namespace WeBoard.Core.Components.Base.Comparers
{
    class ComponentBaseComparer : IComparer<ComponentBase>
    {
        public int Compare(ComponentBase a, ComponentBase b)
        {
            if (a == null || b == null) return 0;

            int zCompare = a.ZIndex.CompareTo(b.ZIndex);
            if (zCompare != 0)
                return zCompare;

            return a.Id.CompareTo(b.Id);
        }
    }
}
