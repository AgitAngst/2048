using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMouseOverButtons : MonoBehaviour
{
    public Image uiImage;
    public Sprite[] _images;
    public void ChangePicture (int index)
    {
        uiImage.sprite = _images[index];
    }
}
