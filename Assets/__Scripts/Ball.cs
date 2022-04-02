using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public Rigidbody BallRigidbody;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private GameObject _sphera;
    [SerializeField] private GameObject _arrow;

    private float _minY;
    private float _maxY;
    [SerializeField] private bool _isStart;
    private GameMaster _gameMaster;
    [SerializeField] private float _rndAngle;

    void Start() {
        IniciateBall();
    }   

    public void IniciateBall() {
        _gameMaster = FindObjectOfType<GameMaster>();
        _minY = _gameMaster.Rocketka1.transform.position.y - 1f;
        _maxY = _gameMaster.Rocketka2.transform.position.y + 1f;       
        StartCoroutine(InciaciateCoroutine());


    }

    private void Update() {
        if (transform.position.y > _maxY) { // ѕроверка на вылет шарика с пол€
            _gameMaster.AddScore();
            Destroy(this.gameObject);
        }
        if (transform.position.y < _minY) {
            _gameMaster.MinusScore();
            Destroy(this.gameObject);
        }

        if (_isStart) { // ѕлавный разворот шарика на нужный угол со стрелкой
            transform.Rotate(0, 0, 3, Space.World);
            if (transform.eulerAngles.z >= _rndAngle) {              
                _isStart = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {         
        Vector3 norm = collision.contacts[0].point.normalized;
        transform.right = BallRigidbody.velocity;
    }


    IEnumerator InciaciateCoroutine() {// ¬ этой корутине задаютс€ значени€ угла, скорости и делаетс€ небольша€ пауза . „то бы игрок мог сообразить, что происходит
        _arrow.SetActive(true);
        _isStart = true;
        _rndAngle = Random.Range(27f, 330f);
        _speed = Random.Range(1f, 1.7f);
        float sizeSphera = Random.Range(0.5f,1.1f);
        _sphera.transform.localScale = new Vector3(sizeSphera, sizeSphera, sizeSphera);

        yield return new WaitForSeconds(1.85f);
        _arrow.SetActive(false);
        BallRigidbody.AddRelativeForce(_speed, 0f, 0f, ForceMode.Impulse);
    }

    public void SetColor(Color color) {
        _sphera.GetComponent<Renderer>().material.color = color;
    }
}
