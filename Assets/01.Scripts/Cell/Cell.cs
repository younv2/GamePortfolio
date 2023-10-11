using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cells
{
    public class Cell
    {
        public CellType m_type = CellType.EMPTY;
        CellBehavior m_cellBehavior;

        public Cell()
        {
            m_type = CellType.EMPTY;
        }
        public Cell(CellType _type)
        {
            m_type = _type;
        }

        public GameObject CreateCell(GameObject _cellPrefab)
        {
            GameObject newCell = Object.Instantiate(_cellPrefab);
            m_cellBehavior = newCell.GetComponent<CellBehavior>();

            return newCell;
        }
    }
}

