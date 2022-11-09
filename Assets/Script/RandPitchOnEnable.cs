using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPitchOnEnable : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float pitchMin = 0.9f, pitchMax = 1.1f;

    void OnEnable()
    {
        audioSource.pitch = Mathf.Lerp(pitchMin, pitchMax, Random.value);
    }
}
