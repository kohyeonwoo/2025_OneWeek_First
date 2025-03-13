using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> spawnList = new List<GameObject>(); //스폰 유닛 모음
    public List<GameObject> poolObject = new List<GameObject>(); // 풀 오브젝트 부분 
    public List<Transform> locationList = new List<Transform>(); // 스폰 위치 모음 

    public int limit; //풀에서 생성될 제한 수 

    private void Start()
    {
        CreatePool();

        for (int i = 0; i < limit; i++)
        {
            SpawnObject();
        }
        
    }

    public void SpawnObject()
    {
        GameObject objects = GetPoolObject();

        int rand = Random.Range(0, locationList.Count);

        if(objects != null)
        {
            objects.transform.position = locationList[rand].position;
            objects.SetActive(true);
        }

    }


    private void CreatePool()
    {

        int rand = Random.Range(0, spawnList.Count);

        for(int i =0; i < limit; i++)
        {
            GameObject obj = Instantiate(spawnList[rand]);
            obj.SetActive(false);
            poolObject.Add(obj);
        }
    }

    public GameObject GetPoolObject()
    {

        for(int i =0; i < poolObject.Count; i++)
        {
            if(!poolObject[i].activeInHierarchy)
            {
                return poolObject[i];
            }
        }

        return null;
    }


}
