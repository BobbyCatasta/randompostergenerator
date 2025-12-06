using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles cheat functionality (editor-only).
/// </summary>
public class CheatScript : MonoBehaviour
{
#if UNITY_EDITOR
    /// <summary>
    /// Checks for cheat key inputs each frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            GameManager.Instance.ToggleXRay();
    }
#endif
}