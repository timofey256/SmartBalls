using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayers : MonoBehaviour
{
    public GameObject playerExample;
    public int playersAmount;

    void Start()
    {
        CreatePlayersOnScene(playersAmount);
    }

    // Создает n-ое количество игроков:
    private void CreatePlayersOnScene(int playersAmount) 
    {
        for (int n = 0; n < playersAmount; n++)
        {
            this.CreatePlayer(n + 2);
        }  
    }

    // Создает нового игрока:
    private void CreatePlayer(int playerIndex) 
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        GameObject newPlayer = Instantiate(playerExample, GeneratePosition(), rotation);
        
        this.ChangeObjectName(newPlayer, playerIndex);
        this.SetParentToObject(newPlayer);
        this.SetObjectColor(newPlayer);
    }

    // Изменяет имя игрока:
    private void ChangeObjectName(GameObject obj, int index)
    {
        obj.name = "Player" + index;
    }

    // Перемещает всех игроков в InteractionObjects/Players:
    private void SetParentToObject(GameObject player) 
    {
        player.transform.parent = gameObject.transform;
    }

    // Устанавливает цвет игроку:
    private void SetObjectColor(GameObject player)
    {
        Material randomPlayerMaterial = GetRandomColor(GetPlayerColors());
        player.GetComponent<Renderer>().material = randomPlayerMaterial;
    }

    // Возвращает список всех цветов:
    private List<Material> GetPlayerColors()
    {
        List<Material> materials = new List<Material>();

        materials.Add(Resources.Load("PlayersMaterial_Red", typeof(Material)) as Material);
        materials.Add(Resources.Load("PlayersMaterial_Yellow", typeof(Material)) as Material);
        materials.Add(Resources.Load("PlayersMaterial_Violet", typeof(Material)) as Material);
        materials.Add(Resources.Load("PlayersMaterial_Blue", typeof(Material)) as Material);

        return materials;
    }
    
    // Возвращает случайный цвет:
    private Material GetRandomColor(List<Material> materials)
    {
        int randomIndex = Random.Range(0, materials.Count);
        return materials[randomIndex];
    }

    // Возвращает позицию, на которой будет создаваться игрок:
    private Vector3 GeneratePosition() 
    {
        Vector3 position = new Vector3();

        position.x = Random.Range(-100.0f, 100.0f);
        position.y = 2f;
        position.z = Random.Range(-100.0f, 100.0f);

        return position;
    }
}
