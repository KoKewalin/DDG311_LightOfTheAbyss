using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeugMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DebugHUD debugHUD;
    [SerializeField] private GameObject debugFeaturePanel;

    [Header("Key")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F3;

    private bool isOpen = false;

    private void Start()
    {
        isOpen = false;

        if (debugHUD != null)
            debugHUD.SetShow(false);

        if (debugFeaturePanel != null)
            debugFeaturePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isOpen = !isOpen;

            if (debugHUD != null)
                debugHUD.SetShow(isOpen);

            if (debugFeaturePanel != null)
                debugFeaturePanel.SetActive(isOpen);
        }
    }
}
