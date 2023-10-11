//일반 블럭의 Sprite를 소지하고 있으며, 블록이 스프라이트를 참조할 때 사용하도록 함
//(메모리를 최대한 사용하지 않도록 하기 위함)
//(같은 메모리 주소를 참조할 뿐 새 스프라이트를 생성하는 것이 아니기 때문에 메모리 사용량이 크게 줄 수 있음
using UnityEngine;

namespace Game.Blocks
{

    [CreateAssetMenu(fileName = "BlockConfig.asset", menuName = "Config/BlockConfig")]
    public class BlockConfig : ScriptableObject
    {
        public Sprite[] m_BlockSprites;

        //노말블록 스프라이트 교체
        public Sprite GetBlockSprite(BlockType _type)
        {
            if (_type == BlockType.NA)
                return null;

            return m_BlockSprites[(int)_type - 1];
        }
    }
}