using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Navigation
{
    /// <summary>
    /// Represents a button that triggers an action when clicked, providing the configured grid dimensions.
    /// </summary>
    /// <remarks>This component requires a <see cref="Button"/> component to be attached to the same
    /// GameObject. When the button is clicked, the <see cref="GridOptionSelected"/> event is invoked with the current
    /// grid dimensions specified by the <see cref="Rows"/> and <see cref="Columns"/> properties.</remarks>
    [RequireComponent(typeof(Button))]
    public class GridOptionButton : MonoBehaviour
    {
        public UnityAction<int, int> GridOptionSelected;
        [field: SerializeField] public int Rows { get; private set; }
        [field: SerializeField] public int Columns { get; private set; }

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            GridOptionSelected?.Invoke(Rows, Columns);
        }
    }
}