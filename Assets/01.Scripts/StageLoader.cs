using UnityEditor;
using UnityEngine;

namespace Game.Stages
{
    public static class StageLoader
    {
        public static StageInfo LoadStage(int nStage)
        {
            Debug.Log($"Load Stage : Stage/{GetFileName(nStage)}");

            //1. 리소스 파일에서 텍스트를 읽어온다.
            TextAsset textAsset = Resources.Load<TextAsset>($"Stage/{GetFileName(nStage)}");
            if (textAsset != null)
            {
                //2. JSON 문자열을 객체(StageInfo)로 변환한다.
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
