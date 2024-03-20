using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUpdate : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        DungeonHPManager._instance.DungeonHPModified += UpdateHPBar;
    }

    private void UpdateHPBar(int maxHP, int currentHP)
    {
        StartCoroutine(LerpSlider(maxHP, currentHP));
    }

    private IEnumerator LerpSlider(int maxHP, int currentHP)
    {
        float startValue = slider.value;
        float targetValue = ((float)currentHP / maxHP) * 100f;
        float distance = Mathf.Abs(targetValue - startValue);

        float lerpSpeed = 5f * (distance + 1f);

        float lerpDuration = distance / lerpSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            slider.value = Mathf.Lerp(startValue, targetValue, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slider.value = targetValue;
    }
}
