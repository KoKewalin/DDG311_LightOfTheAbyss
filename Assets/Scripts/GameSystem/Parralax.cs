using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parralax : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private float horizontalSpeed = 100f;
    [SerializeField] private float verticalSpeed = 0f;

    [SerializeField] private float parallaxMultiplier = 0.5f;

    [Space(10)]
    [SerializeField] private bool useHorizontal = true;
    [SerializeField] private bool useVertical = false;

    private RectTransform rectTransform;
    private float width;
    private float height;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        if (useHorizontal)
        {
            movement.x -= horizontalSpeed * parallaxMultiplier * Time.deltaTime;
        }

        if (useVertical)
        {
            movement.y -= verticalSpeed * parallaxMultiplier * Time.deltaTime;
        }

        rectTransform.anchoredPosition += movement;

        Vector2 pos = rectTransform.anchoredPosition;

        if (useHorizontal && pos.x <= -width)
        {
            pos.x += width;
        }

        if (useVertical && pos.y <= -height)
        {
            pos.y += height;
        }

        rectTransform.anchoredPosition = pos;
    }
}
