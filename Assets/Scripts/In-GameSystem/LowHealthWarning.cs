using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowHealthWarning : MonoBehaviour
{
    [SerializeField] private Image warningImage;
    [SerializeField] private float blinkSpeed = 4f;
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.75f;

    private bool isWarningOn = false;

    private void Awake()
    {
        if (warningImage == null)
            warningImage = GetComponent<Image>();

        SetAlpha(0f);
    }

    private void Update()
    {
        if (!isWarningOn)
        {
            SetAlpha(0f);
            return;
        }

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.unscaledTime * blinkSpeed) + 1f) * 0.5f);
        SetAlpha(alpha);
    }

    public void SetWarning(bool active)
    {
        isWarningOn = active;

        if (!active)
            SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (warningImage == null) return;

        Color c = warningImage.color;
        c.a = alpha;
        warningImage.color = c;
    }
}
