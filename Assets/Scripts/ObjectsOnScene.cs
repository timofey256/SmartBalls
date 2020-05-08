using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class ObjectsOnScene : MonoBehaviour
{
    public float visibleRadius = 10f;   // Радиуса видимости объектов
    private GameObject[] _objectsOnScene;   // Объекты, которые есть на сцене

    // Возвращает координаты и тип каждого объекта:
    public Dictionary<float, string> GetObjectsPosition(Vector3 myCoodinates, float visibleRadiusCoef = 1f) 
    {
        Dictionary<float, string> _objectsCoordinates = new Dictionary<float, string>(); // Для хранения координаты объекта и его типа
        float visibleDistance = visibleRadius * visibleRadiusCoef;
        
        if (_objectsOnScene == null) {
            _objectsOnScene = this.GetObjectsByLayer(8); // 8 - номер слоя InteractionLayers 
        }         

        _objectsCoordinates = this.WriteObjectsCoordinates(_objectsOnScene, myCoodinates, visibleDistance);

        return _objectsCoordinates;
    }

    // Получает координаты и тип каждого объекта:
    private Dictionary<float, string> WriteObjectsCoordinates(GameObject[] objects, Vector3 playerCoodinates, float visibleDistance)
    {
        Dictionary<float, string> objectsCoordinates = new Dictionary<float, string>();

        for (int i = 0; i < objects.Length; i++)
        {   
            float distanceBetweenObjects = DistanceBetweenObjects(playerCoodinates, objects[i].transform.position);
            bool isVisibleDistance = distanceBetweenObjects > visibleDistance;

            objectsCoordinates = AddItem(playerCoodinates, objects[i], isVisibleDistance);
        }

        return objectsCoordinates;
    }

    private Dictionary<float, string> AddItem(Vector3 playerCoodinates, GameObject otherObject, bool isVisible)
    {
        Dictionary<float, string> objectsInfo = new Dictionary<float, string>();

        float distanceBetweenObjects = DistanceBetweenObjects(playerCoodinates, otherObject.transform.position);

        if (playerCoodinates == otherObject.transform.position)
        {
            // Первое условие - отстутвие возможности возвращения в списке своих же координат;
        }
        else if (otherObject.tag == "Player") {
            if(!isVisible) objectsInfo.Add(distanceBetweenObjects, "Character");
            else objectsInfo.Add(-1.0f, "None"); 
            // условие - реализация области видимости у персонажа; 

        }

        return objectsInfo;
    }

    // Функция получения всех объектов, находящихся в слое InteractionLayers
    // InteractionLayers - слой, в котором будут находится все объекты, необходимые ген. алгоритму
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

    // Возвращает расстояние между объектами:
    private float DistanceBetweenObjects(Vector3 objectCoodinates1, Vector3 objectCoodinates2) 
    {
        return Vector3.Distance(objectCoodinates1, objectCoodinates2);
    }
}
