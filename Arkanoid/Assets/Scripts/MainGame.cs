﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Events;

public class MainGame : MonoBehaviour
{
    public static MainGame instance = null;

    public delegate void ResetAction();
    public delegate void ResetGameAction();
    public delegate void StopAction();
    public delegate void ResumeAction();
    public delegate void StartAction();


    public event ResetAction ResetBase;
    public event ResetGameAction ResetGame;
    public event StopAction StopGame;
    public event ResumeAction ResumeGame;
    public event StartAction StartGame;

    

    private int score;
    private int maxHP;
    private int hp;
    private int bricks;
    private int hits;
    private int highScore;


    private bool inGame;
    private bool inPause;

    void Awake()
    {
         if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy (gameObject);

        ResetGame += ResetMain;
        
        highScore = PlayerPrefs.GetInt("highScore");
    }

    void Start(){
        ResetMain();
        Canvas_script.instance.Lifes(hp);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && ResetGame != null){ // !inGame && -Solo se puede reiniciar cuando el juego termina
            ResetMain();
            ResetGame();
            Canvas_script.instance.SetGameText("");
            Canvas_script.instance.Score(score);
            Canvas_script.instance.Lifes(hp);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && inGame && ResumeGame != null && StopGame != null){
            if(inPause){
                inPause = false;
                ResumeGame();
                Canvas_script.instance.SetGameText("");
            }else{
                inPause = true;
                StopGame();
                Canvas_script.instance.SetGameText("PAUSE");
            }
        }
    }

    private void ResetMain(){
        inGame = true;
        inPause = false;
        score = 0;
        maxHP = 12500;
        hp = maxHP;
        bricks = 99999; // Cambiar
        hits = 0;
    }




 
    private void CheckGameOver(){
        if(hp <= 0){
            Debug.Log("GAME OVER");
            StopGame();
            Canvas_script.instance.SetGameText("Game Over");
            inGame = false;
            if(score > highScore)
                PlayerPrefs.SetInt("highScore",score);

            Invoke("ChangeScene",5f);
        }
        else if(hp > 0 && bricks <= 0){
            Debug.Log("WIN GAME");
            StopGame();
            Canvas_script.instance.SetGameText("Congratulations!");
            inGame = false;
        }
    }

     private void ChangeScene(){
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
     }


    public void LooseLife(){
        hp-= 2500;
        hits = 0;
        Canvas_script.instance.Lifes(hp);
        CheckGameOver();
        if(ResetBase != null && inGame)
            Invoke("ResetBaseDalay" ,2f);
    }

    private void ResetBaseDalay(){
        ResetBase();
    }

    public void PiezaRota(){
        bricks--;
        ++hits;
        AddScore();
        LoseHP(150);
        Canvas_script.instance.Score(score);
        Canvas_script.instance.Lifes(hp);
        CheckGameOver();
    }

    public void ShareMaxVelocity(float value){
       Canvas_script.instance.SetSliderMaxVelocity(value);
    }

    public void ShareVelocity(float value){
        Canvas_script.instance.SetSliderVelocity(value);
    }

    public void AddHP(int value){
        hp += value;
        if(hp > maxHP) hp = maxHP;
        Canvas_script.instance.Lifes(hp);
    }

    private void AddScore(){
        if(hits > 15)
            score+= 17;
        else if(hits > 10)
            score+= 11;
        else if(hits > 5)
            score+= 5;
        else
            score+= 3;
    }

    public void LoseHP(int value){
        if(hp > value) 
            hp -= value;
    }

    public void SetStartGame(){

        if(StartGame != null)
            StartGame();
    }

}
