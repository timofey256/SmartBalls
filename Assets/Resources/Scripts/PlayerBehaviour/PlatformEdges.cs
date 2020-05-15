using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Math;

public class PlatformEdges : MonoBehaviour
{
    private float platformRadius = 1000f;   // Радиус платформы 

    // Возвращает лист дистанций до краев платформы(номер элемента соответствует углу):
    public List<float> GetDistanceToEdge(Vector3 playerPosition)
    {
        List<Vector3> dotsToCheck = this.GetDotsOnPlatform();
        SortedDictionary<int, float> distancesToEdge = GetAnglesAndDistances(dotsToCheck, playerPosition);
        List<float> distances = this.InsertEmpty(distancesToEdge);

        return distances;
    }

    // Возвращает все уникальные углы, которые можно посчитать:
    private SortedDictionary<int, float> GetAnglesAndDistances(List<Vector3> dots, Vector3 defaultPos)
    {
        SortedDictionary<int, float> dict = new SortedDictionary<int, float>();

        for (int n = 0; n < dots.Count; n += 1)
        {
            Vector2 playerToNormal = new Vector2(500 - defaultPos.x, 0 - defaultPos.z);
            Vector2 playerToDot = new Vector2(dots[n].x - defaultPos.x, dots[n].z - defaultPos.z);

            int angle = AngleOfVectors(playerToNormal, playerToDot);

            float distance = new float();
            distance = Vector3.Distance(defaultPos, dots[n]);            

            dict = TryToAdd(angle, distance, dict);
        }

        return dict;
    }

    // Добавление [угол, дистанция](не добавляет, если такой угол уже есть):
    private SortedDictionary<int, float> TryToAdd(int angle, float distance, SortedDictionary<int, float> dict)
    {
        try
        {
            dict.Add(angle, distance);
        }
        catch
        {
            // Если такой угол уже существует
        }  

        return dict;
    }

    // Добавляет и считает пропущенные углы:
    private List<float> InsertEmpty(SortedDictionary<int, float> distancesToEdge)
    {
        distancesToEdge = AddIfMissing(distancesToEdge);

        List<float> distances = new List<float>();
        distances = distancesToEdge.Values.ToList();
        distances = SetAverageValuesIfEmpty(distances);
           
        return distances;
    }

    // Вставляет пропущенные углы, если таковых нет:
    private SortedDictionary<int, float> AddIfMissing(SortedDictionary<int, float> dict) 
    {
        for (int n = 0; n < dict.Count; n++)
        {
            if (!dict.ContainsKey(n))
            {
                dict.Add(n, -1f);
            }
        }   

        return dict;
    }

    // Считает пропущенный угол и вставляет его:
    private List<float> SetAverageValuesIfEmpty(List<float> values)
    {
        for (int n = 0; n < values.Count; n++)
        {

            if (n == 0 && values[n] == -1) {
                values[n] = values[n+1];
            }    
            else if (n == values.Count - 1 && values[n] == -1) {
                values[values.Count - 1] = values[values.Count - 2];
            }
            else if (values[n] == -1) {
                int index = 1;

                for (int i = 1; values[n + i] == -1; i++)
                {
                    
                    if (values[n+i] == -1) {
                        index++;
                    }
                }
                
                float average = (values[n - 1] + values[n + index]) / (index + 1);

                for (int j = 0; j < index; j++)
                {
                    values[n + j] = average; 
                }
                
            }
        }

        return values;
    }

    // Возвращает точки на краю платформы:
    private List<Vector3> GetDotsOnPlatform() 
    {
        List<Vector3> checkDots = new List<Vector3>();

        for (int i = 0; i < 360; i += 1)
        {
            Vector3 dotOnPlatform = new Vector3();

            dotOnPlatform.x = platformRadius * (float)Cos(i);
            dotOnPlatform.z = platformRadius * (float)Sin(i);
            
            checkDots.Add(dotOnPlatform);
        }

        checkDots = SortVectors(checkDots);

        return checkDots;
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

    // Сортирует точки:
    private List<Vector3> SortVectors(List<Vector3> dots)
    {
        List<Vector3> b = dots.OrderBy(item => item.x).ThenBy(item => item.z).ToList();
        return b;
    }
}
