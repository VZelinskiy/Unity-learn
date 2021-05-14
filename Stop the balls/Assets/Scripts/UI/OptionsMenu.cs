using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [System.Serializable]
    public class FontColor
    {
        public LocalizedString name;
        public Color color;
    }

    [SerializeField] FontColor[] fontColors;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown fontColorDropdown;
    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] string musicParameterName;
    [SerializeField] string SFXParameterName;

    private Text[] textComponents;
    private TextMeshProUGUI[] textMeshProComponents;
    private List<int> basicTextComponentsFontSizes;
    private List<int> basicTextMeshProComponentsFontSizes;

    // Start is called before the first frame update
    void Start()
    {
        //TO DO
        
    }

    public void HandleBackClick()
    {
        UIManager.Instance.SetActiveOptionsMenu(false);

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            UIManager.Instance.SetActiveMainMenu(true);
        }
        else if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
        {
            UIManager.Instance.SetActivePauseMenu(true);
        }
    }

    public void SetFontSize(float sizeMultiplayer)
    {
        Text currentTextComponent;
        TextMeshProUGUI currentTextMeshProComponent;

        for (int i = 0; i < textComponents.Length; i++)
        {
            currentTextComponent = textComponents[i];

#if UNITY_EDITOR
            if (!EditorUtility.IsPersistent(currentTextComponent))
            {
                currentTextComponent.fontSize = Mathf.RoundToInt(basicTextComponentsFontSizes[i] * sizeMultiplayer);
            }
#else
            currentTextComponent.fontSize = Mathf.RoundToInt(basicTextComponentsFontSizes[i] * sizeMultiplayer);
#endif
        }

        for (int i = 0; i < textMeshProComponents.Length; i++)
        {
            currentTextMeshProComponent = textMeshProComponents[i];

#if UNITY_EDITOR
            if (!EditorUtility.IsPersistent(currentTextMeshProComponent))
            {
                currentTextMeshProComponent.fontSize = Mathf.RoundToInt(basicTextMeshProComponentsFontSizes[i] * sizeMultiplayer);
            }
#else
            currentTextMeshProComponent.fontSize = Mathf.RoundToInt(basicTextMeshProComponentsFontSizes[i] * sizeMultiplayer);
#endif
        }
    }

    public void SetFontColor(int colorIndex)
    {
        Text currentTextComponent;
        TextMeshProUGUI currentTextMeshProComponent;

        for (int i = 0; i < textComponents.Length; i++)
        {
            currentTextComponent = textComponents[i];

#if UNITY_EDITOR
            if (!EditorUtility.IsPersistent(currentTextComponent))
            {
                currentTextComponent.color = fontColors[colorIndex].color;
            }
#else
        currentTextComponent.color = fontColors[colorIndex].color;
#endif
        }

        for (int i = 0; i < textMeshProComponents.Length; i++)
        {
            currentTextMeshProComponent = textMeshProComponents[i];

#if UNITY_EDITOR
            if (!EditorUtility.IsPersistent(currentTextMeshProComponent))
            {
                currentTextMeshProComponent.color = fontColors[colorIndex].color;
            }
#else
        currentTextMeshProComponent.color = fontColors[colorIndex].color;
#endif
        }
    }

    public void SetLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        UpdateFontColorDropdownLocalization();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicParameterName, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFXParameterName, volume);
    }

    private IEnumerator FillFontColorDropdown()
    {
        fontColorDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < fontColors.Length; i++)
        {
            var localizedTextHandle = fontColors[i].name.GetLocalizedString();
            yield return localizedTextHandle;
            //TO DO
            //string option = localizedTextHandle.Result;

            //options.Add(option);
        }

        fontColorDropdown.AddOptions(options);
    }

    private void FillBasicFontSizes()
    {
        for (int i = 0; i < textComponents.Length; i++)
        {
            basicTextComponentsFontSizes.Add(textComponents[i].fontSize);
        }

        for (int i = 0; i < textMeshProComponents.Length; i++)
        {
            basicTextMeshProComponentsFontSizes.Add(Mathf.RoundToInt(textMeshProComponents[i].fontSize));
        }
    }

    private IEnumerator FillLanguageDropdown()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        languageDropdown.ClearOptions();
        List<string> options = new List<string>();

        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            string option = LocalizationSettings.AvailableLocales.Locales[i].Identifier.CultureInfo.NativeName;

            if (LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName == option)
                selected = i;
            options.Add(option);
        }

        languageDropdown.AddOptions(options);
        languageDropdown.value = selected;
        languageDropdown.RefreshShownValue();
    }

    private void UpdateFontColorDropdownLocalization()
    {
        for (int i = 0; i < fontColorDropdown.options.Count; ++i)
        {
            var optionI = i;
            var option = fontColors[i];

            var localizedTextHandle = option.name.GetLocalizedString();
            //TO DO
            /*localizedTextHandle.Completed += (handle) =>
            {
                fontColorDropdown.options[optionI].text = handle.Result;

                if (optionI == fontColorDropdown.value)
                {
                    fontColorDropdown.captionText.text = handle.Result;
                }
            };*/
        }
    }
}
