using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebygHUD : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;      // drag your Player here

    [Header("Toggle")]
    public KeyCode toggleKey = KeyCode.F3;
    public bool show = true;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            show = !show;
    }

    private void OnGUI()
    {
        if (!show || player == null) return;

        float x = 15f, y = 15f, w = 480f, h = 22f;

        GUI.Box(new Rect(x - 10, y - 10, w + 20, 170), "DEBUG: Input Buffer");

        GUI.Label(new Rect(x, y, w, h), "Buffer: " + player.Debug_GetBufferString()); y += h;
        GUI.Label(new Rect(x, y, w, h), "Last Input: " + player.Debug_GetLastInputString()); y += h;
        GUI.Label(new Rect(x, y, w, h), "Buffer Time Left: " + player.Debug_GetBufferSecondsLeft().ToString("0.00") + "s"); y += h;
        GUI.Label(new Rect(x, y, w, h), "Coyote Time Left: " + player.Debug_GetCoyoteSecondsLeft().ToString("0.00") + "s"); y += h;
    }
}
