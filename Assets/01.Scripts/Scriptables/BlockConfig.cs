//�Ϲ� ���� Sprite�� �����ϰ� ������, ����� ��������Ʈ�� ������ �� ����ϵ��� ��
//(�޸𸮸� �ִ��� ������� �ʵ��� �ϱ� ����)
//(���� �޸� �ּҸ� ������ �� �� ��������Ʈ�� �����ϴ� ���� �ƴϱ� ������ �޸� ��뷮�� ũ�� �� �� ����
using UnityEngine;

namespace Game.Blocks
{

    [CreateAssetMenu(fileName = "BlockConfig.asset", menuName = "Config/BlockConfig")]
    public class BlockConfig : ScriptableObject
    {
        public Sprite[] m_BlockSprites;

        //�븻��� ��������Ʈ ��ü
        public Sprite GetBlockSprite(BlockType _type)
        {
            if (_type == BlockType.NA)
                return null;

            return m_BlockSprites[(int)_type - 1];
        }
    }
}