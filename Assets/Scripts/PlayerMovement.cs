using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerController;
    private ObjectsOnScene objectsOnScene;  // Скрипт получения координат объектов на сцене
    private PlatformEdges platformEdges;

    private List<Vector3> _playerPositions = new List<Vector3>(); // test!

    private float _playerSpeed = 5f; // Скорость персонажа
    private float _jumpPower = 8f;  // Высота прыжка
    private float _gravitationForce = 4f;

    void Start()
    {
        platformEdges = GetComponent<PlatformEdges>();    
        objectsOnScene = GetComponent<ObjectsOnScene>();
        playerController = GetComponent<CharacterController>();

        List<float?> distances = platformEdges.GetDistanceToEdge(gameObject.transform.position);

        objectsOnScene.GetObjectsPosition(gameObject.transform.position);

        Cursor.lockState = CursorLockMode.Locked;   // Блокировка курсора
    }

    void Update()
    {
        this.PlayerGravitation();
    }

    // Ходьба персонажа:
    private void PlayerWalking(Vector3 movementVector, float movementCoefficient) 
    {
        float playerDirectionX = movementVector.x;
        float playerDirectionZ = movementVector.z;

        transform.Translate(Vector3.forward * _playerSpeed * playerDirectionZ * Time.deltaTime * movementCoefficient + 
                            Vector3.right * _playerSpeed * playerDirectionZ * movementCoefficient * Time.deltaTime);       
    }

    // Гравитация персонажа:
    private void PlayerGravitation() 
    {   
        
        // Если персонаж в воздухе, то падает:
        if (!playerController.isGrounded)
        {
            _gravitationForce -= 20f * Time.deltaTime;
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
    private void PlayerJumping() 
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
