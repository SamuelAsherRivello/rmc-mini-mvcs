using System.Collections;
using System.Collections.Generic;
using RMC.MiniMvcs.Samples.Configurator.Mini;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Awake()
    {
        TestIt();
    }

    private void TestIt()
    {
        Debug.Log("Test found: " + ConfiguratorMiniSingleton.Instance.gameObject.GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestIt();
        }
    }
}
