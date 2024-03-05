using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerMy : MonoBehaviour
{


    public void SceneSwitch(int index)
    {
        SceneManager.LoadScene(index);
    }

}
