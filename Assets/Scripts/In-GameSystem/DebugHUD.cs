using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHUD : MonoBehaviour
{
    public PlayerController player;
    public Rigidbody2D playerRB;

    public bool show = true;
    public KeyCode toggleKey = KeyCode.F3;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            show = !show;
    }

    void OnGUI()
    {
        if (!show || player == null) return;

        float x = 20;
        float y = 20;
        float h = 22;

        GUI.Box(new Rect(x - 10, y - 10, 320, 220), "DEBUG MENU");

        GUI.Label(new Rect(x, y, 300, h), "PLAYER STATS");
        y += h;

        GUI.Label(new Rect(x, y, 300, h), "Position: " + player.transform.position);
        y += h;

        if (playerRB != null)
        {
            GUI.Label(new Rect(x, y, 300, h), "Speed: " + playerRB.velocity.magnitude.ToString("0.00"));
            y += h;
        }

        string facing = player.transform.localScale.x > 0 ? "Right" : "Left";
        GUI.Label(new Rect(x, y, 300, h), "Facing: " + facing);
        y += h;

        y += 10;

        GUI.Label(new Rect(x, y, 300, h), "INPUT BUFFER");
        y += h;

        GUI.Label(new Rect(x, y, 300, h), "Buffer: " + player.Debug_GetBufferString());
        y += h;

        GUI.Label(new Rect(x, y, 300, h), "Last Input: " + player.Debug_GetLastInputString());
        y += h;

        float buffer = player.Debug_GetBufferSecondsLeft();

        Color old = GUI.color;

        if (buffer > 0)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        GUI.Label(new Rect(x, y, 300, h), "Buffer Time Left: " + buffer.ToString("0.00"));

        GUI.color = old;
        y += h;

        GUI.Label(new Rect(x, y, 300, h),
            "Coyote Time Left: " + player.Debug_GetCoyoteSecondsLeft().ToString("0.00"));
    }
}
