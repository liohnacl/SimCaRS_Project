using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTraffic : MonoBehaviour
{
    public GameObject[] road;
    public int zPos = 80;
    public bool createRoad = false;
    public int roadNum;

    // Update is called once per frame
    void Update()
    {
        if (createRoad == false){
            createRoad = true;
            StartCoroutine(GenerateRoad()); //Coroutine - adding delay between creating road; interval
        }
    }

    IEnumerator GenerateRoad(){
        roadNum = Random.Range(0, 6); //will pick randomly from any of the 7 roads 
        Instantiate(road[roadNum], new Vector3(0,0,zPos), Quaternion.identity);
        zPos += 80;
        yield return new WaitForSeconds(2);
        createRoad = false;
    }
}
