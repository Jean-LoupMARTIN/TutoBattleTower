using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TargetPivot : MonoBehaviour
{
    [SerializeField] List<Transform> points;
    Transform crtPoint;

    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float changePointTime = 2;

    void OnEnable()
    {
        crtPoint = points[(int)(points.Count * Random.value)];
        StartCoroutine("ChangePoint");
    }

    void OnDisable()
    {
        StopCoroutine("ChangePoint");
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(crtPoint.position, transform.position), rotationSpeed * Time.deltaTime);
    }

    IEnumerator ChangePoint()
    {
        while (true)
        {
            Transform newPoint = points[(int)(points.Count * Random.value)];

            while (newPoint == crtPoint)
                newPoint = points[(int)(points.Count * Random.value)];

            crtPoint = newPoint;

            yield return new WaitForSeconds(changePointTime);
        }
    }
}
