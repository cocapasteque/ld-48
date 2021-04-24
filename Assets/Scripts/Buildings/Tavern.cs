using Layout;
using UnityEngine;

namespace Buildings
{
    public class Tavern : MonoBehaviour
    {
        [HideInInspector] public Cell cell;
        public int dwarvesCounts = 5;
        
        private void Start()
        {
            cell = GetComponent<Cell>();
        }
    }
}