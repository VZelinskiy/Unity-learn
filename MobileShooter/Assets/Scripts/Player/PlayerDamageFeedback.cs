using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageFeedback : MonoBehaviour
{
    [SerializeField] Health health;

    [Header("UI Feedback (Dcreen Flash)")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float flashDuration = 0.3f;

    [Header("Audio Feedback")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] painClips;

    [Header("Camera Shake")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeIntensity = 0.15f;
    [SerializeField] private float shakeDuration = 0.2f;

    private CancellationTokenSource flashCTS;
    private CancellationTokenSource shakeCts;
    private Vector3 initialCameraLocalPos;

    private void Awake()
    {
        if (cameraTransform != null)
        {
            initialCameraLocalPos = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnTakeDamage += TriggerDamageFeedback;
            health.OnDie += HandleDie;
        }
    }

    private void OnDisable()
    {
        Cleanup();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        if (health != null)
        {
            health.OnTakeDamage -= TriggerDamageFeedback;
            health.OnDie -= HandleDie;
        }

        flashCTS?.Cancel();
        flashCTS?.Dispose();
        flashCTS = null;

        shakeCts?.Cancel();
        shakeCts?.Dispose();
        shakeCts = null;

        if (damageFlashImage != null)
        {
            damageFlashImage.gameObject.SetActive(false);
        }
    }

    private void HandleDie()
    {
        Cleanup();
    }

    // цей метод викликається, коли гравець отримує урон від ворога
    private void TriggerDamageFeedback()
    {
        PlayPainSound();
        FlashScreen().Forget();
        ShakeCamera().Forget();
    }

    private void PlayPainSound()
    {
        if (audioSource != null && painClips != null && painClips.Length > 0)
        {
            AudioClip clip = painClips[UnityEngine.Random.Range(0, painClips.Length)];
            audioSource.PlayOneShot(clip, 0.7f);
        }
    }

    private async UniTaskVoid FlashScreen()
    {
        if (damageFlashImage == null) return;

        flashCTS?.Cancel();
        flashCTS = new CancellationTokenSource();

        damageFlashImage.gameObject.SetActive(true);
        Color color = damageFlashImage.color;
        color.a = 0.6f;
        damageFlashImage.color = color;

        float elapsedTime = 0;
        try
        {
            while (elapsedTime < flashDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0.6f, 0, elapsedTime / flashDuration);

                if (damageFlashImage == null) return;

                damageFlashImage.color = color;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: flashCTS.Token);
            }
            if (damageFlashImage != null)
            {
                damageFlashImage.gameObject.SetActive(false);
            }
        }
        catch (OperationCanceledException) 
        {
            if (damageFlashImage != null)
            {
                damageFlashImage.gameObject.SetActive(false);
            }
        }
    }

    private async UniTask ShakeCamera()
    {
        if (cameraTransform == null) return;

        shakeCts?.Cancel();
        shakeCts = new CancellationTokenSource();

        float elapsedTime = 0;
        try
        {
            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;

                if (cameraTransform == null) return;

                Vector3 randomPoint = initialCameraLocalPos + UnityEngine.Random.insideUnitSphere * shakeIntensity;
                randomPoint.z = initialCameraLocalPos.z;
                cameraTransform.localPosition = randomPoint;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: shakeCts.Token);
            }
            if (cameraTransform != null)
            {
                cameraTransform.localPosition = initialCameraLocalPos;
            }
        }
        catch (OperationCanceledException) 
        { 
            if (cameraTransform != null)
            {
                cameraTransform.localPosition = initialCameraLocalPos;
            }
        }
    }
}
