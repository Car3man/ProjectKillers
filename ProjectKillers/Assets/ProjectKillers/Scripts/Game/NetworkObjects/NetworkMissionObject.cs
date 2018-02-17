using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMissionObject : MonoBehaviour {
    public enum InterpolateTypes { None, Position, Rotation, Both }

    [HideInInspector] public string ID;

    [SerializeField] private InterpolateTypes interpolateType;
    [SerializeField] protected float InterpolatePosition = 20F;
    [SerializeField] protected float InterpolateRotation = 20F;

    private Vector3 newPosition;
    private Vector3 newRotation;

    private void Start () {
        newPosition = transform.position;
        newRotation = transform.eulerAngles;
    }

    private void Update () {
        Vector3 lastPosition = transform.position;

        Interpolate();
    }

    private void Interpolate () {
        switch (interpolateType) {
            case InterpolateTypes.Position:
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * InterpolatePosition);
                break;
            case InterpolateTypes.Rotation:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * InterpolateRotation);
                break;
            case InterpolateTypes.Both:
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * InterpolatePosition);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * InterpolateRotation);
                break;
        }
    }

    public void SyncTransform (Vector3 newPosition, Vector3 newRotation) {
        this.newPosition = newPosition;
        this.newRotation = newRotation;

        switch (interpolateType) {
            case InterpolateTypes.Position:
                transform.eulerAngles = newRotation;
                break;
            case InterpolateTypes.Rotation:
                transform.position = newPosition;
                break;
            case InterpolateTypes.None:
                transform.position = newPosition;
                transform.eulerAngles = newRotation;
                break;
        }
    }
}
