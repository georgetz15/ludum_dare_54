using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Vector2 _mousePosition;
    private Transform _target;

    [SerializeField] private UnityEvent<GameObject> onPlanetSelected = new();
    [SerializeField] private UnityEvent<Transform> onPlanetHoverEnter = new();
    [SerializeField] private UnityEvent<Transform> onPlanetHoverExit = new();

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private GameObject missionsMenu;
    [SerializeField] private GameObject upgradesMenu;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnLook(InputValue value)
    {
        _mousePosition = value.Get<Vector2>();
        var target = GetMouseTarget(_camera);

        if (target == _target) return;
        if (target is null)
        {
            OnHoverExit(_target);
            _target = null;
            return;
        }

        if (!GameController.IsInPlanetsLayer(target.gameObject.layer)) return;

        _target = target;
        OnHoverEnter(_target);
    }

    private void OnHoverEnter(Transform target)
    {
        Debug.Log("Player hovered over " + target.name);
        var pc = target.GetComponent<PlanetController>();
        if (pc is null) return;
        pc.OnHoverEnter();
        onPlanetHoverEnter.Invoke(target);
    }

    private void OnHoverExit(Transform target)
    {
        Debug.Log("Player un-hovered over " + target.name);
        var pc = target.GetComponent<PlanetController>();
        if (pc is null) return;
        pc.OnHoverExit();
        onPlanetHoverExit.Invoke(target);
    }

    private void OnSelect(InputValue value)
    {
        var selected = value.Get<float>() > 0.5f;
        if (selected && _target is not null)
        {
            var pc = _target.GetComponent<PlanetController>();
            if (pc is null) return;
            onPlanetSelected.Invoke(_target.gameObject);
            pc.OnSelect();
        }
    }

    private Transform GetMouseTarget(Camera cam)
    {
        var ray = cam.ScreenPointToRay(_mousePosition);
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        return (isOverUI || !Physics.Raycast(ray, out var hit)) ? null : hit.transform;
    }

    private void OnMenu(InputValue value)
    {
        var selected = value.Get<float>() > 0.5f;

        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    private void OnInventory(InputValue value)
    {
        var selected = value.Get<float>() > 0.5f;

        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
    }

    private void OnMissions(InputValue value)
    {
        var selected = value.Get<float>() > 0.5f;

        missionsMenu.SetActive(!missionsMenu.activeSelf);
    }

    private void OnUpgrades(InputValue value)
    {
        var selected = value.Get<float>() > 0.5f;

        upgradesMenu.SetActive(!upgradesMenu.activeSelf);
    }

    private void OnSingleTouch(InputValue value)
    {
        var val = value.Get<TouchPhase>();
        Debug.Log($"Touch release value {val}");

        if (val != TouchPhase.Ended) return;
        if (!_target) return;
        
        OnHoverExit(_target);
        _target = null;
    }

    private void OnTouchMove(InputValue value)
    {
        var dir = value.Get<Vector2>();

        _mousePosition = value.Get<Vector2>();
        var target = GetMouseTarget(_camera);

        if (target == _target) return;
        if (target is null)
        {
            OnHoverExit(_target);
            _target = null;
            return;
        }

        if (!GameController.IsInPlanetsLayer(target.gameObject.layer)) return;

        _target = target;
        OnHoverEnter(_target);
    }

    private void OnTouchTap(InputValue value)
    {
        var tap = value.Get<float>() > 0.5f;
        if (!tap) return;
        
        _mousePosition = Touchscreen.current.position.ReadValue();
        var target = GetMouseTarget(_camera);

        if (target is not null)
        {
            var pc = target.GetComponent<PlanetController>();
            if (pc is null) return;
            onPlanetSelected.Invoke(target.gameObject);
            pc.OnSelect();
        }
    }
}