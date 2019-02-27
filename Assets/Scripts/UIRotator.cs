using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
    public RectTransform comp1;
    public RectTransform comp2;
    public RectTransform comp3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        comp2.Rotate(new Vector3(0, 0, 60) * Time.deltaTime * TimeScale.global);
        comp1.Rotate(new Vector3(0, 0, 30) * Time.deltaTime * TimeScale.global);
        comp3.Rotate(new Vector3(0, 0, -90) * Time.deltaTime * TimeScale.global);

    }
}
