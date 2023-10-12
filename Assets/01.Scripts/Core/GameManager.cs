using Game.Blocks;
using Game.Boards;
using Game.Boards.View;
using Game.Stages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

namespace Game.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        Board           m_board;
        BoardViewer     m_boardViewer;
        ActionManager   m_actionManager;
        Stage           m_stage;
        public ActionManager ActionManager { get { return m_actionManager; } }

        protected override void Awake()
        {
            base.Awake();
            Initialized();
        }
        public void Initialized()
        {
            m_actionManager = new ActionManager(this);
            m_board = new Board();
            m_boardViewer = GameObject.Find("Board").GetComponent<BoardViewer>();
            m_stage = new Stage();

            LoadStage(1);
            
        }
        public void LoadStage(int stage)
        {
            m_stage.LoadStage(stage);

            m_board.SetBoardSize(m_stage.StageInfo.m_row, m_stage.StageInfo.m_col);
            m_board.InitCells(m_stage.StageInfo.m_cells);
            m_board.InitBlocks();

            m_boardViewer.SetBoardSize(m_stage.StageInfo.m_row, m_stage.StageInfo.m_col);
            m_boardViewer.ShowCell(m_board.m_cells);
            m_boardViewer.SetBlockInDisplay(m_board.GetBlocks());
        }
        public void Reset()
        {
            m_board.InitBlocks();
            m_boardViewer.SetBlockInDisplay(m_board.GetBlocks());
        }

        public BoardViewer GetBoardViewer()
        {
            return m_boardViewer;
        }
        public void ShowBoardInConsole()
        {
            m_board.ShowBoardInConsole();
        }
        public IEnumerator SwapBlock(Global.Coord coord, Board.Direction dir)
        {
            var offset = m_board.dOffsets[dir];

            yield return m_actionManager.CoSwipe(m_board.m_blocks[coord.m_row, coord.m_col], m_board.m_blocks[coord.m_row + offset.dRow, coord.m_col + offset.dCol]);

            var datas = m_board.SwapBlock(coord, dir);
            
            if (datas == null)
                yield return m_actionManager.CoSwipe(m_board.m_blocks[coord.m_row, coord.m_col], m_board.m_blocks[coord.m_row + offset.dRow, coord.m_col + offset.dCol]);
            else
            {
                List<Block>[] blocksDropData= new List<Block>[datas.Count];
                yield return m_actionManager.CoPop(datas);
                for(int i = 0; i<datas.Count; i++)
                    blocksDropData[i] =  m_board.DropBlock(datas[i]);
                yield return m_actionManager.CoDrop(blocksDropData);

            }
        }
    }
}