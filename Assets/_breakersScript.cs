using Wawa.Modules;
using Wawa.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class _breakersScript:ModdedModule{

    public Material[]colors;
    private bool[,]finalPositions=new bool[3,4];
    private bool[]currentBlackPositions=new bool[4]{false,false,false,false};
    private bool[]currentColorfulPositions=new bool[3]{false,false,false};
    public KMSelectable[]blackBreakers;
    public KMSelectable[]colorfulBreakers;
    public MeshRenderer[]colorfulHandles;
    public KMBombInfo bomb;
    private string serial;
    void Start(){
        foreach(KMSelectable blackB in blackBreakers){
            blackB.Set(onInteract:()=>{
                StartCoroutine(flipBreaker(blackB,Array.IndexOf(blackBreakers,blackB),false));
            });
        }
        foreach(KMSelectable colorfulB in colorfulBreakers){
            colorfulB.Set(onInteract:()=>{
                StartCoroutine(flipBreaker(colorfulB,Array.IndexOf(colorfulBreakers,colorfulB),true));
            });
        }
        serial=bomb.GetSerialNumber();
        colorBreakersIn();
	}

    private IEnumerator flipBreaker(KMSelectable breaker,int index,bool isColorful){
        Shake(breaker,.5f,Sound.BigButtonPress);
        if((!isColorful&&!currentBlackPositions[index])||(isColorful&&!currentColorfulPositions[index])){
            for(int i=0;i<10;i++){
                breaker.GetComponent<Transform>().Rotate(0f,0f,-9f);
                yield return null;
            }
        }else if((!isColorful&&currentBlackPositions[index])||(isColorful&&currentColorfulPositions[index])){
            for(int i=0;i<10;i++){
                breaker.GetComponent<Transform>().Rotate(0f,0f,9f);
                yield return null;
            }
        }
        if(isColorful)
            currentColorfulPositions[index]=!currentColorfulPositions[index];
        else
            currentBlackPositions[index]=!currentBlackPositions[index];
    }

    private void colorBreakersIn(){
        string[] colorNames=new string[]{"red","blue","green","yellow"};
        int color1=UnityEngine.Random.Range(0,4);
        int color2;
        do{
            color2=UnityEngine.Random.Range(0,4);
        }while(color2==color1);
        int color3;
        do{
            color3=UnityEngine.Random.Range(0,4);
        }while(color3==color1||color3==color2);
        colorfulHandles[0].material=colors[color1];
        colorfulHandles[1].material=colors[color2];
        colorfulHandles[2].material=colors[color3];
        Log("The colorful breakers are "+colorNames[color1]+", "+colorNames[color2]+", and "+colorNames[color3]+" from top to bottom.");
    }
}