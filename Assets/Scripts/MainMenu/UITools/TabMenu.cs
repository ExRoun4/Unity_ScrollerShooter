using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabMenu : MonoBehaviour
{
    public GameObject[] tabs;
    public bool showFirstTabOnStart = true;

    private Dictionary<string, GameObject> tabsDictionary = new ();


    private void Start()
    {
        foreach(GameObject tab in tabs){
            tabsDictionary[tab.name] = tab;
            tab.SetActive(false);
        }

        if(showFirstTabOnStart) ShowTab(0);
    }


    public void ShowTab(string tabName){
        foreach(KeyValuePair<string, GameObject> tabHolder in tabsDictionary){
            if(tabHolder.Key == tabName){
                tabHolder.Value.SetActive(true);
                continue;
            }
            tabHolder.Value.SetActive(false);
        }
    }

    public void ShowTab(int index){
        if(index + 1 > tabs.Length) return;

        ShowTab(tabs[index].name);
    }
}
