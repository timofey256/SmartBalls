using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 500f; // Скорость персонажа
    [SerializeField] private float _power; // Сила отталкивания
    private float _jumpPower = 800f;  // Высота прыжка
    private float _gravitationForce = 400f;

    private GameObject GroupPlayers; // Объект родитель всех особей (Create Player)
    private CharacterController playerController;
    private ObjectsOnScene objectsOnScene;  // Скрипт получения координат объектов на сцене

    private Vector3 movementVector;
    private float movementCoefficient = 0f;

    private void Start() 
    {
        playerController = GetComponent<CharacterController>();
        objectsOnScene = Resources.Load<ObjectsOnScene>("Scripts/PlayerBehaviour/ObjectsOnScene.cs");

        movementVector = new Vector3(0f, 0f, 0f);
        //Cursor.lockState = CursorLockMode.Locked;   // Блокировка курсора
    }

    private void Update()
    {
        PlayerGravitation();
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
    public void PlayerDestroy()
    {
        Destroy(gameObject);
    }

    // Меняет цвет игрока в соответствии с переданой строкой
    // Доступные цвета: "Red", "Yellow", "Violet", "Blue"
    public void ChangePlayerColor(string color)
    {
        Dictionary<string, Material> colors = GetColors();
        gameObject.GetComponent<Renderer>().material = colors[color];
    }

    // Возвращает словарь со всеми материалами(цветами):
    private Dictionary<string, Material> GetColors()
    {
        Dictionary<string, Material> materials = new Dictionary<string, Material>();

        materials.Add("Red", Resources.Load("Materials/PlayersMaterial_Red", typeof(Material)) as Material);
        materials.Add("Yellow", Resources.Load("Materials/PlayersMaterial_Yellow", typeof(Material)) as Material);
        materials.Add("Violet", Resources.Load("Materials/PlayersMaterial_Violet", typeof(Material)) as Material);
        materials.Add("Blue", Resources.Load("Materials/PlayersMaterial_Blue", typeof(Material)) as Material);

        return materials;
    }

    // Функция, возвращающая объекты поблизости
    private List<Dictionary<float, string>> CheckAround() 
    {
        return objectsOnScene.GetObjectsPosition(gameObject.transform.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody targetRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 targetPos = other.transform.position;
            Vector3 direction = (targetPos - gameObject.transform.position).normalized;

            targetRb.AddForce(direction * _power, ForceMode.Impulse);
        }
        
    }
}
