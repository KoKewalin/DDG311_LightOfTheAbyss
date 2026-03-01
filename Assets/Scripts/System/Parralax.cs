using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parralax : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 100f;
    [SerializeField] private float parallaxMultiplier = 0.5f;

    private RectTransform rectTransform;
    private float width;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width;
    }

    void Update()
    {
        rectTransform.anchoredPosition +=
            Vector2.left * scrollSpeed * parallaxMultiplier * Time.deltaTime;

        // Loop
        if (rectTransform.anchoredPosition.x <= -width)
        {
            rectTransform.anchoredPosition += new Vector2(width, 0f);
        }
    }
}
