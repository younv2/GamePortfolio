using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public struct Coord
    {
        public int m_row;
        public int m_col;
        public Coord(int _row, int _col)
        {
            m_row = _row;
            m_col = _col;
        }
        public bool MatchedCol(Coord coord)
        {
            if(m_col == coord.m_col)
                return true;
            else
                return false;
        }
    }
}
