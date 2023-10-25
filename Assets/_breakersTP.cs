using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wawa.TwitchPlays;
using Wawa.TwitchPlays.Domains;

public class _breakersTP:Twitch<_breakersScript>{
    private const string lrLR="lrLR";
    private bool[]tempColorful=new bool[]{false,false,false};
    private bool[]tempBlack=new bool[]{false,false,false,false};
    [Command("")]
    IEnumerable<Instruction>flip(params string[]command){
        List<KMSelectable>endResult=new List<KMSelectable>();
        for(int i=0;i<3;i++)
            tempColorful[i]=Module.currentColorfulPositions[i];
        for(int i=0;i<4;i++)
            tempBlack[i]=Module.currentBlackPositions[i];
        foreach(string input in command){
            int colorfulBreakerIndex=-1;
            if(Int32.TryParse(input,out colorfulBreakerIndex)&&colorfulBreakerIndex>0&&colorfulBreakerIndex<4){
                endResult.Add(Module.colorfulBreakers[colorfulBreakerIndex-1]);
                tempColorful[colorfulBreakerIndex-1]=!tempColorful[colorfulBreakerIndex-1];
            }
            else if(input.Length==4){
                bool containsOnlyLR=true;
                foreach(char c in input){
                    if(!lrLR.Contains(c.ToString())){
                        containsOnlyLR=false;
                        break;
                    }
                }
                if(containsOnlyLR){
                    for(int i=0;i<4;i++){
                        if((Char.ToLower(input[i])=='l'&&tempBlack[i])||(Char.ToLower(input[i])=='r'&&!tempBlack[i])){
                            endResult.Add(Module.blackBreakers[i]);
                            tempBlack[i]=!tempBlack[i];
                        }
                    }
                }
            }
            else{
                yield return TwitchString.SendToChatError("{0}, has not been processed due to an invalid input. Please read the help message to understand input syntax.");
            }
        }
        yield return null;
        yield return new Instruction(Sequence(endResult,2f));
    }

    public override IEnumerable<Instruction>ForceSolve(){
        yield return null;
        for(int c=0;c<3;c++){
            for(int b=0;b<4;b++){
                if(!Module.currentColorfulPositions[c]&&Module.currentBlackPositions[b]!=Module.finalPositions[c,b]){
                    Module.blackBreakers[b].OnInteract();
                    yield return new WaitForSeconds(.25f);
                }
            }
            if(!Module.currentColorfulPositions[c]){
                Module.colorfulBreakers[c].OnInteract();
                yield return new WaitForSeconds(.25f);
           }
        }
        for(int i=0;i<4;i++){
            if(!Module.currentBlackPositions[i]){
                Module.blackBreakers[i].OnInteract();
                yield return new WaitForSeconds(.25f);
            }
        }
    }
}
