using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerController;
    private ObjectsOnScene objectsOnScene;  // Скрипт получения координат объектов на сцене
    public GameObject GroupPlayers; // Объект родитель всех особей (Create Player)

    private List<Vector3> _playerPositions = new List<Vector3>(); // test!

    private float _playerSpeed = 500f; // Скорость персонажа
    private float _jumpPower = 800f;  // Высота прыжка
    private float _gravitationForce = 400f;

    void Start() {
        objectsOnScene = GroupPlayers.GetComponent<ObjectsOnScene>();
        playerController = GetComponent<CharacterController>();

        //Cursor.lockState = CursorLockMode.Locked;   // Блокировка курсора
    }

    void Update()
    {
        this.PlayerGravitation();
    }

    // Ходьба персонажа:
    public void PlayerWalking(Vector3 movementVector, float movementCoefficient) 
    {
        float playerDirectionX = movementVector.x;
        float playerDirectionZ = movementVector.z;
        
        gameObject.transform.Translate(
            Vector3.forward * _playerSpeed * playerDirectionX * Time.deltaTime * movementCoefficient + 
            Vector3.right * _playerSpeed * playerDirectionZ * movementCoefficient * Time.deltaTime);       
    }

    // Гравитация персонажа:
    private void PlayerGravitation() 
    {   
        
        // Если персонаж в воздухе, то падает:
        if (!playerController.isGrounded)
        {
            _gravitationForce -= 2000f * Time.deltaTime;
        }

        PlayerFalling();
    }

    // Падение персонажа:
    private void PlayerFalling() {
        Vector3 _playerPosition = Vector3.zero;
        _playerPosition.y = _gravitationForce;
        playerController.Move(_playerPosition * Time.deltaTime);
    }

    // Прыжок персонажа:
    public void PlayerJumping() 
    {
        if (playerController.isGrounded)
        {
            _gravitationForce = _jumpPower;
        }
    }

    // Удаление персонажа со сцены:
    private void PlayerDestroy()
    {
        Destroy(gameObject);
    }

    // Функция, возвращающая объекты поблизости
    private Dictionary<float, string> CheckAround() 
    {
        return objectsOnScene.GetObjectsPosition(gameObject.transform.position);
    }
}
