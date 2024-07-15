using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;
    private float _speedMulti = 2f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVis;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_spawnManager == null)
        {
            Debug.LogError("the spawn manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("the ui manager is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("the audio source on player is null");
        }
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

        //new Vector3(1,0,0) * real Time (not frames)
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime); // 60 frames per sec & Vector3.right = new Vector3(1,0,0)
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // the two from above and the two from below are the same

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        
        // making territory for y
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);    , like the if else statement

        // making territory for x
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
         _canFire = Time.time + _fireRate;

         if (_isTripleShotActive == true)
         {
             Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
         }
         else
         {
             Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
         }

        _audioSource.clip = _laserSound;
        _audioSource.Play();
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldVis.SetActive(false);
            _isShieldActive = false;
            return;
        }

        this._lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (this._lives < 1) 
        {
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            _uiManager.UpdateGameOver();
            _spawnManager.onPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        this._speed *= _speedMulti;
        StartCoroutine(SpeedBoostDownRoutine());
    }
    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        this._speed /= _speedMulti;
    }
    public void ShieldActive()
    {
        this._isShieldActive = true;
        _shieldVis.SetActive(true);
    }

    public void ScoreAdd(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
