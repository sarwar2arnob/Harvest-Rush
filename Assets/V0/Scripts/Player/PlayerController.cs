using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static event Action<ToolData> OnToolSwitched;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Tools")]
    [SerializeField] private List<ToolData> toolBelt;
    [SerializeField] private Vector3 toolHolderOffset;

    [Header("Interaction")]
    [SerializeField] private float _interactionDistance = 1.0f;
    [SerializeField] private LayerMask _interactionLayers;

    private List<GameObject> instantiatedTools = new List<GameObject>();
    private int _currentToolIndex = 0;
    private bool _isSwinging = false;
    private bool _facingRight = true;
    private Transform toolHolder;
    private SpriteRenderer _spriteRenderer;

    private Vector2 moveInput;
    [SerializeField]private GameObject InventoryScreen;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (toolHolder == null)
        {
            GameObject holder = new GameObject("ToolHolder");
            holder.transform.SetParent(transform);
            holder.transform.localPosition = toolHolderOffset;
            toolHolder = holder.transform;
        }
    }

    private void Start()
    {
        InitializeTools();
    }

    private void Update()
    {
        PlayerStamina stamina = GetComponent<PlayerStamina>();
        if (stamina != null && !stamina.CanMove) return;

        if (moveInput.x > 0f && !_facingRight)
        {
            _facingRight = true;
            _spriteRenderer.flipX = true;
            toolHolder.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput.x < 0f && _facingRight)
        {
            _facingRight = false;
            _spriteRenderer.flipX = false;
            toolHolder.localScale = new Vector3(1, 1, 1);
        }

        Vector2 movement = moveInput.normalized * _moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        GetComponent<PlayerStamina>()?.SetMoveInput(moveInput);
    }
    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            GetComponent<PlayerStamina>()?.Attack();
        if (context.performed) PerformAction();
        if (context.performed)
            PerformAction();
        
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) PerformInteract();
    }

    
    public void OnScroll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float scrollInput = context.ReadValue<Vector2>().y;

        if (scrollInput > 0.1f)
            SwitchTool(-1);
        else if (scrollInput < -0.1f)
            SwitchTool(1);
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryScreen.SetActive(!InventoryScreen.activeSelf);
        }
    }
    private void PerformAction()
    {
        if (_isSwinging || instantiatedTools.Count == 0) return;

        _isSwinging = true;

        GameObject currentTool = instantiatedTools[_currentToolIndex];
        currentTool.SetActive(true);

        Quaternion startRotation = Quaternion.Euler(0, 0, -45f);
        Quaternion endRotation = Quaternion.Euler(0, 0, 45f);
        currentTool.transform.localRotation = startRotation;

        Sequence swingSequence = DOTween.Sequence();
        swingSequence.Append(currentTool.transform.DOLocalRotateQuaternion(endRotation, 0.25f).SetEase(Ease.OutQuad));
        swingSequence.OnComplete(() =>
        {
            _isSwinging = false;
            currentTool.SetActive(false);
        });
    }

    

    private void PerformInteract()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _interactionDistance, _interactionLayers);

        if (hit != null)
        {
            IInteract interactable = hit.GetComponent<IInteract>();
            if (interactable != null && interactable.CanInteract())
            {
                interactable.Interact();
            }
        }
    }

    
    private void InitializeTools()
    {

        foreach (ToolData toolData in toolBelt)
        {
            if (toolData.toolPrefab != null)
            {
                GameObject toolInstance = Instantiate(toolData.toolPrefab, toolHolder);
                toolInstance.SetActive(false);
                instantiatedTools.Add(toolInstance);
            }
        }
    }

    private void SwitchTool(int direction)
    {
        if (instantiatedTools.Count == 0) return;

        instantiatedTools[_currentToolIndex].SetActive(false);
        _currentToolIndex = (_currentToolIndex + direction + instantiatedTools.Count) % instantiatedTools.Count;

        if (toolBelt.Count > _currentToolIndex)
        {
            OnToolSwitched?.Invoke(toolBelt[_currentToolIndex]);
        }
    }

    public void SetToolHolderOffset(Vector3 offset)
    {
        toolHolderOffset = offset;
        if (toolHolder != null)
        {
            toolHolder.localPosition = toolHolderOffset;
        }
    }
}
