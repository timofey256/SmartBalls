using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerController;
    private ObjectsOnScene objectsOnScene;  // Скрипт получения координат объектов на сцене
    public GameObject GroupPlayers; // Объект родитель всех особей (Create Player)

    private List<Vector3> _playerPositions = new List<Vector3>(); // test!

    public float _playerSpeed = 500f; // Скорость персонажа
    private float _jumpPower = 800f;  // Высота прыжка
    private float _gravitationForce = 400f;

    private Vector3 movementVector;
    private float playerDirectionX = 0f;
    private float playerDirectionZ = 0f;
    private float movementCoefficient = 0f;

    void Start() {
        objectsOnScene = GroupPlayers.GetComponent<ObjectsOnScene>();
        playerController = GetComponent<CharacterController>();

        movementVector = new Vector3(0f, 0f, 0f);

        

        //Cursor.lockState = CursorLockMode.Locked;   // Блокировка курсора
    }

    void Update()
    {
        this.PlayerGravitation();
        gameObject.transform.Translate(movementVector * _playerSpeed * movementCoefficient * Time.deltaTime);
    }

    // Ходьба персонажа
    // Осуществляется в соответствии с заданым вектором, пример: (1, 0, 0); (1, 0, -1)...
    public void SetDirectionWalking(Vector3 newMovementVector, float movementCoefficient) 
    {
        this.movementVector = newMovementVector;
        this.movementCoefficient = movementCoefficient;
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

    // Меняет цвет игрока в соответствии с переданой строкой
    // Доступные цвета: "Red", "Yellow", "Violet", "Blue"
    private void ChangePlayerColor(string color)
    {
        Dictionary<string, Material> colors = GetColors();
        gameObject.GetComponent<Renderer>().material = colors[color];
    }

    // Возвращает словарь со всеми материалами(цветами):
    private Dictionary<string, Material> GetColors()
    {
        Dictionary<string, Material> materials = new Dictionary<string, Material>();

        materials.Add("Red", Resources.Load("PlayersMaterial_Red", typeof(Material)) as Material);
        materials.Add("Yellow", Resources.Load("PlayersMaterial_Yellow", typeof(Material)) as Material);
        materials.Add("Violet", Resources.Load("PlayersMaterial_Violet", typeof(Material)) as Material);
        materials.Add("Blue", Resources.Load("PlayersMaterial_Blue", typeof(Material)) as Material);

        return materials;
    }

    // Функция, возвращающая объекты поблизости
    private List<Dictionary<float, string>> CheckAround() 
    {
        return objectsOnScene.GetObjectsPosition(gameObject.transform.position);
    }
}
