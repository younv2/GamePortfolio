using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cells
{
    public class CellFactory
    {
        public static Cell SpawnCell()
        {
            Cell cell = new Cell();

            return cell;
        }

        public static Cell SpawnCell(int cellType)
        {
            Cell cell = new Cell();

            cell.m_type = (CellType)cellType;

            return cell;
        }
    }

}
