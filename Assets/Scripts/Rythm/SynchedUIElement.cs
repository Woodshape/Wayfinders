using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SynchedUIElement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, Conductor.Instance.loopPositionInAnalog));
    }
}
