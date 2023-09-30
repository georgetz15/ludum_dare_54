using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _mousePosition;
    [SerializeField] private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnLook(InputValue value)
    {
        _mousePosition = value.Get<Vector2>();
    }

    private void OnSelect(InputValue value)
    {
        bool selected = value.Get<float>() > 0.5f;
        if (selected)
        {
            var target = GetMouseTarget(_camera);
            if (target)
            {
                Debug.Log("Player clicked on " + target.name);
            }
        }
    }
    
    private Transform GetMouseTarget(Camera cam)
    {
        var ray = cam.ScreenPointToRay(_mousePosition);
        return !Physics.Raycast(ray, out var hit) ? null : hit.transform;
    }
}
