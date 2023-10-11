
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Blocks;
using Game.Cells;
using Game.Core;
using static Global;
using System.Collections;

namespace Game.Boards
{
    public class Board
    {
        public enum Direction { NONE, UP, DOWN, LEFT, RIGHT }
        //리스트나 배열로도 사용 가능함
        public Dictionary<Direction, (int dRow, int dCol)> dOffsets = new Dictionary<Direction, (int dRow, int dCol)>
        {
            { Direction.UP, (-1, 0) },
            { Direction.DOWN, (1, 0) },
            { Direction.LEFT, (0, -1) },
            { Direction.RIGHT, (0 ,1) }
        };

        public Cell[,] m_cells;
        public Block[,] m_blocks;

        public int m_height;
        public int m_width;

        public Board()
        {
            Initialize();
        }
        public void Initialize()
        {

        }

        public void InitBlocks()
        {
            for (int i = 0; i < m_blocks.GetLength(0); i++)
            {
                for (int j = 0; j < m_blocks.GetLength(1); j++)
                {
                    if (m_blocks[i,j] == null)
                        m_blocks[i, j] = BlockFactory.SpawnBlock(m_cells[i,j].m_type);
                    else
                        BlockFactory.ChangeBlock(m_blocks[i, j]);
                }
            }
            for (int i = 0; i < m_blocks.GetLength(0); i++)
            {
                for (int j = 0; j < m_blocks.GetLength(1); j++)
                {
                    while (CheckMatched(new Coord(i, j)) != null && m_blocks[i,j].m_type != BlockType.NA)
                        BlockFactory.ChangeBlock(m_blocks[i, j]);
                }
            }
        }
        public void InitCells(int[] celltypes)
        {
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++)
                {
                    m_cells[i, j] = CellFactory.SpawnCell(celltypes[i*m_cells.GetLength(0) + j]);
                }
            }
        }
        public void SetBoardSize(int row, int col)
        {
            m_cells = new Cell[row, col];
            m_blocks = new Block[row, col];

            m_height = row;
            m_width = col;
        }
        bool IsCoordValidate(Coord coord)
        {
            if(coord.m_row < 0 || coord.m_row > m_height || coord.m_col < 0 || coord.m_col > m_width)
                return false;
            return true;
        }
        public List<Block> SwapBlock(Coord coord, Direction direction)
        {
            var offset = dOffsets[direction];
            Coord nextCoord = new Coord(coord.m_row + offset.dRow, coord.m_col + offset.dCol);

            if (!IsCoordValidate(coord) || !IsCoordValidate(nextCoord) || direction == Direction.NONE)
                return null;

            Block block = m_blocks[coord.m_row, coord.m_col];
            Block block2 = m_blocks[coord.m_row + offset.dRow, coord.m_col + offset.dCol];

            if (!(block.IsValidate() && block2.IsValidate()))
                return null;

            Swap(coord, nextCoord);

            List<Block> data = CheckMatched(coord);
            if(CheckMatched(nextCoord) != null)
            {
                if (data == null)
                    data = CheckMatched(nextCoord);
                data.AddRange(CheckMatched(nextCoord));
            }
            if (data == null)
            {
                Swap(coord, nextCoord);
                return null;
            }
            else
            {
                PopBlocks(data);
                return data;
            }
        }
        public void Swap(Coord coord, Coord tempCoord)
        {
            Block temp = m_blocks[coord.m_row, coord.m_col];
            m_blocks[coord.m_row, coord.m_col] = m_blocks[tempCoord.m_row, tempCoord.m_col];
            m_blocks[tempCoord.m_row, tempCoord.m_col] = temp;
        }
        public void ShowBoardInConsole()
        {
            var str = "";
            for (int i = 0; i < m_blocks.GetLength(0); i++)
            {
                for (int j = 0; j < m_blocks.GetLength(1); j++)
                {
                    str += $"[{i} ,{j}]-> {m_blocks[i, j].m_type.ToString().Substring(0, 2)} ";
                }
                str += "\n";
            }
            Debug.Log(str);

        }
        public void PopBlocks(List<Block> blocks)
        {
            if (blocks == null)
                return;
            foreach (Block block in blocks)
            {
                if (block != null)
                    block.m_state = BlockState.POP;
            }
        }
        public List<Block> DropBlock(Block block)
        {
            List<Block> result = new List<Block>() {block };

            if (block.m_state==BlockState.POP)
            {
                Coord coord = MatchedBlockCoord(block);

                for (int i = coord.m_row; i > 0;)
                {
                    if (m_blocks[i, coord.m_col].m_type == BlockType.NA)
                        continue;
                    int cnt = 1;

                    //NA일 경우 그 블럭은 건너띄고 그 다음을 검사
                    for(; ; )
                    {
                        if (m_blocks[i-cnt, coord.m_col].m_type == BlockType.NA)
                        {
                            cnt++;
                            if (i - cnt < 0) 
                                break;
                            continue;
                        }
                        break;
                    }
                    if (i - cnt < 0) 
                        break;
                    result.Add(m_blocks[i - cnt, coord.m_col]);

                    Swap(new Coord(i, coord.m_col), new Coord(i-cnt, coord.m_col));
                    i -= cnt;
                }
            }
            else
                return null;
            ShowBoardInConsole();
            return result;
        }
        public Coord MatchedBlockCoord(Block block)
        {
            for (int i = 0; i < m_blocks.GetLength(0); i++)
            {
                for (int j = 0; j < m_blocks.GetLength(1); j++)
                {
                    if (m_blocks[i, j] == block)
                        return new Coord(i, j);
                }
            }
            return new Coord(-1, -1);
        }
        public List<Block> CheckMatched(Coord coord)
        {
            if (m_blocks[coord.m_row, coord.m_col] == null)
                return null;

            List<Block> lBlock = GetConnectedBlocks(coord.m_row, coord.m_col);

            if (lBlock.Count >= 3)
                return lBlock;

            return null;
        }
        //연결된 블럭을 찾는 함수
        public List<Block> GetConnectedBlocks(int x, int y)
        {
            var connectedBlocks = new List<Block> { m_blocks[x, y] };
            //가로 및 세로의 매칭을 따로 확인 하기 위함
            var horiConnectedBlocks = new List<Block>();
            var vertConnectedBlocks = new List<Block>();

            void DFS(int row, int col, int dRow, int dCol)
            {
                //배열을 벗어날 경우 바로 Return시킴
                if (row < 0 || row >= m_blocks.GetLength(0) || col < 0 || col >= m_blocks.GetLength(1))
                    return;
                //블록 타입과 해당 방향이 맞지 않을경우 Return
                if (m_blocks[row, col].m_type != m_blocks[x, y].m_type)
                    return;

                if (dRow != 0 && dCol == 0)
                    vertConnectedBlocks.Add(m_blocks[row, col]);
                else if (dRow == 0 && dCol != 0)
                    horiConnectedBlocks.Add(m_blocks[row, col]);
                //방향은 계속 같은 방향만 탐색시킴
                DFS(row + dRow, col + dCol, dRow, dCol);
            }

            // 전체 방향을 탐색 시킴
            DFS(x - 1, y, -1, 0); // up
            DFS(x + 1, y, 1, 0); // down
            DFS(x, y - 1, 0, -1); // left
            DFS(x, y + 1, 0, 1); // right

            if (vertConnectedBlocks.Count < 2)
                vertConnectedBlocks.Clear();
            else if (horiConnectedBlocks.Count < 2)
                horiConnectedBlocks.Clear();
            connectedBlocks.AddRange(horiConnectedBlocks);
            connectedBlocks.AddRange(vertConnectedBlocks);

            return connectedBlocks;
        }
        public Block[,] GetBlocks()
        {
            return m_blocks;
        }
    }
}