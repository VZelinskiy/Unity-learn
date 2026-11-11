using UnityEngine;

public class PlatformUIController : MonoBehaviour
{
    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileJoysticksParent;

    [Header("Web/DesktopUI")]
    [SerializeField] private GameObject webControlsHint;

    private void Awake()
    {
        // перевіряємо, чи запущено проект у браузері (WebGL)
#if UNITY_WEBGL || UNITY_EDITOR
        if (mobileJoysticksParent != null) mobileJoysticksParent.SetActive(false);
        if (webControlsHint != null) webControlsHint.SetActive(true);
#else
        // для мобільних пристроїв
        if (mobileJoysticksParent != null) mobileJoysticksParent.SetActive(true);
        if (webControlsHint != null) webControlsHint.SetActive(false);
#endif
    }
}
