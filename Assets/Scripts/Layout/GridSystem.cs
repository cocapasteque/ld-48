using System;
using UnityEngine;

namespace Layout
{
    public class GridSystem : MonoBehaviour
    {
        public Grid grid;
        public GameObject housePrefab;
        public GameObject tavernPrefab;
        public GameObject forgePrefab;

        public BuildingType currentType;
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleGridClicked();                
            }
        }

        private void HandleGridClicked()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                var cell = hit.collider.GetComponent<Cell>();

                switch (currentType)
                {
                    case BuildingType.House:
                        grid.SetCell(housePrefab, cell.x, cell.y);
                        break;
                    case BuildingType.Tavern:
                        grid.SetCell(tavernPrefab, cell.x, cell.y);
                        break;
                    case BuildingType.Forge:
                        grid.SetCell(forgePrefab, cell.x, cell.y);
                        break;
                }
            }
        }
    }

    public enum BuildingType
    {
        House, Tavern, Forge
    }
}
