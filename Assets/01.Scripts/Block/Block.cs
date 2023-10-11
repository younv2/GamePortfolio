using Unity.VisualScripting;
using UnityEngine;

namespace Game.Blocks 
{   public class Block
    {
        public BlockType m_type = BlockType.NA;
        public BlockState m_state = BlockState.NORMAL;
        public BlockBehavior m_blockBehavior;

        #region Constructor
        public Block()
        {
            m_type = BlockType.NA;
            m_state = BlockState.NORMAL;
        }
        #endregion

        //블록 생성
        public GameObject CreateBlock(GameObject blockPrefab)
        {
            GameObject newBlock = Object.Instantiate(blockPrefab);
            m_blockBehavior = newBlock.GetComponent<BlockBehavior>();
            m_blockBehavior.Initialize(m_type);

            return newBlock;
        }
        public bool IsValidate()
        {
            return !(m_type == BlockType.NA);
        }
        public bool IsPop()
        {
            return m_state == BlockState.POP;
        }
    }
}

