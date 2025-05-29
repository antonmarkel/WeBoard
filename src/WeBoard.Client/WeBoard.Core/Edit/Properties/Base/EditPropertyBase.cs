namespace WeBoard.Core.Edit.Properties.Base
{
    public abstract class EditPropertyBase
    {
        private readonly Action<object> _setterObj;
        private readonly Func<object> _getterObj;
        public EditPropertyBase(string name, Type valueType, Action<object> setterObj, Func<object> getterObj)
        {
            Name = name;
            ValueType = valueType;
            _setterObj = setterObj;
            _getterObj = getterObj;
        }

        public string Name { get; }
        public Type ValueType { get; set; }

        public void UpdateValue(object value) => _setterObj(value);
        public object GetValue() => _getterObj();
    }
}
