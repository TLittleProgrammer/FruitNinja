using System;
using UnityEngine;

namespace Runtime
{
    public class CameraViewChanger : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        
        private void Awake()
        {
            _target.position = new(_camera.orthographicSize * ((float)Screen.width / Screen.height), 0f, 0f);
        }
    }
}