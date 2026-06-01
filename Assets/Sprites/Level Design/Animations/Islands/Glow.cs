using UnityEngine;
using UnityEngine.UI;

//Магиние кнопки
public class GlowPulse : MonoBehaviour
{
    public Image glowImage;
    public float speed = 2f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.8f;

    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, 
                      (Mathf.Sin(Time.time * speed) + 1f) / 2f);
        Color c = glowImage.color;
        c.a = alpha;
        glowImage.color = c;
    }
}