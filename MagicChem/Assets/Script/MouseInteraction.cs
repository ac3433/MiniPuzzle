using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CircleData))]
public class MouseInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    //only 1 object should be dragged
    public static GameObject DraggedObject;

    [SerializeField]
    private float _moveSpeed = 1f;
    private Vector3 _startPosition;
    private Transform _orignalParent;
    private Vector3 _originalPosition;
    private float _originalRotation;
    private Transform _startParent;

    private Vector2 _startCollider;
    [SerializeField]
    private BoxCollider2D _collider; //this should be the sprite collider not the gameobject using this script. For layer issue

    private CircleData _circleData;
    private bool _lockRotation; //allow rotation only on the drop location

    void Start()
    {
        _startCollider = _collider.size;
        _circleData = GetComponent<CircleData>();
        _lockRotation = false;
        _orignalParent = transform.parent;
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation.eulerAngles.z;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DraggedObject = gameObject;
        _startParent = transform.parent;
        _startPosition = transform.position;
        _collider.size = new Vector2(0, 0);
        _circleData.TurnLightOff();
        _circleData.RemoveNextCircleData();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), _moveSpeed);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggedObject = null;
        if (transform.parent == _startParent)
            transform.position = _startPosition;
        else
        {
            transform.localPosition = Vector3.zero;
            GridLevelManager.Instance.SetCircleData(gameObject, _circleData.GetPoint());
            
            _lockRotation = true;
        }
        _circleData.CheckSurrounding();
        _collider.size = _startCollider;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (_lockRotation)
        //    {
        //        transform.Rotate(Vector3.forward, -90);
        //        _circleData.RotateClockwise();
        //        _circleData.RemoveNextCircleData();
        //        _circleData.CheckSurrounding();
        //        GridLevelManager.Instance.PrintMap();
        //    }
        //}

        if(Input.GetMouseButtonUp(1))
        {
            ResetPosition();
        }

    }

    public void ResetPosition()
    {
        transform.parent = _orignalParent;
        transform.localPosition = _originalPosition;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(Vector3.forward, _originalRotation);
        _circleData.RemoveNextCircleData();
        SetPoint(null);
    }

    public void SetPoint(Point p)
    {
        _circleData.SetPoint(p);
    }
}
