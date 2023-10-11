using UnityEditor;
using UnityEngine;

namespace Game.Stages
{
    public static class StageLoader
    {
        public static StageInfo LoadStage(int nStage)
        {
            Debug.Log($"Load Stage : Stage/{GetFileName(nStage)}");

            //1. ���ҽ� ���Ͽ��� �ؽ�Ʈ�� �о�´�.
            TextAsset textAsset = Resources.Load<TextAsset>($"Stage/{GetFileName(nStage)}");
            if (textAsset != null)
            {
                //2. JSON ���ڿ��� ��ü(StageInfo)�� ��ȯ�Ѵ�.
                StageInfo stageInfo = JsonUtility.FromJson<StageInfo>(textAsset.text);

                return stageInfo;
            }

            return null;
        }

        static string GetFileName(int nStage)
        {
            return string.Format("stage_{0:D4}", nStage);
        }
    }
}
