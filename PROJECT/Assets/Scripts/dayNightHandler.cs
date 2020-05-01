using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject lightGameObject = new GameObject("The Light");
        Light lightComp = lightGameObject.AddComponent<Light>();
        lightComp.color = Color.white;
        lightComp.intensity = 1;
        lightComp.shadowStrength = 1;
        lightComp.shadowBias = 0.05f;
        lightComp.shadowNormalBias = 0.4f;
        lightComp.shadowNearPlane = 0.2f;
        lightComp.cookieSize = 10;
        lightComp.transform.position = new Vector3(960, 471, -69);
        lightComp.type = LightType.Directional;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
