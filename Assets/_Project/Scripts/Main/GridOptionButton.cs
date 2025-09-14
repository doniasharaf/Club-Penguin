using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
