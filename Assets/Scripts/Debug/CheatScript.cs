using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private bool isUsingXRay = false;

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(KeyCode.F5))
        {
            if (!isUsingXRay)
            {
                isUsingXRay = true;
                GameManager.Instance.CheatXRay(isUsingXRay);
            }
        }
        else if (isUsingXRay)
        {
            isUsingXRay = false;
            GameManager.Instance.CheatXRay(isUsingXRay);
        }

    }
}
