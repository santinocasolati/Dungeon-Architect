using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBarUpdate : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        DungeonLevelManager._instance.xpModified += UpdateXpBar;
    }

    private void UpdateXpBar(int maxXp, int currentXp)
    {
        StartCoroutine(LerpSlider(maxXp, currentXp));
    }

    private IEnumerator LerpSlider(int maxXp, int currentXp)
    {
        float startValue = slider.value;
        float targetValue = ((float)currentXp / maxXp) * 100f;
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
