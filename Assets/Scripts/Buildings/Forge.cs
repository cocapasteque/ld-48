using Layout;
using UnityEngine;

namespace Buildings
{
    public class Forge : MonoBehaviour
    {
        [HideInInspector] public Cell cell;
        public int dwarvesCounts = 5;
        
        private void Start()
        {
            cell = GetComponent<Cell>();
        }
    }
}