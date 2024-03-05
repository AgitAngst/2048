using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Drawing;

public class UIManager : MonoBehaviour
{
    Vector3 scale = new Vector3();

    public void Scale(float size)
    {
        scale = gameObject.transform.localScale;
        var tempScale = new Vector3();
        tempScale = scale * size;
        gameObject.transform.DOScale(tempScale, 0.25f);
    }

    public void MouseExit()
    {
        gameObject.transform.DOScale(scale, 0.25f);

    }
}
