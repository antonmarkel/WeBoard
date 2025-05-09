namespace WeBoard.Core.Components.Base.Comparers
{
    class ComponentBaseComparer : IComparer<ComponentBase>
    {
        public int Compare(ComponentBase a, ComponentBase b)
        {
            int zCompare = a.ZIndex.CompareTo(b.ZIndex);
            return zCompare != 0 ? zCompare : 1;
        }
    }
}
