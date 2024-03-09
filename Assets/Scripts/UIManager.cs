using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Vector3 scale = new Vector3();
    Vector3 tempScale = new Vector3();


    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
    }
    public void Scale(float size)
    {
        scale = gameObject.transform.localScale;
        tempScale = scale * size;
        gameObject.transform.DOScale(tempScale, 0.25f)
            .SetLoops(2, LoopType.Yoyo)
            .OnPlay(() => gameObject.GetComponent<Button>().interactable = false)
            .OnComplete(() => gameObject.GetComponent<Button>().interactable = true);
    }

    public void MouseExit()
    {
        gameObject.transform.DOScale(scale, 0.25f);

    }

    void PlayBackward()
    {
        gameObject.transform.DOScale(tempScale, 0.25f).From();
    }
}
