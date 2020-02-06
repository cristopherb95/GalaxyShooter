using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTrippleShootAcitve = false;
    private bool _isSpeedBoostAcitve = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject _shieldVisualizer;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    private bool _speedBoostCooldownActive = false;
    private bool _tripleShotCooldownActive = false;

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
            Debug.LogError("The spawn manager is NULL");

        if (!_uiManager)
            Debug.LogError("UI Manager is NULL");

        if (!_audioSource)
            Debug.LogError("AudioSource on the player is NULL");
        else
            _audioSource.clip = _laserSoundClip;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontalInput, verticalInput, 0);

        if (!_isSpeedBoostAcitve)
            transform.Translate(direction * _speed * Time.deltaTime);
        else
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.0f, 0), 0);

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        // Quaternion.identity - default rotation
        if (_isTrippleShootAcitve)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);

        _audioSource.Play();
    }

    public void ActivateTripleShot()
    {
        _isTrippleShootAcitve = true;

        if (_tripleShotCooldownActive == true)
        {
            StopCoroutine("TripleShotPowerDownRoutine");
        }

        StartCoroutine("TripleShotPowerDownRoutine");
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        _tripleShotCooldownActive = true;
        yield return new WaitForSeconds(5.0f);
        _isTrippleShootAcitve = false;
        _tripleShotCooldownActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostAcitve = true;

        if (_speedBoostCooldownActive == true)
        {
            StopCoroutine("SpeedBoostPowerDownRoutine");
        }

        StartCoroutine("SpeedBoostPowerDownRoutine");
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        _speedBoostCooldownActive = true;
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostAcitve = false;
        _speedBoostCooldownActive = false;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore();
            Destroy(this.gameObject);
        }
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public int getPlayerScore()
    {
        return _score;
    }



}