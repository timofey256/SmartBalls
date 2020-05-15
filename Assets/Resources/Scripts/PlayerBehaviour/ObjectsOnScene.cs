using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class ObjectsOnScene : MonoBehaviour
{
    public float visibleRadius = 5000f;   // Радиуса видимости объектов

    private List<Dictionary<float, string>> radarInfo;

    // Возвращает координаты и тип каждого объекта
    public List<Dictionary<float, string>> GetObjectsPosition(Vector3 myCoodinates, float visibleRadiusCoef = 1f) 
    {
        radarInfo = FillingList(360);
        List<Dictionary<float, string>> _objectsCoordinates = new List<Dictionary<float, string>>(); // Для хранения координаты объекта и его типа
        float visibleDistance = visibleRadius * visibleRadiusCoef;

        // Объекты, которые есть на сцене
        GameObject[] _objectsOnScene = this.GetObjectsByLayer(8); // 8 - номер слоя InteractionLayers          

        _objectsCoordinates = this.WriteObjectsCoordinates(_objectsOnScene, myCoodinates, visibleDistance);

        return _objectsCoordinates;
    }

    // Получает координаты и тип каждого объекта:
    private List<Dictionary<float, string>> WriteObjectsCoordinates(GameObject[] objects, Vector3 playerCoodinates, float visibleDistance)
    {
        List<Dictionary<float, string>> objectsCoordinates = new List<Dictionary<float, string>>();

        for (int i = 0; i < objects.Length; i++)
        {   
            float distanceBetweenObjects = DistanceBetweenObjects(playerCoodinates, objects[i].transform.position);
            bool isVisibleDistance = distanceBetweenObjects < visibleDistance;

            Vector2 playerToNormal = new Vector2(500 - playerCoodinates.x, 0 - playerCoodinates.z);
            Vector2 playerToDot = new Vector2(objects[i].transform.position.x - playerCoodinates.x, 
                                                objects[i].transform.position.z - playerCoodinates.z);

            int angle = AngleOfVectors(playerToNormal, playerToDot);
            
            if (playerCoodinates != objects[i].transform.position)
            {
                radarInfo = AddItem(radarInfo, objects[i], isVisibleDistance, angle, distanceBetweenObjects);  
            }
        }

        return radarInfo;
    }

    private List<Dictionary<float, string>> AddItem(List<Dictionary<float, string>> existList, GameObject otherObject, bool isVisible, int angle, float distance)
    {
        if (otherObject.tag == "Player") {
            if (angle == 360) angle = 0;

            if(isVisible)   // Условие - реализация области видимости у персонажа; 
            {
                Dictionary<float, string> newItem = new Dictionary<float, string>();
                newItem.Add(distance, "Character");
                
                existList[angle] = newItem;
            } 
            else 
            {
                Dictionary<float, string> newItem = new Dictionary<float, string>();
                newItem.Add(-1.0f, "None");
                existList[angle] = newItem;
            }
        }

        return existList;
    }

    // Функция получения всех объектов, находящихся в слое InteractionLayers
    // InteractionLayers - слой, в котором будут находится все объекты, необходимые ген. алгоритму
    public GameObject[] GetObjectsByLayer(int layer)
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

    // Заполняет список пустыми значениями("-1" - показатель отсутствия игрока по-близости):
    private List<Dictionary<float, string>> FillingList(int length)
    {
        List<Dictionary<float, string>> list = new List<Dictionary<float, string>>();
        Dictionary<float, string> emptyElement = new Dictionary<float, string>();
        emptyElement.Add(-1f, "None");
        for (float n = 0; n < length; n++)
        {
            list.Add(emptyElement);
        }

        return list;
    }

    private static double AngleOfReference(Vector2 v)
    {
        return NormalizeAngle(Atan2(v.y, v.x) / PI * 180);
    }

    // Возвращает угол между векторами:
    private static int AngleOfVectors(Vector2 first, Vector2 second)
    {
        double angle = NormalizeAngle(AngleOfReference(first) - AngleOfReference(second));
        return (int)Round(angle);
    }

    private static double NormalizeAngle(double angle)
    {
        bool CheckBottom(double a) => a >= 0;
        bool CheckTop(double a) => a < 360;

        double turn = CheckBottom(angle) ? -360 : 360;
        while (!(CheckBottom(angle) && CheckTop(angle))) angle += turn;
        return angle;
    }
}
