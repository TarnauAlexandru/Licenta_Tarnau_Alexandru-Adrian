using UnityEngine;
using UnityEngine.UI;

public class MicroBarReceiver : MonoBehaviour
{
    [Tooltip("Copilul Image care se umple (ex. „HP Bar (Image)”).")]
    public Image fillImage;

    public void Set01(float v)
    {
        v = Mathf.Clamp01(v);

        if (fillImage) fillImage.fillAmount = v;
    }
}
