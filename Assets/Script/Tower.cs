using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform basePivot, headPivot, firePoint;

    [SerializeField] GameObject shootingPrefab;
    [SerializeField] float shootingSpeed = 4;
    [SerializeField] UnityEvent onShoot;

    [SerializeField] float rotationSpeed = 8;
    [SerializeField] float rotationSoundAngleMin = 3, rotationSoundAngleMax = 90;
    [SerializeField] float rotationVolumeMax = 1;
    [SerializeField] float rotationPitchMin = 0.25f, rotationPitchMax = 1f;

    Animator animator;
    AudioSource rotationAudioSource;
    Quaternion baseLocalRotationTarget, headLocalRotationTarget;


    void OnDrawGizmos()
    {
        DrawFirePointRay(100);
    }

    void Awake()
    {
        Cache();
    }

    void Cache()
    {
        animator = GetComponent<Animator>();
        rotationAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        animator.SetFloat("ShootingSpeed", shootingSpeed);
    }

    void Update()
    {
        baseLocalRotationTarget = BaseLocalRotation();
        headLocalRotationTarget = HeadLocalRotation();

        UpdateRotationSound();

        LerpLocalRotation(basePivot, baseLocalRotationTarget);
        LerpLocalRotation(headPivot, headLocalRotationTarget);
    }



    Quaternion BaseLocalRotation()
    {
        Vector3 targetProj = MathExtension.WorldToLocal(basePivot.position, transform.rotation, target.position);
        float angle = Vector2.SignedAngle(new Vector2(targetProj.x, targetProj.z), Vector2.up);
        return Quaternion.Euler(0, angle, 0);
    }

    Quaternion HeadLocalRotation()
    {
        Quaternion baseLocalRotaionMem = basePivot.localRotation;
        basePivot.localRotation = baseLocalRotationTarget;

        Vector3 fpToHead = headPivot.position - firePoint.position;
        Vector3 headTarget = target.position + fpToHead;
        Vector3 headToTarget = headTarget - headPivot.position;
        float angle = Vector3.SignedAngle(basePivot.forward, headToTarget, basePivot.right);

        basePivot.localRotation = baseLocalRotaionMem;

        return Quaternion.Euler(angle, 0, 0);
    }

    Quaternion LerpLocalRotation(Transform transform, Quaternion localRotation)
        => transform.localRotation = Quaternion.Lerp(transform.localRotation, localRotation, rotationSpeed * Time.deltaTime);


    void UpdateRotationSound()
    {
        if (!rotationAudioSource)
            return;

        float angle = Quaternion.Angle(basePivot.localRotation, baseLocalRotationTarget) +
                      Quaternion.Angle(headPivot.localRotation, headLocalRotationTarget);

        if (angle < rotationSoundAngleMin)
        {
            if (rotationAudioSource.isPlaying)
                rotationAudioSource.Stop();
        }

        else {
            if (!rotationAudioSource.isPlaying)
                rotationAudioSource.Play();

            float proress = Mathf.InverseLerp(rotationSoundAngleMin, rotationSoundAngleMax, angle);
            rotationAudioSource.volume = rotationVolumeMax * proress;
            rotationAudioSource.pitch = Mathf.Lerp(rotationPitchMin, rotationPitchMax, proress);
        }
    }

    // Call in the shooting animation
    void Shoot()
    {
        if (shootingPrefab)
            Instantiate(shootingPrefab, firePoint.position, firePoint.rotation);

        onShoot.Invoke();
    }

    void DrawFirePointRay(float dist)
    {
        Vector3 A = firePoint.position;
        Vector3 B = Physics.Raycast(A, firePoint.forward, out RaycastHit hit, dist) ? hit.point : A + firePoint.forward * dist;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(A, B);
    }
}
