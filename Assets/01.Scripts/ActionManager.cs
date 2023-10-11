using Game.Blocks;
using Game.Boards;
using Game.Core;
using Game.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class ActionManager
{
    public bool m_isRunning = false;
    public MonoBehaviour m_monoBehaviour;
    public ActionManager(MonoBehaviour mono)
    {
        m_monoBehaviour = mono;
        m_isRunning = false;
    }

    public Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return m_monoBehaviour.StartCoroutine(enumerator);
    }
    public IEnumerator CoSwipe(Block block, Block block1)
    {
        if(!m_isRunning) 
        {
            m_isRunning = true;
            Returnable<bool> bSwipedBlock = new Returnable<bool>(false);
            yield return GameManager.Instance.GetBoardViewer().Swap(block,block1, bSwipedBlock);
            m_isRunning = false;
        }
        yield return null;

        
    }
    public IEnumerator CoPop(List<Block> block)
    {
        if (!m_isRunning)
        {
            m_isRunning = true;
            Returnable<bool> bSwipedBlock = new Returnable<bool>(false);
            foreach(Block b in block)
                m_monoBehaviour.StartCoroutine(GameManager.Instance.GetBoardViewer().Pop(b, bSwipedBlock));
            yield return new WaitForSecondsRealtime(Constraints.BLOCK_POP_ANIMATION_DURATION);
            m_isRunning = false;
        }
        yield return null;
    }

    public IEnumerator CoDrop(List<Block>[] dropDatas)
    {
        if (!m_isRunning)
        {
            m_isRunning = true;
            Returnable<bool> bSwipedBlock = new Returnable<bool>(false);
            List<Vector3>[] dropDatasPosition = new List<Vector3>[dropDatas.Length];
            for (int i = 0; i < dropDatas.Length; i++)
            {
                dropDatasPosition[i] = new List<Vector3>();
                for (int j = 0; j < dropDatas[i].Count; j++)
                    dropDatasPosition[i].Add(dropDatas[i][j].m_blockBehavior.transform.position);
            }
            for (int i = 0;  i < dropDatas.Length; i++)
            {
                for (int j = dropDatas[i].Count-1; j > 0; j--)
                    m_monoBehaviour.StartCoroutine(GameManager.Instance.GetBoardViewer().DropBlock(dropDatas[i][j], dropDatasPosition[i][j-1], bSwipedBlock));
            }
            yield return new WaitForSecondsRealtime(Constraints.BLOCK_MOVE_DURATION);
            m_isRunning = false;
        }
        yield return null;
    }
}
