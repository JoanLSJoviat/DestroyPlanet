using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private int lives = 3;
    private int score = 0;
    public bool gameOver = false;
    public bool gameWon = false;
    public SpawnManager sm;
    public TMP_Text lbl_Score, lbl_Lives, lbl_Wave, lbl_BestScore, lbl_MaxWave, info;
    public Image gameTitle;
    public GameObject menu, inGamePanel, earth, clouds, infoPanel;
    public float waveMsgTimer = 0;
    public bool waveMsg = true;
    public GameObject backBtn;
    public AudioClip win;
    public AudioClip lose;
    private AudioSource audioSource;
    public int bestScore;
    private int maxWave = 10;

    /*
     * Al començar es carreguen les dades persistents de la score i les fases superades.
     * Es preparen els elements de la UI del menú
     */
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("bestScore");
        audioSource = GetComponent<AudioSource>();
        earth.SetActive(false);
        clouds.SetActive(false);
        menu.SetActive(true);
        backBtn.SetActive(false);
        inGamePanel.SetActive(false);
        infoPanel.SetActive(false);
        gameTitle.enabled = true;
        gameOver = true;
        lbl_BestScore.text = "Best score: " + bestScore.ToString();
        lbl_MaxWave.text = "Max wave: " + PlayerPrefs.GetInt("maxWave", 0).ToString();

    }
    
    void Update()
    {
        CheckGameFinished();  //Es va comprovant si el joc ha de finalitzar o no
        
        //Quan s'iniciï una nova fase, sortirà un missatge informatiu
        if (waveMsg == true)
        {
            EnableWaveMsg(true);
            waveMsgTimer = 0;
           // Debug.Log("wavenmsg timer: " + waveMsgTimer);
            waveMsg = false;

        }
        
        waveMsgTimer += Time.deltaTime;
        
        //El missatge serà visible durant 2s, passat aquest temps es desactivar'a l'element corresponent de la UI
        if (waveMsgTimer > 2 && !gameOver)
        {
            EnableWaveMsg(false);
        }
       
    }

    /**
     * AL iniciar el joc es desactiven els elements de la UI del menú i s'activen els elements de la interfíficie in game.
     */
    public void StartGame()
    {
        menu.SetActive(false);
        gameTitle.enabled = false;
        lbl_BestScore.enabled = false;
        lbl_MaxWave.enabled = false;
        lbl_Lives.text = "Lives: " + lives;
        lbl_Score.text = "Score: " + score;
        earth.SetActive(true);
        clouds.SetActive(true);
        inGamePanel.SetActive(true);
        gameOver = false;
        DisplayWave(1);
        waveMsg = true;

    }
    
    /**
     * Funció per afegir punts 
     */
    public void AddScore()
    {
        score++;
        lbl_Score.text = "Score: " + score;
    }
    
    /**
     * Funció per a restar vides
     */
    public void TakeDamage()
    {
        lives--;
        lbl_Lives.text = "Lives: " + lives;
    }

    /**
     * Funció per a establir el text informatiu al panell de la UI on van les vides i els punts
     */
    public void DisplayWave(int wave)
    {
        if (wave <= maxWave)
        {
            lbl_Wave.text = "Wave: " + wave;
        }
    }
    
    /**
     * Funció per a activar o desactivar els elements de la UI del missatge que surt en cada nova fase
     * @param
     * bool active - si s'ha d'activar la UI valdrà true, en cas contrari false
     */

    public void EnableWaveMsg(bool active)
    {
        if (active)
        {
            infoPanel.SetActive(true);
            info.text = "Wave " + sm.waveCounter;
        }
        else
        {
            infoPanel.SetActive(false);
        }
        
    }
    
    /**
     * Funció per a comprovar els requisits de fi de partida
     */
    
    public void CheckGameFinished()
    {
        if (lives == 0 && score <= bestScore)  //si el jugador s'ha quedat sense vides i no ha superat la best score
        {
            SetupEndgame(0);
        }

        if (lives == 0 && score > bestScore)  //si el jugador s'ha quedat sense vides i ha superat la best score
        {
            SetupEndgame(1);
            
        }

        if (sm.respawnTime <= sm.minRespawnTime)  //si el temps de respawn dels coets és més petit o igual que el mínim establert
        {                                           //i per tant el jugador ha arribat al final establert 
            SetupEndgame(2);
        }
        
    }
    
    /**
     * Funció per a preparar la finalitzacio del joc.
     * @param int type
     * 0 -> el jugador ha perdut (s'ha quedat sense vides i no ha superat best score)
     * 1 -> el jugador ha superat la best score tot i que s'ha quedat sense vides
     * 2 -> el jugador ha guanyat (ha superat totes les fases sense quedar-se sense vides)
     */
    public void SetupEndgame(int type)
    {
        gameOver = true;
        Destroy(GameObject.FindGameObjectWithTag("Bombs"));  //en finalitzar el joc, tots els coets es destrueixen
        infoPanel.SetActive(true);
        backBtn.SetActive(true);
        
        //segons el paràmetre d'entrada es mostren els missatges corresponents i es reprodueixen els sons
        
        if (type == 0)
        {
           // audioSource.PlayOneShot(lose);
            Debug.Log(audioSource);
            info.text = "GAME OVER";
            gameWon = false;
        }
        else if (type == 1)             //nota: els sons estan comentats perque no es reprodueixen bé i no he sabut trobar per què
        {
          // audioSource.PlayOneShot(win);
            info.text = "VICTORY! NEW BEST SCORE";
            gameWon = true;
        }
        else if (type == 2)
        {
           // audioSource.PlayOneShot(win);
            info.text = "THE END. YOU WON";
            gameWon = true;
        }
        
        SetPlayerPrefs();  //Es guarden els nous resultats obtinguts
    }
    
    /**
     * Funcio per a guardar els resultats obtinguts
     */
    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("bestScore", score);
        PlayerPrefs.SetInt("maxWave", sm.waveCounter);
    }

    /**
     * Per a sortir del joc
     */
    public void ExitGame()
    {
        Application.Quit();
    }

    /**
     * Per reiniciar el joc
     */
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    
}
