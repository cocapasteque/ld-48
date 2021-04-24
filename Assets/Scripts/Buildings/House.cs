﻿using System;
using Layout;
using UnityEngine;

namespace Buildings
{
    public class House : MonoBehaviour
    {
        [HideInInspector] public Cell cell;
        public int dwarvesCounts = 5;
        
        private void Start()
        {
            cell = GetComponent<Cell>();
        }
    }
}