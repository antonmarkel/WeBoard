using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Menu.Containers;
using WeBoard.Core.Edit;
using WeBoard.Core.Edit.Base;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Client.Services.Managers
{
    public class EditManager
    {
        private static EditManager? Instance;
        public VerticalStackContainer? CurrentEditContainer { get; private set; }
 
        public EditManager()
        {
          
        }
        public static EditManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        public void UpdateEditPanel(IEditable editable,Vector2f position)
        {
            var editList = new List<EditBase>();
            foreach (var editProperty in editable.EditProperties)
            {
                if (editProperty.ValueType == typeof(int))
                    editList.Add(new NumberEdit((EditProperty<int>)editProperty));
                if (editProperty.ValueType == typeof(Color))
                    editList.Add(new ColorEdit((EditProperty<Color>)editProperty));
            }
            CurrentEditContainer?.Hide();
            CurrentEditContainer = new VerticalStackContainer(editList)
            {
                BackgroundColor = new Color(255, 255, 255, 120),
                Padding = new(5,5),
                SpaceBetween = 20,
                Position = position
            };
        }

    }
}
