using System;
using Achievements;
using DG.Tweening;
using UnityEngine;

namespace Layout
{
    public class GridSystem : MonoBehaviour
    {
        public Grid grid;
        public SpriteRenderer CursorIcon;
        public Vector2 CursorIconOffset;
        public float CanvasMoveSpeed;

        private Building currentBuilding;
        private Vector3 mousePos;
        private Cell currentlyShowingCanvas;

        public static GridSystem Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            grid.SetCell(DiggingManager.Instance.Mine.gameObject, grid.width / 2, grid.height / 2);
        }

        void Update()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                HandleGridClicked();
            }
            if (currentBuilding != null)
            {
                CursorIcon.transform.position = (Vector2)mousePos + CursorIconOffset;
            }
        }

        public void SetBuilding(Building building)
        {
            currentBuilding = building;
            CursorIcon.gameObject.SetActive(building != null);
            if (building != null)
            {
                CursorIcon.sprite = building.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            }
        }

        private void HandleGridClicked()
        {

            var hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                var cell = hit.collider.GetComponent<Cell>();
                if (cell.x == grid.width / 2 && cell.y == grid.height / 2)
                {
                    if (currentBuilding == null && !DiggingManager.Instance.ActiveFader)
                    {
                        hit.collider.gameObject.transform.DOPunchScale(Vector3.one / 4, 0.1f);
                        DiggingManager.Instance.MineClicked();
                    }
                    else
                        return;
                }
                else
                {
                    if (currentBuilding != null)
                    {
                        grid.SetCell(currentBuilding.gameObject, cell.x, cell.y);

                        DiggingManager.Instance.PayGems(currentBuilding.Cost * DiggingManager.Instance.Depth);
                        AchievementSystem.Instance.BuildingBuilt(currentBuilding);
                        
                        SetBuilding(null);
                    }
                    else
                    {
                        if (cell.building != null)
                        {                           
                            HideCurrentBuildingCanvas(cell);
                            cell.building.GetComponentInChildren<BuildingCanvas>().Show(true);
                            currentlyShowingCanvas = cell;
                        }
                    }
                }
            }
            else
            {
                SetBuilding(null);
            }
        }
        public void HideCurrentBuildingCanvas()
        {
            HideCurrentBuildingCanvas(null);
        }

        public void HideCurrentBuildingCanvas(Cell cell)
        {
            if (currentlyShowingCanvas != null && (cell == null || currentlyShowingCanvas != cell))
            {
                currentlyShowingCanvas.building.GetComponentInChildren<BuildingCanvas>().Show(false);
            }
        }

        public void ClearGrid()
        {
            foreach(var cell in grid.cells)
            {
                Destroy(cell.gameObject);
            }
            grid.BuildGrid();
            Initialize();
        }
    }
}