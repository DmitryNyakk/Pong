using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketkaControl : MonoBehaviour {
    public Transform _leftBoard;
    public Transform _rightBoard;
    public float Speed = 5f;
    public bool IsTwoPlayer;

    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    private Rigidbody _rocketka1Rigidbody;
    private Rigidbody _rocketka2Rigidbody;

    private GameMaster _gameMaster;
    private bool _isMobile;
    private bool _isSwiping;
    [SerializeField] private Vector2 _tapPosition;
    [SerializeField] private float _swipeDeltaX;
    [SerializeField] private float _swipeDeltaXOld;
    [SerializeField] private float _direction;

    private void Start() {
        _isMobile = Application.isMobilePlatform;
        _gameMaster = FindObjectOfType<GameMaster>();
        _leftBoard = _gameMaster.LeftBoard.transform;
        _rightBoard = _gameMaster.RightBoard.transform;
        _rocketka1Rigidbody = _gameMaster.Rocketka1.GetComponent<Rigidbody>();
        _rocketka2Rigidbody = _gameMaster.Rocketka2.GetComponent<Rigidbody>();

        _maxX = _rightBoard.position.x - (_leftBoard.localScale.x * 0.5f) - (_rocketka1Rigidbody.transform.Find("Flat").transform.localScale.x * 0.5f);
        _minX = _leftBoard.position.x + (_rightBoard.localScale.x * 0.5f) + (_rocketka1Rigidbody.transform.Find("Flat").transform.localScale.x * 0.5f);
    }
    private void Update() {
        MakeSwaiping();
        ReturnRocketka(_rocketka1Rigidbody.transform.position.x);
    }

    private void FixedUpdate() {
        _rocketka1Rigidbody.MovePosition(_rocketka1Rigidbody.transform.position + _rocketka1Rigidbody.transform.right * Speed * _direction * Time.deltaTime);
        if (!IsTwoPlayer) {
            _rocketka2Rigidbody.MovePosition(_rocketka2Rigidbody.transform.position + _rocketka2Rigidbody.transform.right * Speed * _direction * Time.deltaTime);
        }
    }

    public void MakeSwaiping() {// Управление свайпом
        if (!_isMobile) {
            if (Input.GetMouseButtonDown(0)) {
                _isSwiping = true;
                _tapPosition = Input.mousePosition;
            } else if (Input.GetMouseButtonUp(0)) {
                ResetSwipe();
            }
        } else {
            if (Input.touchCount > 0) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    _isSwiping = true;
                    _tapPosition = Input.GetTouch(0).position;
                } else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended) {
                    ResetSwipe();
                }
            }
        }

        CheckSwipe();
    }

    private void CheckSwipe() {
        _swipeDeltaX = 0f;
        if (_isSwiping) {
            if (!_isMobile && Input.GetMouseButton(0)) {// Свайп нужен только горизонтальный поэтому высчитываем значение по X
                _swipeDeltaX = Input.mousePosition.x - _tapPosition.x;
            } else if (Input.touchCount > 0) {
                _swipeDeltaX = Input.GetTouch(0).position.x - _tapPosition.x;
            }
        }

        float distanceLong = _swipeDeltaX - _swipeDeltaXOld;
        if (Mathf.Abs(distanceLong) > 0f) {// Проверяю достаточна ли длинна свайпа
            // Debug.Log(distanceLong);
            float coof = (distanceLong / Screen.width);// Отношение длинны движения по свайпу к ширине экрана
            _direction = Mathf.Clamp(coof, -1, 1);
            //Debug.Log(coof);
            //  ResetSwipe();
        } else {
            _rocketka1Rigidbody.velocity = new Vector3(0f, 0f, 0f);
            _direction = 0f;
        }
    }

    private void ResetSwipe() {
        _isSwiping = false;
        _tapPosition = Vector3.zero;
        _swipeDeltaX = 0f;
        _rocketka1Rigidbody.velocity = new Vector3(0f, 0f, 0f);
        _direction = 0f;
    }

    public void ReturnRocketka(float x) { // Ограничение движения ракетки по X
        if (x > _maxX) {
            _rocketka1Rigidbody.transform.position = new Vector3(_maxX, _rocketka1Rigidbody.transform.position.y, _rocketka1Rigidbody.transform.position.z);
        }
        if (x < _minX) {
            _rocketka1Rigidbody.transform.position = new Vector3(_minX, _rocketka1Rigidbody.transform.position.y, _rocketka1Rigidbody.transform.position.z);           
        }

        if (!IsTwoPlayer) {
            _rocketka2Rigidbody.transform.position = new Vector3(_rocketka1Rigidbody.transform.position.x, _rocketka2Rigidbody.transform.position.y, _rocketka2Rigidbody.transform.position.z);
        }
    }

}
