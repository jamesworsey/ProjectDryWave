using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelWaveManager : MonoBehaviour
{
    private Animator waveAnimator;
    private AnimatorStateInfo animationState;
    private AnimatorClipInfo[] animationClipInfo;
    private float currentClipTime = 0.0f;

    public Transform effectLocation;
    public GameObject effectPrefab;
    public float effectSpawnTime = 6.5f;
    private bool effectSpawned = false;

    public GameObject sprayPrefab;
    public Transform sprayLocation;

    public GameObject faceSpray;
    public Transform faceLocation;

    private AudioSource audioSource;

    public float getWaveTimer()
    {
        return currentClipTime;
    }
    public float getBreakTime()
    {
        return effectSpawnTime;
    }

    void Start()
    {
        waveAnimator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        animationState = waveAnimator.GetCurrentAnimatorStateInfo(0);
        animationClipInfo = waveAnimator.GetCurrentAnimatorClipInfo(0);

        currentClipTime = animationClipInfo[0].clip.length * animationState.normalizedTime;
    }

    
    void Update()
    {
        animationState = waveAnimator.GetCurrentAnimatorStateInfo(0);
        animationClipInfo = waveAnimator.GetCurrentAnimatorClipInfo(0);

        currentClipTime = animationClipInfo[0].clip.length * animationState.normalizedTime;

        if(currentClipTime > effectSpawnTime && !effectSpawned)
        {
            GameObject effect = Instantiate(effectPrefab, effectLocation.position, effectLocation.localRotation);
            effect.transform.SetParent(transform, true);
            effect.transform.localScale = new Vector3(1, 1, 1);
            effectSpawned = true;

            Quaternion sprayRot = Quaternion.Euler(0, 90, 0);
            GameObject spray = Instantiate(sprayPrefab, sprayLocation.position, sprayRot);
            spray.transform.SetParent(transform, true);
            spray.transform.localScale = new Vector3(1, 1, 1);

            GameObject fSpray = Instantiate(faceSpray, faceLocation.position, sprayRot);
            fSpray.transform.SetParent(transform, true);
            fSpray.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
