using System;
using Achievements;
using UnityEngine;

namespace Layout
{
    public class GridSystem : MonoBehaviour
    {
        public Grid grid;
        public SpriteRenderer CursorIcon;
        public Vector2 CursorIconOffset;

        private Building currentBuilding;
        private Vector3 mousePos;
       
        public static GridSystem Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);
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
            if (currentBuilding != null)
            {
                var hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit)
                {
                    var cell = hit.collider.GetComponent<Cell>();
                    grid.SetCell(currentBuilding.gameObject, cell.x, cell.y);
                    
                    DiggingManager.Instance.PayGems(currentBuilding.Cost);
                    AchievementSystem.Instance.BuildingBuilt(currentBuilding);
                    
                    SetBuilding(null);
                }
            }
        }
    }
}
