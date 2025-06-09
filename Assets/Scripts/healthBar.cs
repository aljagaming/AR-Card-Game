using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class healthBar : MonoBehaviour
{

    private GameObject firstRedTile;
    private GameObject firstBlueTile;
    private TMP_Text redHealthPoints;
    private TMP_Text blueHealthPoints;


    private CreateImageTarget scriptInstance;

    private GameObject[] redTiles=new GameObject[10];
    private GameObject[] blueTiles=new GameObject[10];


    private int lastBlueHealth;

    private int lastRedHealth;



    float[,] colors = new float[10, 3] {
    {255f,0f,0f},  
    {255f,50f,0f},  
    {255f,100f,0f},  
    {255f,150f,0f},  
    {255f,200f,0f},  
    {255f,230f,0f},  
    {200f,250f,50f}, 
    {150f,255f,100f},  
    {100f,255f,150f},
    {0f,255f,150f}
    };



    // Start is called before the first frame update
    void Start()
    {

        redTiles[0]=GameObject.Find("HealthBarRed");
        blueTiles[0]=GameObject.Find("HealthBarBlue");

        redHealthPoints=GameObject.Find("redHp").GetComponent<TMP_Text>();
        blueHealthPoints=GameObject.Find("blueHp").GetComponent<TMP_Text>();

    

        scriptInstance=GameObject.Find("GameObject").GetComponent<CreateImageTarget>();

        lastRedHealth=scriptInstance.redHealth;
        lastBlueHealth=scriptInstance.blueHealth;




        CreateHealthBars(lastRedHealth,lastBlueHealth);


        
    }

    // Update is called once per frame
    void Update()
    {
        manageHealthBar(scriptInstance.redHealth, scriptInstance.blueHealth);
    }


    void CreateHealthBars(int redHp, int blueHp){

        RectTransform tileToPlace;
        RectTransform firstTile;

        redHealthPoints.text=""+redHp;
        blueHealthPoints.text=""+blueHp;

        

        redTiles[0].GetComponent<Image>().color=new Color(colors[0,0]/255f,colors[0,1]/255f,colors[0,2]/255f);
        blueTiles[0].GetComponent<Image>().color=new Color(colors[0,0]/255f,colors[0,1]/255f,colors[0,2]/255f);

        for(int i=1;i<((int)(blueHp/400));i++){
            //redTiles[i]=Instantiate(redTiles[0], redTiles[0].transform.position+Vector3.right*30f*i, redTiles[0].transform.rotation);

            redTiles[i] = Instantiate(redTiles[0], redTiles[0].transform.parent);
            tileToPlace = redTiles[i].GetComponent<RectTransform>();
            firstTile = redTiles[0].GetComponent<RectTransform>();
            tileToPlace.anchoredPosition = firstTile.anchoredPosition + new Vector2(0, 90f * i);
            tileToPlace.GetComponent<Image>().color=new Color(colors[i,0]/255f,colors[i,1]/255f,colors[i,2]/255f);

        }

        for(int i=1;i<((int)(redHp/400));i++){
            blueTiles[i] = Instantiate(blueTiles[0], blueTiles[0].transform.parent);
            tileToPlace = blueTiles[i].GetComponent<RectTransform>();
            firstTile = blueTiles[0].GetComponent<RectTransform>();
            tileToPlace.anchoredPosition = firstTile.anchoredPosition + new Vector2(0, 90f * i);
            tileToPlace.GetComponent<Image>().color=new Color(colors[i,0]/255f,colors[i,1]/255f,colors[i,2]/255f);
        }

    }



    void manageHealthBar(int redHp, int blueHp){
        if(redHp<=0){
            for(int i=0; i<redTiles.Length; i++){
                if(redTiles[i]!=null){
                    Destroy(redTiles[i]);
                }
            }


            redHealthPoints.text=""+0;
            enabled=false;
            return;
        }

        if(blueHp<=0){
            for(int i=0; i<blueTiles.Length; i++){
                if(blueTiles[i]!=null){
                    Destroy(blueTiles[i]);
                }
            }
            blueHealthPoints.text=""+0;
            enabled=false;
            return;
        }


        if(redHp==lastRedHealth && blueHp==lastBlueHealth){
            return;
        }else if(redHp!=lastRedHealth && blueHp==lastBlueHealth){ //at least one of them probably didnt change bcs its a turn based game

            for(int i=9; i>=(redHp/400);i--){//note that this rounds down so like 3100/400=7.75, but it will show it only as 7
                Destroy(redTiles[i], 0.5f);
            }

            redHealthPoints.text=""+redHp;
            lastRedHealth=redHp;
        
        }else{
            for(int i=9; i>=(blueHp/400);i--){//note that this rounds down so like 3100/400=7.75, but it will show it only as 7
                Destroy(blueTiles[i], 0.5f);
            }

            blueHealthPoints.text=""+blueHp;
            lastBlueHealth=blueHp;
            
        }

    }


    public void deleteAll(){

        for(int i=0; i<redTiles.Length; i++){
            if(redTiles[i]!=null){
                Destroy(redTiles[i]);
            }
        }


        for(int i=0; i<blueTiles.Length; i++){
            if(blueTiles[i]!=null){
                Destroy(blueTiles[i]);
            }
        }

        Destroy(blueHealthPoints);
        Destroy(redHealthPoints);


    }
}
