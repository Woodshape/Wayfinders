using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public float moveSpeed;

    public Transform target;

    public Camera _mainCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position,
            new Vector3(target.position.x, target.position.y, -10f),
            moveSpeed * Time.deltaTime);
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
