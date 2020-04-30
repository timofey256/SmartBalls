using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsOnScene : MonoBehaviour
{
    
    private float visibleRadius = 1f;   // Величина радиуса персонажа, в котором он может видеть предметы
    private Dictionary<float, string> _objectsCoordinates = new Dictionary<float, string>(); // Словарь, содержащий координату объекта и его тип

    // Функция получения координат всех игроков:
    public Dictionary<float, string> GetPlayersPosition(Vector3 myCoodinates, float visibleRadiusCoefficient = 1f) 
    {
        GameObject[] objects = GetObjectsByLayer(8); // 8 - номер слоя InteractionLayers

        float visibleDistance = visibleRadius * visibleRadiusCoefficient; 

        for (int i = 0; i < objects.Length; i++)
        {   
            float distanceBetweenObjects = DistanceBetweenObjects(myCoodinates, objects[i].transform.position);
            if (myCoodinates == objects[i].transform.position || distanceBetweenObjects > visibleDistance)
            {
                // Первое условие - отстутвие возможности возвращения в списке своих же координат;
                // Второе условие - реализация области видимости у персонажа; 
            }
            else if (objects[i].tag == "Player") 
            {
                _objectsCoordinates.Add(distanceBetweenObjects, "Character");
            }
            else if (objects[i].tag == "other-object") 
            {
                _objectsCoordinates.Add(distanceBetweenObjects, "other-object");
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
