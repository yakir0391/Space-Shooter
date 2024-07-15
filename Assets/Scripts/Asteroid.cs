using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 19f;
    [SerializeField]
    private GameObject _explosionPerfab;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("the audio source on player is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,1) * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPerfab, transform.position, Quaternion.identity);
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            Destroy(other.gameObject);
            Destroy(this.gameObject , 0.25f);
            _spawnManager.StartSpawn();
            

        }
    }

}
