using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridController : MonoBehaviour
{
    [SerializeField] private int verticalMargin = 300;
    [SerializeField] private int horizontalMargin = 150;
    private GridLayoutGroup _gridLayoutGroup;
    private float _availableHeight;
    private float _availableWidth;
    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    public void SetGridSize(int rows, int columns)
    {
        GetMaxSize();

        _gridLayoutGroup.constraintCount = columns;

        float totalWidth = _availableWidth - (_gridLayoutGroup.spacing.x * (columns - 1));
        float totalHeight = _availableHeight - (_gridLayoutGroup.spacing.y * (rows - 1));

        float cellWidth = totalWidth / columns;
        float cellHeight = totalHeight / rows;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }

    public void AddChildTransform(Transform child)
    {
        child.SetParent(_gridLayoutGroup.transform);
        child.localScale = Vector3.one;
    }


    private void GetMaxSize()
    {
        RectTransform parentRt = _gridLayoutGroup.transform.parent.GetComponent<RectTransform>();
        _availableHeight = parentRt.rect.height - verticalMargin * 2;
        _availableWidth = parentRt.rect.width - horizontalMargin * 2;

    }


#if UNITY_EDITOR

    [ContextMenu("Set 2")]
    public void Set2()
    {
        _gridLayoutGroup.constraintCount = 2;
        SetGridSize(2, 2);
    }

    [ContextMenu("Set 3")]
    public void Set5()
    {
        _gridLayoutGroup.constraintCount = 6;
        SetGridSize(5, 6);
    }

    [ContextMenu("Set 4")]

    public void Set10()
    {
        _gridLayoutGroup.constraintCount = 4;
        SetGridSize(4, 4);
    }
#endif
}
