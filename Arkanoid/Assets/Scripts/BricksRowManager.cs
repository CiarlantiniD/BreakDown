﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksRowManager : MonoBehaviour
{
    [SerializeField] private Transform BricksRow;
    [SerializeField] private float timeToMoveBricks;
    [SerializeField] private float moveSpace = 0.03f;
    
    private int numberRow = 10;
    private float highBricks = 0.8f;
    private float inicialPosition = 12f;

    private float time = 0;
    private bool inGame = false;

    Transform [] rows;

    void Awake(){
        
        for (int i = 0; i < numberRow; i++)
        {
            Transform temp;
            temp = Instantiate(BricksRow,transform);
            temp.position = new Vector3(transform.position.x, inicialPosition, transform.position.z);

            inicialPosition+=highBricks;
        }

       rows = GetComponentsInChildren<Transform>();
    }

    void Start(){
        MainGame.instance.StopGame += Stop;
        MainGame.instance.ResetGame += Resume;
        MainGame.instance.StartGame += Resume;
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time > timeToMoveBricks && inGame){
            MoveRows();
            time = 0;
        }
    }

    private void MoveRows(){
        foreach (var item in rows)
        {
            float yPosition = item.position.y - moveSpace;
            item.position = new Vector3(item.position.x, yPosition, item.position.z);
        }
    }

    private void Stop(){inGame = false;}

    private void Resume(){inGame = true;}

}
