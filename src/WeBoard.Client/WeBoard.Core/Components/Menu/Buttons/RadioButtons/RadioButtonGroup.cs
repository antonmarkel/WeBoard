namespace WeBoard.Core.Components.Menu.Buttons.RadioButtons
{
    public class RadioButtonGroup
    {
        public event Action<RadioButtonComponent> OnGroupUpdateHandler;

        private List<RadioButtonComponent> _radioButtons = [];
        public void AddButton(RadioButtonComponent radioButton)
        {
            if (!_radioButtons.Contains(radioButton))
                _radioButtons.Add(radioButton);
        }

        public void AddButtonRange(IEnumerable<RadioButtonComponent> buttons)
        {
            _radioButtons.AddRange(buttons);
        }

        public void SetOption(RadioButtonComponent radioButton)
        {
            if (!_radioButtons.Contains(radioButton))
                return;

            OnGroupUpdateHandler?.Invoke(radioButton);

            foreach (var button in _radioButtons)
            {
                button.OnGroupClick(button == radioButton);
            }
        }
    }
}
