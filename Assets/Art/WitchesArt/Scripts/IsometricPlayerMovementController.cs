using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    float horizontalInput;
    float verticalInput;

    [SerializeField] private Tilemap map;
    private Vector3 destination;
    // public GridLayout grid;
    Camera mainCamera;

    MouseInput mouseInput;

    private void Awake()
    {
        mouseInput = new MouseInput();
        rbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    private void OnEnable()
    {
        mouseInput.Enable();
    }
    private void OnDisable()
    {
        mouseInput.Disable();

    }
    private void Start()
    {
        mouseInput.Mouse.MouseClick.performed += (_) => MouseClick();
        destination = transform.position;
    }

    private void MouseClick()
    {
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // Make sure we are clicking the cell, not the map not the screen;
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        if (map.HasTile(gridPosition))
        {
            destination = mousePosition;
        }
    }
    private void Update()
    {
        // horizontalInput = Input.GetAxis("Horizontal");
        // verticalInput = Input.GetAxis("Vertical");


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        // Below is calculating the speed based on the tile;
        // // TileBase tileBase = tilemap.GetTile(new Vector3Int((int)rbody.position.x, (int)rbody.position.y, 0));
        // string tileName = tileBase.name;
        // Debug.Log(tileName);
        // float speed = 50;
        // if (tileName.Contains("ice"))
        // {
        //     speed = 10;
        // }
        // Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * speed);

        // Get current tile;
        // Vector2Int cellPosition = (Vector2Int)grid.WorldToCell(new Vector3(currentPos.x, currentPos.y, 0f));

        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);



        // rbody.MovePosition(newPos);
        if (Vector3.Distance(rbody.position, destination) > 0.1f)
            rbody.position = Vector3.MoveTowards(rbody.position, destination, movementSpeed * Time.deltaTime);
    }
}
