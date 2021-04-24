using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

namespace Layout
{
    public class Grid : MonoBehaviour
    {
        public int width;
        public int height;
        public float cellWidth;
        public float cellHeight;

        public GameObject emptyCell;
        
        public Cell[,] cells;

        private void Start()
        {
            BuildGrid();
        }
        
        public void SetCell(GameObject prefab, int x, int y)
        {
            var previous = cells[x, y];
            Destroy(previous.gameObject);
            var instance = Instantiate(prefab, transform);
            instance.transform.position = new Vector2(x * cellWidth, y * cellHeight);
            instance.name = $"Cell ({x}-{y})";

            var cell = instance.GetComponent<Cell>();
            cell.x = x;
            cell.y = y;

            cells[x, y] = cell;
        }

        public void BuildGrid()
        {
            cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pos = new Vector2(x * cellWidth, y * cellHeight);
                    var instance = Instantiate(emptyCell, transform);
                    instance.transform.position = pos;
                    instance.name = $"Cell ({x}-{y})";

                    var cell = instance.GetComponent<Cell>();
                    cell.x = x;
                    cell.y = y;

                    cells[x, y] = cell;
                }
            }
        }

        private void OnDrawGizmos()
        {
            var size = new Vector2(cellWidth, cellHeight);
            Gizmos.color = Color.green;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pos = new Vector2(x * cellWidth, y * cellHeight);
                    Gizmos.DrawWireCube(pos, size);
                }
            }
        }
    }
}