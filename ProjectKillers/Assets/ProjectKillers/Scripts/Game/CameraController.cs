using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : LocalSingletonBehaviour<CameraController> {
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxCursorDistanceDelta = 5f;

    public GameObject Target;

    private void Update() {
        if (Target != null) {
            Vector3 pos = Target.transform.position;
            pos = Vector3.MoveTowards(pos, Camera.main.ScreenToWorldPoint(Input.mousePosition), maxCursorDistanceDelta);

            transform.position = Vector3.Lerp(transform.position, new Vector3(
                pos.x,
                pos.y,
                transform.position.z), Time.deltaTime * speed);
        }
    }
}
