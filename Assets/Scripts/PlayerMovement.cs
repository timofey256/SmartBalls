using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO:
  - Обнаружение персонажем пропасти поблизости;
  - 
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
        PlayerWalking();
        PlayerGravitation();      
    }

    // Ходьба персонажа:
    private void PlayerWalking() 
    {
        _playerPosition = Vector3.zero;
        _playerPosition.x = Input.GetAxis("Horizontal") * _playerSpeed;
        _playerPosition.z = Input.GetAxis("Vertical") * _playerSpeed;

        _playerPosition.y = _gravitationForce;  // Падение

        playerController.Move(_playerPosition * Time.deltaTime);
    }

    // Гравитация персонажа:
    private void PlayerGravitation() 
    {
        // Если персонаж в воздухе, то падает:
        if (!playerController.isGrounded)
        {
            _gravitationForce -= 20f * Time.deltaTime;
        }

        PlayerJumping();
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
