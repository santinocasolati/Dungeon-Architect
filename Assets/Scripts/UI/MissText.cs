using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissText : MonoBehaviour
{
    private TMPro.TMP_Text text;
    private float fadeDuration = .5f;
    private float moveSpeed = .5f;
    private float moveDistance = .25f;

    private float fadeTimer = 0f;
    private float moveTimer = 0f;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        text = GetComponent<TMPro.TMP_Text>();
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.anchoredPosition -= new Vector2(0, moveDistance);

        fadeTimer = 0;
        moveTimer = 0;

        FadeIn();
    }

    private void FadeIn()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    private void Update()
    {
        if (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }

        if (moveTimer < fadeDuration)
        {
            moveTimer += Time.deltaTime;
            rectTransform.anchoredPosition += new Vector2(0f, moveSpeed * Time.deltaTime);
        } else
        {
            gameObject.SetActive(false);
        }

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
