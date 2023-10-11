using Game.Cells;
using System;
using UnityEngine;

namespace Game.Blocks
{
    public class BlockFactory : MonoBehaviour
    {
        public static Block SpawnBlock()
        {
            Block block = new Block();

            block.m_type = (BlockType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(BlockType)).Length);

            return block;
        }
        //블록 생성
        public static Block SpawnBlock(BlockType blockType)
        {
            Block block = new Block();

            block.m_type = blockType;

            return block;
        }

        public static Block SpawnBlock(CellType cell)
        {
            Block block = new Block();
            block.m_type = cell == CellType.EMPTY ? BlockType.NA : (BlockType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(BlockType)).Length);

            return block;
        }
        public static void ChangeBlock(Block block)
        {
            if(block.m_type != BlockType.NA)
            {
                while(true)
                {
                    var temp = block.m_type;
                    block.m_type = (BlockType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(BlockType)).Length);
                    if (temp != block.m_type)
                        break;
                }

            }
        }
    }
}

