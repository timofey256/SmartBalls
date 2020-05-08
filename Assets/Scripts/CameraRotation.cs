using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{ 
    public float mouseSensitivity = 4f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;
 
    private Transform _XForm_Camera;
    private Transform _XForm_Parent;
 
    private Vector3 _localRotation;
    private float _cameraDistance = 5f;
 
    // Use this for initialization
    void Start() {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
    }
 
    void LateUpdate() {
        // Rotation of the Camera based on Mouse Coordinates
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
            _localRotation.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
 
            // Clamp the y Rotation to horizon and not flipping over at the top
            if (_localRotation.y < 0f)
                _localRotation.y = 0f;
            else if (_localRotation.y > 90f)
                _localRotation.y = 90f;
            }

        // Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_localRotation.y, _localRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);
 
        if ( this._XForm_Camera.localPosition.z != this._cameraDistance * -1f )
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, 
                                                        this._cameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}