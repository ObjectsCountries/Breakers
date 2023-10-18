using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wawa.TwitchPlays;
using Wawa.TwitchPlays.Domains;

public class _breakersTP:Twitch<_breakersScript>{
    private const string lrLR="lrLR";
    [Command("")]
    IEnumerable<Instruction>flip(params string[]command){
        foreach(string input in command){
            int colorfulBreakerIndex=-1;
            if(Int32.TryParse(input,out colorfulBreakerIndex)&&colorfulBreakerIndex>0&&colorfulBreakerIndex<3){
                yield return new[]{Module.colorfulBreakers[colorfulBreakerIndex]};
                yield return new WaitForSeconds(1);
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
                        if((Char.ToLower(input[i])=='l'&&Module.currentBlackPositions[i])||(Char.ToLower(input[i])=='r'&&!Module.currentBlackPositions[i]))
                            yield return new[]{Module.blackBreakers[i]};
                            yield return new WaitForSeconds(1);
                    }
                }
            }
            else{
                yield return TwitchString.SendToChatError("{0}, your command has been processed up until the invalid input. Please read the help message to understand input syntax.");
            }
        }
    }

    public override IEnumerable<Instruction>ForceSolve(){
        for(int c=0;c<3;c++){
            for(int b=0;b<4;b++){
                if(!Module.currentColorfulPositions[c]&&Module.currentBlackPositions[b]!=Module.finalPositions[c,b])
                    yield return new[]{Module.blackBreakers[b]};
                    yield return new WaitForSeconds(1);
            }
            if(!Module.currentColorfulPositions[c])
                yield return new[]{Module.colorfulBreakers[c]};
                yield return new WaitForSeconds(1);
        }
        for(int i=0;i<4;i++){
            if(!Module.currentBlackPositions[i])
                yield return new[]{Module.blackBreakers[i]};
                yield return new WaitForSeconds(1);
        }
    }
}