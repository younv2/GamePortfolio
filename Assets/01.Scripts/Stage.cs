using Game.Boards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stages
{
    public class Stage
    {
        int m_stage;
        StageInfo m_stageInfo;
        public StageInfo StageInfo { get { return m_stageInfo; } }
        public void LoadStage(int stage)
        {
            m_stageInfo = StageLoader.LoadStage(stage);
            m_stage = stage;
        }
    }
}
