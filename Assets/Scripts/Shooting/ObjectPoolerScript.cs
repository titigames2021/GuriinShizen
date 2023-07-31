using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ObjectPoolerScript : MonoBehaviour
{
    //Esta es una clase generica para crear pools de objetos 

    public static ObjectPoolerScript current;

    // Intente crear una array para el tipo de balas pero no se puede 
    //public List<GameObject> pooledObject;
    public GameObject pooledObject;
    public int poolMaxSize;
   public  List<GameObject> pooledObjectsList;
    public bool willGrow;
    public int bulletType;
    
    void Awake()
    {
        current = this;
    }



    //cuando inicia el juego se crea la pool con el tamaño  y los objetos que hemos puesto en el inspector 
    void Start()
    {
        pooledObjectsList = new List<GameObject>();
        for (int i = 0; i < poolMaxSize; ++i)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject, gameObject.transform);
           
            obj.SetActive(false);
            pooledObjectsList.Add(obj);
        }
    }
    //Esta funcion retorna los objetos inactivos a una clase que haga de disparador que los active y coloque 
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjectsList.Count; ++i)
        {
            if (!pooledObjectsList[i].activeInHierarchy)
            {
                
                //Return pooledObjectsList[i];
                GameObject obj = pooledObjectsList[i];
                obj.SetActive(true);
                // Calls the DeactivateObjects function after 5 seconds.
                
                return obj;

            }
        }
        //Si activamos este bool cuando nos quedemos sin objetos para instanciar creara nuevos 
        //Para el jugador no lo necesitaremos porque queremos limitar el numero de balas a disparar pero lo dejamos para un posible enemigo que lo utilice 
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjectsList.Add(obj);
            return obj;





        }

        return null;
    }
   
}
