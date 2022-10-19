using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlacingObject : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public int numberOfObject;
    public float radious;
    public float floatAngle;
    public bool isTurning;
    //public float angleSection;


    public void InstantiateInCircle(GameObject prefab, Vector3 location, int howMany, float radius)
    {
        float angleSection = Mathf.PI * 2f / howMany;

        floatAngle = angleSection * Mathf.Rad2Deg;


        for (int i = 0; i < howMany; i++)
        {
            float angle = i * angleSection;
            Vector3 newPos = location + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            GameObject Cprefab =Instantiate(prefab, newPos, prefab.transform.rotation);
            Cprefab.transform.SetParent(transform);
        }
    }

    // client EXAMPLE
    private void Start()
    {
        InstantiateInCircle(prefabToInstantiate, transform.position, numberOfObject, radious);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) && !isTurning)
        {
            StartCoroutine(RotateMe(Vector3.up * floatAngle, .8f));

            // RotateLeft();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isTurning)
        {
            StartCoroutine(RotateMe(Vector3.up * -floatAngle, .8f));

            // RotateRight();
        }

    }


    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        isTurning = true;
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        transform.rotation = toAngle;
        isTurning = false;
    }
}
