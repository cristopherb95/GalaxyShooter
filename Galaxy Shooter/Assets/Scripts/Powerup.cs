using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    TripleShot,
    Speed,
    Shield
}

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private PowerupType powerupID;

    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case PowerupType.TripleShot:
                        player.ActivateTripleShot();
                        break;
                    case PowerupType.Speed:
                        player.ActivateSpeedBoost();
                        break;
                    case PowerupType.Shield:
                        player.ActivateShield();
                        break;

                }

            }
            Destroy(this.gameObject);
        }        
    }

}
