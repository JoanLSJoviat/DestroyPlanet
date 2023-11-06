using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject bombParent;
    private GameManager gm;
    public float respawnTime = 3.0f;
    public float minRespawnTime = 0.7f;
    private float timer = 0.0f;
    public GameObject prefabBomb;
    public Transform earthTransform;
    public int waveCounter = 1;
    private int bombCounter = 0;
    public float waveTimer = 0.0f;
    private float waveTimeLimit = 30.0f;
    private GameObject bomb;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        waveCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Si la partida no ha acabat i el respawn time és més petit que el mínim de temps de respawn establert
        //s'instanciarà un coet quan el comptador superi el respawn time
      
        if (!gm.gameOver && respawnTime > minRespawnTime)
        {
            timer += Time.deltaTime;
            if (timer > respawnTime)
            {
                timer = 0.0f;
                CreateNewBomb();
            }
            
            //Un altre comptador per les fases de joc, cada fase té una duració establerta de 30s    

            waveTimer += Time.deltaTime;
        
           if (waveTimer > waveTimeLimit)
            {
                waveCounter += 1;           //quan el comptador superi els 30s, es passarà a la següent fase
                gm.waveMsg = true;
                gm.DisplayWave(waveCounter);
                waveTimer = 0;
                //Debug.Log("Bombs created: " + bombCounter);
                CreateNextWave();
            }
        }
    }
    
/**
 * Funció per a la instanciació dels coets
 */
    private void CreateNewBomb()
    {
        Vector3 randPosition = Random.onUnitSphere * 0.5f;
      
       bomb = GameObject.Instantiate(prefabBomb, randPosition, Quaternion.identity,bombParent.transform);
       Vector3 relativePos = (bomb.transform.position - earthTransform.position).normalized;
       bomb.transform.up = relativePos;
       bombCounter += 1;
        
    }

/**
 * Funció per a la "generació" de la següent fase, que bàsicament només disminueix el temps de respawn
 * dels coets per a que vagin apareixent més ràpid a mesura que es van superant fases
 */
    public void CreateNextWave()
    {
       // Debug.Log("Wave: " + waveCounter);
        //bombCounter = 0;
        respawnTime -= 0.23f;
    }
    
}
