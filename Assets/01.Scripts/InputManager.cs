using Game.Boards;
using Game.Boards.View;
using Game.Core;
using UnityEngine;
using static Global;

//���ӸŴ����� ����������Ʈ�ѷ� ������ �ϸ� �ɰ� ����, StageController�� InputManager�� �����Ͽ� Input���ø� �����.
public class InputManager : MonoSingleton<InputManager>
{
    #region Variable
    BoardViewer boardViewer;
    Transform boardTransform;
    Coord tempCoord;
   
    float pushedMousePosY;
    float pushedMousePosX;
    bool isPushed;
    ActionManager m_actionManager;

    int width;
    int height;
    #endregion
    private void Start()
    {
        boardViewer = GameManager.Instance.GetBoardViewer();
        height = boardViewer.Height;
        width = boardViewer.Width;
        boardTransform = boardViewer.transform;
        m_actionManager = GameManager.Instance.ActionManager;
    }
    private void Update()
    {
        InputController();
    }
    public void InputController()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //���� ������ġ�� ��� ���� ���콺 ��ġ��ǥ ���� ���
        mousePos.y -= boardTransform.position.y;
        mousePos.x += boardTransform.localScale.x * 0.5f;
        mousePos.y += -boardTransform.localScale.y * 0.5f;

        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.ShowBoardInConsole();
            pushedMousePosX = mousePos.x;
            pushedMousePosY = mousePos.y;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            tempCoord = boardViewer.FindBlockCoord(mousePos);

            if (!Coord.Equals(tempCoord, new Coord(-1,-1)))
                isPushed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPushed = false;
            pushedMousePosX = 0;
            pushedMousePosY = 0;
        }

        if (isPushed)
        {
            if (pushedMousePosX < mousePos.x - boardTransform.localScale.x / width)
            {
                m_actionManager.StartCoroutine(GameManager.Instance.SwapBlock(tempCoord, Board.Direction.RIGHT));
                isPushed = false;
            }
            else if (pushedMousePosX > mousePos.x + boardTransform.localScale.x / width)
            {
                m_actionManager.StartCoroutine(GameManager.Instance.SwapBlock(tempCoord, Board.Direction.LEFT));
                isPushed = false;
            }
            else if (pushedMousePosY < mousePos.y - boardTransform.localScale.y / height)
            {
                m_actionManager.StartCoroutine(GameManager.Instance.SwapBlock(tempCoord, Board.Direction.UP));
                isPushed = false;
            }
            else if (pushedMousePosY > mousePos.y + boardTransform.localScale.y / height)
            {
                m_actionManager.StartCoroutine(GameManager.Instance.SwapBlock(tempCoord, Board.Direction.DOWN));
                isPushed = false;
            }
        }
    }
}
