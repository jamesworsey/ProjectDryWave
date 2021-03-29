using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimerManager : MonoBehaviour
{
    public float minStartDelay = 0, maxStartDelay = 10f;
    private float startDelayTime;

    private float timeDelta;

    public GameObject wave;

    private void Start()
    {
        startDelayTime = Random.Range(minStartDelay, maxStartDelay);

        wave.transform.position = new Vector3(Random.Range(-100, 100), wave.transform.position.y, Random.Range(0, 200));
        Debug.Log("Time Delay " + startDelayTime + " location " + wave.transform.position);
    }

    private void Update()
    {
        timeDelta += Time.deltaTime;

        if(timeDelta >= startDelayTime)
        {
            wave.SetActive(true);
        }
    }
}
