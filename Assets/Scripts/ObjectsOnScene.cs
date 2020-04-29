using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsOnScene : MonoBehaviour
{
    
    private Dictionary<float, string> _objectsCoordinates = new Dictionary<float, string>(); // Словарь, содержащий координату объекта и его тип

    // Функция получения координат всех игроков:
    public Dictionary<float, string> GetPlayersPosition(Vector3 myCoodinates) {
        GameObject[] objects = GetObjectsByLayer(8); // 8 - номер слоя InteractionLayers

        for (int i = 0; i < objects.Length; i++)
        {   
            if (myCoodinates == objects[i].transform.position)  
            {
                // Если координаты объекта из списка совпадают с координатами персонажа, запрашивающего объекты поблизости,
                // то эти данные не записываются, т.к. свои данные он не запрашивает.
            }
            else if (objects[i].tag == "Player") 
            {
                _objectsCoordinates.Add(DistanceBetweenObjects(myCoodinates, objects[i].transform.position), "Character");
            }
            else if (objects[i].tag == "other-object") 
            {
                _objectsCoordinates.Add(DistanceBetweenObjects(myCoodinates, objects[i].transform.position), "other-object");
            }
        }

        return _objectsCoordinates;
    }

    // Функция получения всех объектов, находящихся в слое InteractionLayers
    // InteractionLayers - слой, в котором будут находится все объекты, необходимые нейронке
    private GameObject[] GetObjectsByLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();

        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
            }
        }

        if (goList.Count == 0)
        {
            return null;
        }

        return goList.ToArray();
    }

    private float DistanceBetweenObjects(Vector3 objectCoodinates1, Vector3 objectCoodinates2) {
        return Vector3.Distance(objectCoodinates1, objectCoodinates2);
    } 
}
