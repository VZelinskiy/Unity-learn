using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI waveInfoText;

    private void Awake()
    {
        if (waveManager == null)
        {
            waveManager = FindAnyObjectByType<WaveManager>();
        }
    }

    private void OnEnable()
    {
        if(waveManager != null)
        {
            waveManager.OnWaveProgressChanged += UpdateWaveUI;
            waveManager.OnAllWavesCompleted += HandleVictory;
        }
    }

    private void OnDisable()
    {
        if (waveManager != null)
        {
            waveManager.OnWaveProgressChanged -= UpdateWaveUI;
            waveManager.OnAllWavesCompleted -= HandleVictory;
        }
    }

    private void UpdateWaveUI(int waveNumber, int remainingEnemies)
    {
        if (waveInfoText != null)
        {
            waveInfoText.text = $"Wave: {waveNumber} | Enemies: {remainingEnemies}";
        }
    }

    private void HandleVictory()
    {
        if (waveInfoText != null)
        {
            waveInfoText.text = "";
        }
    }
}
