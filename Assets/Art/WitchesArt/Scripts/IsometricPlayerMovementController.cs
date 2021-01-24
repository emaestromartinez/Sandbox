using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    Vector2 movementVector;
    float horizontalInput;
    float verticalInput;
    bool lastActionWasClick;

    [SerializeField] private Tilemap map;
    private Vector3 destination;
    // public GridLayout grid;
    Camera mainCamera;

    MovementInput movementInput;

    private void Awake()
    {
        movementInput = new MovementInput();
        rbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    private void OnEnable()
    {
        movementInput.Enable();
    }
    private void OnDisable()
    {
        movementInput.Disable();

    }
    private void Start()
    {
        movementInput.Mouse.MouseClick.performed += (_) => MouseClick();
        movementInput.Keyboard.Movement.performed += (context) =>
        {
            lastActionWasClick = false;
            movementVector = context.ReadValue<Vector2>();
            Debug.Log("performed");
        };
        movementInput.Keyboard.Movement.canceled += (_) =>
        {
            movementVector = Vector2.zero;
            Debug.Log("cancelled");
        };

        destination = transform.position;
    }

    private void MouseClick()
    {

        Vector2 mousePosition = movementInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // Make sure we are clicking the cell, not the map not the screen;
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        if (map.HasTile(gridPosition))
        {
            destination = mousePosition;
        }
        lastActionWasClick = true;
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

        Vector2 inputVector = new Vector2(movementVector.x, movementVector.y);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        // isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);


        // Only line needed for mouse movement in FixedUpdate;
        if (lastActionWasClick && Vector3.Distance(rbody.position, destination) > 0.1f)
        {
            Debug.Log("clickmoveee");
            rbody.position = Vector3.MoveTowards(rbody.position, destination, movementSpeed * Time.deltaTime);
        }
    }
}
