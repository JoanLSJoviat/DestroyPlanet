using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float timeToExplosion = 4.0f;
    private float timer = 0.0f;
    private GameManager gm = null;
    public GameObject prefabExplosion;
    private AudioSource audioSource;
    private GameObject explosions;
    private bool bombActive = true;
    public AudioClip time, deactivation;
    
    void Start()
    {
        explosions = GameObject.Find("/Bombs/Explosions");
        GameObject o = GameObject.FindGameObjectWithTag("GameManager");
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(time);  //en instanciar-se un coet sonarà el sò del compte enrere per a l'explosió
        
        if (o == null)
        {
            Debug.LogError("There's no gameObject with GameManager tag.");
        }
        else
        {
            gm = o.GetComponent<GameManager>();
            if (gm == null)
            {
                Debug.LogError("The GameObject with GameManager tag doesn't have the GameManager script attached to it");
            }
        }
        
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
    
    void Update()
    {
        
                                     //Si han passat 4 segons i el coet no s'ha desactivat, aquest explotarà 
        timer += Time.deltaTime;
  
        if (timer > timeToExplosion && bombActive)
        {
            timer = 0.0f;
            Explode();
        }

                            //si no s'ha desactivat el coet, el color anirà canviant a vermell
        if (bombActive)
        {
            Color newColor = Color.Lerp(Color.white, Color.red, timer / timeToExplosion);
            GetComponent<MeshRenderer>().material.color = newColor;
        }
        
    }
    
  /**
   * Si el jugador fa click sobre el coet, s'afegeix un punt al comptador, es reestableix el color a blanc,
   *es reprodueix el sò de desactivació i es destrueix el coet quan el sò ha acabat de reproduir-se
   */
    private void OnMouseDown()
    {
        gm.AddScore();
        GetComponent<MeshRenderer>().material.color = Color.white;
        audioSource.PlayOneShot(deactivation);
        bombActive = false;
        Destroy(gameObject, deactivation.length);
       
    }

  /**
   * Funció per a fer explotar el coet, restant una vida al jugador i instanciant una explosió
   */

    void Explode()
    {
        gm.TakeDamage();
        GameObject.Instantiate(prefabExplosion, transform.position, Quaternion.identity, explosions.transform);
        Destroy(gameObject);
    }
}



