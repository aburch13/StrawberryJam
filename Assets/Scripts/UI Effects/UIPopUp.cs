using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIPopUp : MonoBehaviour
{
    [SerializeField]
    float scaleMin, scalePeak;
    [SerializeField]
    float alphaStart;
    [SerializeField]
    float rate;
    [SerializeField]
    Image[] scaleTargets;
    [SerializeField]
    Image[] alphaTargets;

    public float t = -1, targetScale, targetAlpha, usedRate;

    private enum mode { POPUP, VANISH, SCALEONLY, INACTIVE};
    private mode currMode;

    // Update is called once per frame
    void Update()
    {
        if (t == -1) return;
        LerpAlpha();
        LerpScale();
    }

    public void PopUp()
    {
        currMode = mode.POPUP;
        t = 0;
        usedRate = rate;
        targetScale = scalePeak;
        targetAlpha = 1;
    }

    public void Vanish()
    {
        currMode = mode.SCALEONLY;
        t = 0;
        usedRate = 3 * rate;
        targetScale = scalePeak;
        targetAlpha = alphaStart;
    }

    private void LerpAlpha()
    {
        if (currMode == mode.SCALEONLY) return;
        for(int i = 0; i < alphaTargets.Length; i++)
        {
            alphaTargets[i].color = new Color(1, 1, 1, Mathf.Lerp(alphaTargets[i].color.a, targetAlpha, t));
        }
    }

    private void LerpScale()
    {
        float f;
        for(int i = 0; i < scaleTargets.Length; i++)
        {
            f = Mathf.Lerp(scaleTargets[i].rectTransform.localScale.x, targetScale, t);
            scaleTargets[i].rectTransform.localScale = new Vector3(f, f, f);
        }
        t += Time.deltaTime * usedRate;
        if (t > 1)
        {
            if(currMode == mode.POPUP && targetScale == scalePeak)
            {
                targetScale = 1;
                t = 0;
                usedRate = 3 * rate;
            }
            else if(currMode == mode.SCALEONLY && targetScale == scalePeak)
            {
                targetScale = scaleMin;
                t = 0;
                usedRate = rate;
                currMode = mode.VANISH;
            }
            else
            {
                t = -1;
                currMode = mode.INACTIVE;
            }
        }

    }
}
