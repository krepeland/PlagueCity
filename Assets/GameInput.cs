// GENERATED AUTOMATICALLY FROM 'Assets/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""787942bb-1e34-4e10-ad33-e9425684ae08"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""096d8fd8-3575-4cff-92fe-f684709aaec9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse_Delta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""060a9f0a-a364-4b3b-a3c8-0b18fba54302"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse_Left"",
                    ""type"": ""Button"",
                    ""id"": ""86d4609d-cbab-4561-b1a1-945f34fdfd1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse_Right"",
                    ""type"": ""Button"",
                    ""id"": ""c1829f7f-1517-4aa3-b6a4-3e942715146a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse_Middle"",
                    ""type"": ""Button"",
                    ""id"": ""d1ff9a6f-7679-425b-ba1e-bc5bc1375187"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse_Position"",
                    ""type"": ""Value"",
                    ""id"": ""064dbcc1-75c4-40e8-b76a-e5a0d621cc87"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse_Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""267e90e2-35a5-4157-97f3-60abd7d59d26"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""154bebe0-e975-4cdf-9b0f-eb4efae7fa2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""da27f949-61f9-4dc2-8e7c-016d3d67baea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""66fcce47-333f-4822-bbe5-893070896eaa"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1f0958e6-5e96-41a4-a210-36499b9ac385"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""671e367d-471e-42fa-9dbf-495b884f8fbb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e90e5054-a25d-499b-b9e2-e841ab67566b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""52ffcba9-0de9-4cf2-ad0c-edc9ce23bc39"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse_Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""365a79fe-8494-49e3-bc97-873da843902e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee6fa1a3-294d-454d-b790-36c2ab0b4976"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96f3593d-7db1-481d-9f7c-f8f21ddc1b4a"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse_Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""984f54b3-7afe-4b2b-87b9-7d043b3bfe89"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse_Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""045e871b-4402-4175-8da9-bb354416802c"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse_Middle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cb0734c-e701-413a-b8a8-777b3463eff5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Mouse_Delta = m_Gameplay.FindAction("Mouse_Delta", throwIfNotFound: true);
        m_Gameplay_Mouse_Left = m_Gameplay.FindAction("Mouse_Left", throwIfNotFound: true);
        m_Gameplay_Mouse_Right = m_Gameplay.FindAction("Mouse_Right", throwIfNotFound: true);
        m_Gameplay_Mouse_Middle = m_Gameplay.FindAction("Mouse_Middle", throwIfNotFound: true);
        m_Gameplay_Mouse_Position = m_Gameplay.FindAction("Mouse_Position", throwIfNotFound: true);
        m_Gameplay_Mouse_Scroll = m_Gameplay.FindAction("Mouse_Scroll", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Mouse_Delta;
    private readonly InputAction m_Gameplay_Mouse_Left;
    private readonly InputAction m_Gameplay_Mouse_Right;
    private readonly InputAction m_Gameplay_Mouse_Middle;
    private readonly InputAction m_Gameplay_Mouse_Position;
    private readonly InputAction m_Gameplay_Mouse_Scroll;
    private readonly InputAction m_Gameplay_Pause;
    public struct GameplayActions
    {
        private @GameInput m_Wrapper;
        public GameplayActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Mouse_Delta => m_Wrapper.m_Gameplay_Mouse_Delta;
        public InputAction @Mouse_Left => m_Wrapper.m_Gameplay_Mouse_Left;
        public InputAction @Mouse_Right => m_Wrapper.m_Gameplay_Mouse_Right;
        public InputAction @Mouse_Middle => m_Wrapper.m_Gameplay_Mouse_Middle;
        public InputAction @Mouse_Position => m_Wrapper.m_Gameplay_Mouse_Position;
        public InputAction @Mouse_Scroll => m_Wrapper.m_Gameplay_Mouse_Scroll;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Mouse_Delta.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Delta;
                @Mouse_Delta.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Delta;
                @Mouse_Delta.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Delta;
                @Mouse_Left.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Left;
                @Mouse_Left.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Left;
                @Mouse_Left.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Left;
                @Mouse_Right.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Right;
                @Mouse_Right.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Right;
                @Mouse_Right.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Right;
                @Mouse_Middle.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Middle;
                @Mouse_Middle.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Middle;
                @Mouse_Middle.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Middle;
                @Mouse_Position.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Position;
                @Mouse_Position.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Position;
                @Mouse_Position.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Position;
                @Mouse_Scroll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Scroll;
                @Mouse_Scroll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Scroll;
                @Mouse_Scroll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouse_Scroll;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Mouse_Delta.started += instance.OnMouse_Delta;
                @Mouse_Delta.performed += instance.OnMouse_Delta;
                @Mouse_Delta.canceled += instance.OnMouse_Delta;
                @Mouse_Left.started += instance.OnMouse_Left;
                @Mouse_Left.performed += instance.OnMouse_Left;
                @Mouse_Left.canceled += instance.OnMouse_Left;
                @Mouse_Right.started += instance.OnMouse_Right;
                @Mouse_Right.performed += instance.OnMouse_Right;
                @Mouse_Right.canceled += instance.OnMouse_Right;
                @Mouse_Middle.started += instance.OnMouse_Middle;
                @Mouse_Middle.performed += instance.OnMouse_Middle;
                @Mouse_Middle.canceled += instance.OnMouse_Middle;
                @Mouse_Position.started += instance.OnMouse_Position;
                @Mouse_Position.performed += instance.OnMouse_Position;
                @Mouse_Position.canceled += instance.OnMouse_Position;
                @Mouse_Scroll.started += instance.OnMouse_Scroll;
                @Mouse_Scroll.performed += instance.OnMouse_Scroll;
                @Mouse_Scroll.canceled += instance.OnMouse_Scroll;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMouse_Delta(InputAction.CallbackContext context);
        void OnMouse_Left(InputAction.CallbackContext context);
        void OnMouse_Right(InputAction.CallbackContext context);
        void OnMouse_Middle(InputAction.CallbackContext context);
        void OnMouse_Position(InputAction.CallbackContext context);
        void OnMouse_Scroll(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
