using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Action2D
{

    /*
     * ������ �ð����� ������ ��ġ�� �̵��Ѵ�.
     * 
     * @param target �ִϸ��̼��� ������ Ÿ�� GameObject
     * @param to �̵��� ��ǥ ��ġ
     * @param duration �̵� �ð�
     * @param bSelfRemove �ִϸ��̼� ���� �� Ÿ�� GameObject ���� ���� �÷���
     * 
     * ���� = https://ninezmk2.blogspot.com/2019/11/blog-post_11.html
     */
    public static IEnumerator MoveTo(Transform target, Vector3 to, float duration, bool bSelfRemove = false)
    {
        Vector2 startPos = target.transform.position;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.smoothDeltaTime;
            target.transform.position = Vector2.Lerp(startPos, to, elapsed / duration);

            yield return null;
        }

        target.transform.position = to;

        if (bSelfRemove)
            Object.Destroy(target.gameObject, 0.1f);

        yield break;
    }
    public static IEnumerator Pop(Transform target,float goalSize, float duration, bool bSelfRemove = false)
    {
        Vector2 originScale = target.transform.localScale;
        Vector2 goalScale = target.transform.localScale * goalSize;

        //�۾����� �ִϸ��̼�
        float elapsed = 0.0f;
        while (elapsed < duration/2)
        {
            elapsed += Time.smoothDeltaTime;
            target.transform.localScale = Vector2.Lerp(originScale, goalScale, elapsed / duration);

            yield return null;
        }
        elapsed = 0.0f;
        //Ŀ���� �ִϸ��̼�
        while (elapsed < duration/2)
        {
            elapsed += Time.smoothDeltaTime;
            target.transform.localScale = Vector2.Lerp(goalScale, goalScale*1.7f, elapsed / duration);

            yield return null;
        }
        //�ִϸ��̼� ���� �� ��Ȱ��
        target.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        target.transform.localScale = originScale;
        if (bSelfRemove)
            Object.Destroy(target.gameObject, 0.1f);

        yield break;
    }
}
