using UnityEngine;

namespace Game.Stages
{
    [System.Serializable]
    public class StageInfo
    {
        public int m_row;
        public int m_col;

        public int[] m_cells;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
