using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualInputSize : MonoBehaviour
{
    public RectTransform min;
    public RectTransform max;

    public void AdjustInputRadius(float radius, float vari)
    {
        float diameter = radius * 2;

        min.sizeDelta = new Vector2(diameter - vari, diameter - vari);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(diameter, diameter);
        max.sizeDelta = new Vector2(diameter + vari, diameter + vari);

    }
}
