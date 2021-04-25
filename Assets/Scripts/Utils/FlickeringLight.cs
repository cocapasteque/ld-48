using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class FlickeringLight : MonoBehaviour
{
    public Light2D target;

    public float minIntensity;
    public float maxIntensity;

    public int smoothing = 5;

    private Queue<float> _smoothQueue;
    private float _lastSum = 0;

    void Awake()
    {
        target = GetComponent<Light2D>();
    }

    void Reset()
    {
        _smoothQueue.Clear();
        _lastSum = 0;
    }

    private void Start()
    {
        _smoothQueue = new Queue<float>(smoothing);
    }

    void Update()
    {
        while (_smoothQueue.Count() >= smoothing)
        {
            _lastSum -= _smoothQueue.Dequeue();
        }

        float newVal = Random.Range(minIntensity, maxIntensity);
        _smoothQueue.Enqueue(newVal);
        _lastSum += newVal;

        target.intensity = _lastSum / _smoothQueue.Count();
    }
}