using EZCameraShake;
using UnityEngine;



public class ShakeCamSphereOnEnable : MonoBehaviour
{
    [SerializeField] float radius = 5;
    [SerializeField] float magnitude = 5;
    [SerializeField] float time = 0.5f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnEnable()
    {
        ShakeCamSphere(transform.position, radius, magnitude, time);
    }

    static public void ShakeCamSphere(Vector3 center, float radius, float magnitude, float time)
    {
        CameraShaker cam = CameraShaker.Instance;

        if (!cam)
            return;

        float dist = (cam.transform.position - center).magnitude;

        if (dist >= radius)
            return;

        float progress = 1 - dist / radius;

        cam.ShakeOnce(magnitude * progress, magnitude * progress, 0, time * progress);
    }
}
