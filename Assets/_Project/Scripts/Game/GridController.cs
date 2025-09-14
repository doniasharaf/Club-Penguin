using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{

    [RequireComponent(typeof(GridLayoutGroup))]
    /// <summary>
    /// Controls and manages a responsive grid layout.
    /// Dynamically adjusts grid size based on parent RectTransform and margins.
    /// Ensures children are added and scaled correctly.
    /// Can clear and repopulate the grid.
    /// </summary>
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private int verticalMargin = 300;
        [SerializeField] private int horizontalMargin = 150;
        private float _availableHeight;
        private float _availableWidth;

        /// <summary>
        /// Sets the grid size based on row/column count and resizes cells
        /// to fit within available space after margins.
        /// </summary>
        public void SetGridSize(int rows, int columns)
        {
            ClearGrid();
            GetMaxSize();

            _gridLayoutGroup.constraintCount = columns;

            float totalWidth = _availableWidth - (_gridLayoutGroup.spacing.x * (columns - 1));
            float totalHeight = _availableHeight - (_gridLayoutGroup.spacing.y * (rows - 1));

            float cellWidth = totalWidth / columns;
            float cellHeight = totalHeight / rows;

            float cellSize = Mathf.Min(cellWidth, cellHeight);

            _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        }
        /// <summary>
        /// Adds a child transform to the grid and resets its scale.
        /// </summary>
        public void AddChildTransform(Transform child)
        {
            child.SetParent(_gridLayoutGroup.transform);
            child.localScale = Vector3.one;
        }

        private void ClearGrid()
        {
            foreach (Transform child in _gridLayoutGroup.transform)
            {
                Destroy(child.gameObject);
            }
        }
        private void GetMaxSize()
        {
            RectTransform parentRt = _gridLayoutGroup.transform.parent.GetComponent<RectTransform>();
            _availableHeight = parentRt.rect.height - verticalMargin * 2;
            _availableWidth = parentRt.rect.width - horizontalMargin * 2;

        }


    }
}