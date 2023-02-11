using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trafficlightbox_Color : MonoBehaviour
{
      [SerializeField] private GameObject trafficlight_boxRed;
      [SerializeField] private GameObject trafficlight_boxYellow;
      [SerializeField] private GameObject trafficlight_boxGreen;

      // public void hide(){
      //    trafficlight_box.SetActive(false);
      // }

      // public void show(){
      //    trafficlight_box.SetActive(true);
      // }

    
      void Start()
{
    StartCoroutine(trafficlight_Colorchange());
}

IEnumerator trafficlight_Colorchange()
{
        while(true){
         trafficlight_boxYellow.SetActive(false);
         trafficlight_boxGreen.SetActive(false);
         trafficlight_boxRed.SetActive(true);
         yield return new WaitForSeconds(15);
         trafficlight_boxRed.SetActive(false);
         trafficlight_boxGreen.SetActive(true);
         yield return new WaitForSeconds(10);
         trafficlight_boxGreen.SetActive(false);
         trafficlight_boxYellow.SetActive(true);
         yield return new WaitForSeconds(5);
        }
}
    
  



    
}
