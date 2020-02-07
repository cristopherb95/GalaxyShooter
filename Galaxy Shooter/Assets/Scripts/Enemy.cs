using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _speedAddition = 0.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private Player _player;

    private Animator _anim;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (!_player)
            Debug.LogError("Player is NULL");

        if (!_anim)
            Debug.LogError("Animator is null");

        if (!_audioSource)
            Debug.LogError("AudioSource on the enemy is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        // Prevent shooting if enemy death animation is triggered
        if (Time.time > _canFire && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Destroyed_anim"))
        {
            _fireRate = Random.Range(3, 8);
            _canFire +=Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_speed + _speedAddition) * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 6.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
                player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            this._speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }

        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddToScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            this._speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
        // Debug.Log("Hit: " + other.transform.name);
    }

    public void IncreaseSpeed(float speedValue)
    {
        _speedAddition += speedValue;
    }
}
