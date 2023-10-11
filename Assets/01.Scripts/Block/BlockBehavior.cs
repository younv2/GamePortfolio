using System;
using System.Collections;
using UnityEngine;

namespace Game.Blocks
{

    public class BlockBehavior : MonoBehaviour
    {
        public enum State { NORMAL, MOVE }
        public State m_state;
        [SerializeField] BlockConfig m_blockConfig;
        [SerializeField] SpriteRenderer m_spriteRenderer;

        public void Initialize(BlockType blockType)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            SetSprite(blockType);
        }
        public void SetSprite(BlockType blockType)
        {
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);
            m_spriteRenderer.sprite = blockType == BlockType.NA ? null : m_blockConfig.GetBlockSprite(blockType);
        }
        //Todo : ActionManager등을 따로 만들어 모듈화하도록 함
        public void MoveTo(Vector3 goal)
        {
            StartCoroutine(Action2D.MoveTo(transform, goal, Constraints.BLOCK_MOVE_DURATION));
        }
        public void Pop()
        {
            StartCoroutine(Action2D.Pop(transform, Constraints.BLOCK_POP_ANIMATION_SIZE, Constraints.BLOCK_POP_ANIMATION_DURATION));
        }
    }

}