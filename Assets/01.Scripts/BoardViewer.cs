using Game.Blocks;
using Game.Cells;
using Game.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

namespace Game.Boards.View
{
    public class BoardViewer : MonoBehaviour
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject blockPrefab;
        [SerializeField] Transform boardTrans;

        int m_height;
        int m_width;
        public int Height { get { return m_height; }}
        public int Width { get { return m_width; }}

        Vector3 blockSize;

        public Coord FindBlockCoord(Vector3 _pos)
        {
            //보드의 크기에 맞춰 몇번째 블럭을 클릭했는지 받아옴
            int col = (int)((_pos.x / boardTrans.localScale.x) / blockSize.x);
            int row = (int)((Math.Abs(_pos.y) / boardTrans.localScale.y) / blockSize.y);

            //블럭이 위치한 곳을 클릭하지 않을 경우
            if (col < 0 || col >= m_width || row < 0 || row >= m_height)
                return new Coord(-1, -1);

            return new Coord(row, col);
        }
        public void SetBlockInDisplay(Block[,] blocks)
        {
            for(int i =0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    if (blocks[i,j].m_blockBehavior == null)
                    {
                        Transform temp = blocks[i, j].CreateBlock(blockPrefab).transform;
                        temp.parent = boardTrans;
                        temp.localScale = new Vector3(1f / m_width, 1f / m_height);
                        Vector3 tempScale = temp.localScale;
                        temp.localPosition = new Vector3(j * tempScale.x - (tempScale.x * m_width / 2),
                            -(i * tempScale.y) + (tempScale.y * m_height / 2), 0);
                        temp.localPosition += new Vector3(tempScale.x / 2, -tempScale.y / 2);
                        blockSize = temp.localScale;
                        temp.localScale *= 0.9f;
                    }
                    else
                        blocks[i, j].m_blockBehavior.SetSprite(blocks[i, j].m_type);
                }
            }
        }
        public void SetBoardSize(int row, int col)
        {
            m_height = row;
            m_width = col;
        }
        public void ShowCell(Cell[,] cells)
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].m_type != CellType.EMPTY)
                    {
                        Transform temp = cells[i, j].CreateCell(cellPrefab).transform;
                        temp.parent = boardTrans;
                        temp.localScale = new Vector3(1f / m_width, 1f / m_height);
                        temp.localPosition = new Vector3(j * temp.localScale.x - (temp.localScale.x * m_width / 2),
                            -(i * temp.localScale.y) + (temp.localScale.y * m_height / 2), 0);
                        temp.localPosition += new Vector3(temp.localScale.x / 2, -temp.localScale.y / 2);
                    }
                }
            }
        }
        #region Animation
        public IEnumerator Swap(Block block, Block block2, Returnable<bool> returnable)
        {
            returnable.value = false;
            if (!(block.IsValidate() && block2.IsValidate()))
                yield break;
            var block2Pos = block2.m_blockBehavior.transform.position;
            var blockPos = block.m_blockBehavior.transform.position;
            block.m_blockBehavior.MoveTo(block2Pos);
            block2.m_blockBehavior.MoveTo(blockPos);

            yield return new WaitForSeconds(Constraints.BLOCK_MOVE_DURATION);

            returnable.value = true;
        }

        public IEnumerator Pop(Block block, Returnable<bool> returnable)
        {
            returnable.value = false;

            block.m_blockBehavior.Pop();

            yield return new WaitForSeconds(Constraints.BLOCK_POP_ANIMATION_DURATION);

            returnable.value = true;
        }
        public IEnumerator DropBlock(Block block, Vector3 block2, Returnable<bool> returnable)
        {
            returnable.value = false;

            //TODO : 해당 블럭 및 상단에 있는 블럭들이 움직일 수 있도록 해야함
            block.m_blockBehavior.MoveTo(block2);

            yield return new WaitForSeconds(Constraints.BLOCK_MOVE_DURATION);

            returnable.value = true;
        }
        #endregion
    }
}
