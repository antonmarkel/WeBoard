namespace WeBoard.Core.Edit.Properties.Base
{
    public class EditProperty<T> : EditPropertyBase
    {
        private readonly Action<T> _setter;
        private readonly Func<T> _getter;
        public EditProperty(string name, Action<T> setter, Func<T> getter) :
            base(name, typeof(T),
                obj => setter((T)obj),
                () => getter()!)
        {
            _setter = setter;
            _getter = getter;
        }

        public virtual void UpdateValue(T value) => _setter(value);
        public virtual T GetValue() => _getter();
    }
}
