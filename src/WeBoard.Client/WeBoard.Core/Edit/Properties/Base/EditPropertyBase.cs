namespace WeBoard.Core.Edit.Properties.Base
{
    public abstract class EditPropertyBase
    {
        public EditPropertyBase(string name, Type valueType)
        {
            Name = name;
            ValueType = valueType;
        }

        public string Name { get; }
        public Type ValueType { get; set; }
    }
}
