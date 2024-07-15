using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    // 0 = Triple shot , 1 = Speed , 2 = Shield
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _audioClip;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player _player = collision.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (_player != null)
            {
                switch (_powerupID)
                {
                    case 0: 
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        _player.ShieldActive();
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
