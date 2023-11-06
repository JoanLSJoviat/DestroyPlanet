using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timeToDestroy;
    public AudioClip explosion;

    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(explosion);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
