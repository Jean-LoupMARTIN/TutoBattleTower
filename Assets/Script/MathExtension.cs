using UnityEngine;




public static class MathExtension
{
    // https://forum.unity.com/threads/whats-the-math-behind-transform-transformpoint.107401/
    static public Vector3 WorldToLocal(Vector3 refCenter, Quaternion refRotation, Vector3 point)
    {
        return Quaternion.Inverse(refRotation) * (point - refCenter);
    }

    static public Vector3 WorldToLocal(Transform refTransform, Vector3 point)
    {
        return Quaternion.Inverse(refTransform.rotation) * (point - refTransform.position);
    }
}
