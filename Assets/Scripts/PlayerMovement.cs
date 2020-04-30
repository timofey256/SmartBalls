 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO:
  - Обнаружение персонажем пропасти поблизости;
  - Переделать движение персонажа с помощью заданого вектора;
  - Заменить Vector3.Distance();
*/

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;

    private CharacterController playerController;
    private ObjectsOnScene playersOnScene;  // Скрипт получения координат других персонажей

    private float _playerSpeed = 5f; // Скорость персонажа
    private float _jumpPower = 8f;  // Высота прыжка
    private float _gravitationForce = 4f;
    private Vector3 _playerPosition;

    void Start()
    {
        playersOnScene = GetComponent<ObjectsOnScene>();
        playerController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;   // Блокировка курсора
    }

    void Update()
    {
        PlayerGravitation();

        Vector3 movement = new Vector3();
        movement.x = 1;
        movement.z = 1;    
        
        PlayerWalking(movement, 1.5f);  
    }

    // Ходьба персонажа:
    private void PlayerWalking(Vector3 movementVector, float movementCoefficient) 
    {
        float playerDirectionX = movementVector.x;
        float playerDirectionZ = movementVector.z;

        transform.Translate(Vector3.forward * _playerSpeed * playerDirectionZ * Time.deltaTime + Vector3.right * _playerSpeed * playerDirectionZ * Time.deltaTime);       
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
        PlayerJumping();
    }

    private void PlayerFalling() {
        _playerPosition = Vector3.zero;
        _playerPosition.y = _gravitationForce;
        playerController.Move(_playerPosition * Time.deltaTime);
    }

    // Прыжок персонажа:
    private void PlayerJumping() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
        {
            _gravitationForce = _jumpPower;
        }
    }

    // Функция, возвращающая объекты поблизости
    private Dictionary<float, string> CheckAround() 
    {
        return playersOnScene.GetPlayersPosition(player.transform.position);
    }
}
