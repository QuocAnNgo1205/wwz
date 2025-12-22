

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// Provides programmatic access to <see cref="InputActionAsset" />, <see cref="InputActionMap" />, <see cref="InputAction" /> and <see cref="InputControlScheme" /> instances defined in asset "Assets/Scripts/Player/PlayerInputActions.inputactions".
/// </summary>
/// <remarks>
/// This class is source generated and any manual edits will be discarded if the associated asset is reimported or modified.
/// </remarks>
/// <example>
/// <code>
/// using namespace UnityEngine;
/// using UnityEngine.InputSystem;
///
/// // Example of using an InputActionMap named "Player" from a UnityEngine.MonoBehaviour implementing callback interface.
/// public class Example : MonoBehaviour, MyActions.IPlayerActions
/// {
///     private MyActions_Actions m_Actions;                  // Source code representation of asset.
///     private MyActions_Actions.PlayerActions m_Player;     // Source code representation of action map.
///
///     void Awake()
///     {
///         m_Actions = new MyActions_Actions();              // Create asset object.
///         m_Player = m_Actions.Player;                      // Extract action map object.
///         m_Player.AddCallbacks(this);                      // Register callback interface IPlayerActions.
///     }
///
///     void OnDestroy()
///     {
///         m_Actions.Dispose();                              // Destroy asset object.
///     }
///
///     void OnEnable()
///     {
///         m_Player.Enable();                                // Enable all actions within map.
///     }
///
///     void OnDisable()
///     {
///         m_Player.Disable();                               // Disable all actions within map.
///     }
///
///     #region Interface implementation of MyActions.IPlayerActions
///
///     // Invoked when "Move" action is either started, performed or canceled.
///     public void OnMove(InputAction.CallbackContext context)
///     {
///         Debug.Log($"OnMove: {context.ReadValue&lt;Vector2&gt;()}");
///     }
///
///     // Invoked when "Attack" action is either started, performed or canceled.
///     public void OnAttack(InputAction.CallbackContext context)
///     {
///         Debug.Log($"OnAttack: {context.ReadValue&lt;float&gt;()}");
///     }
///
///     #endregion
/// }
/// </code>
/// </example>
public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    /// <summary>
    /// Provides access to the underlying asset instance.
    /// </summary>
    public InputActionAsset asset { get; }

    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""version"": 1,
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Move"",
            ""id"": ""9e54d4e2-8a96-4f84-8ff9-ac5a5c734aa0"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Value"",
                    ""id"": ""8c989323-675c-4665-ad65-934cc45e8d37"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2cea5a22-ab62-4324-8e21-84f0fb427d02"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""41b7207e-f336-4a33-9607-8c2a470a4485"",
                    ""path"": ""w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""168924d6-356a-4f24-b37b-e9b1bc4b4c57"",
                    ""path"": ""s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""00018cff-2e7d-4b81-93f4-e9f1f189bdd8"",
                    ""path"": ""a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fe0859c4-54be-4132-a408-62a1e475fa5c"",
                    ""path"": ""d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Run"",
            ""id"": ""e7eb6a4c-bd17-48f5-b5a3-ed7f3059e292"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""e91f7ba6-411b-48c7-a537-f034d045e7f0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff4b6a21-5365-4215-aea2-5fa00779e04a"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Crouch"",
            ""id"": ""a98081a3-90a7-4bc6-95f1-5f5ba7046e6b"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""2148a857-0131-461a-995b-fa9d849e82d7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e0be2dca-0776-40c9-be24-a6163d0a9fc4"",
                    ""path"": ""<Keyboard>/#(C)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Jump"",
            ""id"": ""a5368d60-fad3-46f0-bac6-7a9e6c069643"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""8de67da3-604d-4972-b195-2cd402705174"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c8c11bd9-0f70-4a25-9184-088b385fc5f6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Shoot"",
            ""id"": ""a24ab935-e65f-44bd-9fca-b3007556e0dc"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""6a84a2c0-6d23-41a9-9f12-90137ad2920b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""43093bcf-d636-4d73-8432-bdae4b80e353"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_Newaction = m_Move.FindAction("New action", throwIfNotFound: true);
        // Run
        m_Run = asset.FindActionMap("Run", throwIfNotFound: true);
        m_Run_Newaction = m_Run.FindAction("New action", throwIfNotFound: true);
        // Crouch
        m_Crouch = asset.FindActionMap("Crouch", throwIfNotFound: true);
        m_Crouch_Newaction = m_Crouch.FindAction("New action", throwIfNotFound: true);
        // Jump
        m_Jump = asset.FindActionMap("Jump", throwIfNotFound: true);
        m_Jump_Newaction = m_Jump.FindAction("New action", throwIfNotFound: true);
        // Shoot
        m_Shoot = asset.FindActionMap("Shoot", throwIfNotFound: true);
        m_Shoot_Newaction = m_Shoot.FindAction("New action", throwIfNotFound: true);
    }

    ~@PlayerInputActions()
    {
        UnityEngine.Debug.Assert(!m_Move.enabled, "This will cause a leak and performance issues, PlayerInputActions.Move.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_Run.enabled, "This will cause a leak and performance issues, PlayerInputActions.Run.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_Crouch.enabled, "This will cause a leak and performance issues, PlayerInputActions.Crouch.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_Jump.enabled, "This will cause a leak and performance issues, PlayerInputActions.Jump.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_Shoot.enabled, "This will cause a leak and performance issues, PlayerInputActions.Shoot.Disable() has not been called.");
    }

    /// <summary>
    /// Destroys this asset and all associated <see cref="InputAction"/> instances.
    /// </summary>
    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindingMask" />
    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.devices" />
    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.controlSchemes" />
    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Contains(InputAction)" />
    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.GetEnumerator()" />
    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Enable()" />
    public void Enable()
    {
        asset.Enable();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Disable()" />
    public void Disable()
    {
        asset.Disable();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindings" />
    public IEnumerable<InputBinding> bindings => asset.bindings;

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindAction(string, bool)" />
    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindBinding(InputBinding, out InputAction)" />
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Move
    private readonly InputActionMap m_Move;
    private List<IMoveActions> m_MoveActionsCallbackInterfaces = new List<IMoveActions>();
    private readonly InputAction m_Move_Newaction;
    /// <summary>
    /// Provides access to input actions defined in input action map "Move".
    /// </summary>
    public struct MoveActions
    {
        private @PlayerInputActions m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public MoveActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "Move/Newaction".
        /// </summary>
        public InputAction @Newaction => m_Wrapper.m_Move_Newaction;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="MoveActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="MoveActions" />
        public void AddCallbacks(IMoveActions instance)
        {
            if (instance == null || m_Wrapper.m_MoveActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MoveActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="MoveActions" />
        private void UnregisterCallbacks(IMoveActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="MoveActions.UnregisterCallbacks(IMoveActions)" />.
        /// </summary>
        /// <seealso cref="MoveActions.UnregisterCallbacks(IMoveActions)" />
        public void RemoveCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="MoveActions.AddCallbacks(IMoveActions)" />
        /// <seealso cref="MoveActions.RemoveCallbacks(IMoveActions)" />
        /// <seealso cref="MoveActions.UnregisterCallbacks(IMoveActions)" />
        public void SetCallbacks(IMoveActions instance)
        {
            foreach (var item in m_Wrapper.m_MoveActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MoveActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="MoveActions" /> instance referencing this action map.
    /// </summary>
    public MoveActions @Move => new MoveActions(this);

    // Run
    private readonly InputActionMap m_Run;
    private List<IRunActions> m_RunActionsCallbackInterfaces = new List<IRunActions>();
    private readonly InputAction m_Run_Newaction;
    /// <summary>
    /// Provides access to input actions defined in input action map "Run".
    /// </summary>
    public struct RunActions
    {
        private @PlayerInputActions m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public RunActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "Run/Newaction".
        /// </summary>
        public InputAction @Newaction => m_Wrapper.m_Run_Newaction;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_Run; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="RunActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(RunActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="RunActions" />
        public void AddCallbacks(IRunActions instance)
        {
            if (instance == null || m_Wrapper.m_RunActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RunActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="RunActions" />
        private void UnregisterCallbacks(IRunActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="RunActions.UnregisterCallbacks(IRunActions)" />.
        /// </summary>
        /// <seealso cref="RunActions.UnregisterCallbacks(IRunActions)" />
        public void RemoveCallbacks(IRunActions instance)
        {
            if (m_Wrapper.m_RunActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="RunActions.AddCallbacks(IRunActions)" />
        /// <seealso cref="RunActions.RemoveCallbacks(IRunActions)" />
        /// <seealso cref="RunActions.UnregisterCallbacks(IRunActions)" />
        public void SetCallbacks(IRunActions instance)
        {
            foreach (var item in m_Wrapper.m_RunActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RunActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="RunActions" /> instance referencing this action map.
    /// </summary>
    public RunActions @Run => new RunActions(this);

    // Crouch
    private readonly InputActionMap m_Crouch;
    private List<ICrouchActions> m_CrouchActionsCallbackInterfaces = new List<ICrouchActions>();
    private readonly InputAction m_Crouch_Newaction;
    /// <summary>
    /// Provides access to input actions defined in input action map "Crouch".
    /// </summary>
    public struct CrouchActions
    {
        private @PlayerInputActions m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public CrouchActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "Crouch/Newaction".
        /// </summary>
        public InputAction @Newaction => m_Wrapper.m_Crouch_Newaction;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_Crouch; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="CrouchActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(CrouchActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="CrouchActions" />
        public void AddCallbacks(ICrouchActions instance)
        {
            if (instance == null || m_Wrapper.m_CrouchActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CrouchActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="CrouchActions" />
        private void UnregisterCallbacks(ICrouchActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="CrouchActions.UnregisterCallbacks(ICrouchActions)" />.
        /// </summary>
        /// <seealso cref="CrouchActions.UnregisterCallbacks(ICrouchActions)" />
        public void RemoveCallbacks(ICrouchActions instance)
        {
            if (m_Wrapper.m_CrouchActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="CrouchActions.AddCallbacks(ICrouchActions)" />
        /// <seealso cref="CrouchActions.RemoveCallbacks(ICrouchActions)" />
        /// <seealso cref="CrouchActions.UnregisterCallbacks(ICrouchActions)" />
        public void SetCallbacks(ICrouchActions instance)
        {
            foreach (var item in m_Wrapper.m_CrouchActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CrouchActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="CrouchActions" /> instance referencing this action map.
    /// </summary>
    public CrouchActions @Crouch => new CrouchActions(this);

    // Jump
    private readonly InputActionMap m_Jump;
    private List<IJumpActions> m_JumpActionsCallbackInterfaces = new List<IJumpActions>();
    private readonly InputAction m_Jump_Newaction;
    /// <summary>
    /// Provides access to input actions defined in input action map "Jump".
    /// </summary>
    public struct JumpActions
    {
        private @PlayerInputActions m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public JumpActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "Jump/Newaction".
        /// </summary>
        public InputAction @Newaction => m_Wrapper.m_Jump_Newaction;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_Jump; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="JumpActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(JumpActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="JumpActions" />
        public void AddCallbacks(IJumpActions instance)
        {
            if (instance == null || m_Wrapper.m_JumpActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_JumpActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="JumpActions" />
        private void UnregisterCallbacks(IJumpActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="JumpActions.UnregisterCallbacks(IJumpActions)" />.
        /// </summary>
        /// <seealso cref="JumpActions.UnregisterCallbacks(IJumpActions)" />
        public void RemoveCallbacks(IJumpActions instance)
        {
            if (m_Wrapper.m_JumpActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="JumpActions.AddCallbacks(IJumpActions)" />
        /// <seealso cref="JumpActions.RemoveCallbacks(IJumpActions)" />
        /// <seealso cref="JumpActions.UnregisterCallbacks(IJumpActions)" />
        public void SetCallbacks(IJumpActions instance)
        {
            foreach (var item in m_Wrapper.m_JumpActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_JumpActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="JumpActions" /> instance referencing this action map.
    /// </summary>
    public JumpActions @Jump => new JumpActions(this);

    // Shoot
    private readonly InputActionMap m_Shoot;
    private List<IShootActions> m_ShootActionsCallbackInterfaces = new List<IShootActions>();
    private readonly InputAction m_Shoot_Newaction;
    /// <summary>
    /// Provides access to input actions defined in input action map "Shoot".
    /// </summary>
    public struct ShootActions
    {
        private @PlayerInputActions m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public ShootActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "Shoot/Newaction".
        /// </summary>
        public InputAction @Newaction => m_Wrapper.m_Shoot_Newaction;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_Shoot; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="ShootActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(ShootActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="ShootActions" />
        public void AddCallbacks(IShootActions instance)
        {
            if (instance == null || m_Wrapper.m_ShootActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ShootActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="ShootActions" />
        private void UnregisterCallbacks(IShootActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="ShootActions.UnregisterCallbacks(IShootActions)" />.
        /// </summary>
        /// <seealso cref="ShootActions.UnregisterCallbacks(IShootActions)" />
        public void RemoveCallbacks(IShootActions instance)
        {
            if (m_Wrapper.m_ShootActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="ShootActions.AddCallbacks(IShootActions)" />
        /// <seealso cref="ShootActions.RemoveCallbacks(IShootActions)" />
        /// <seealso cref="ShootActions.UnregisterCallbacks(IShootActions)" />
        public void SetCallbacks(IShootActions instance)
        {
            foreach (var item in m_Wrapper.m_ShootActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ShootActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="ShootActions" /> instance referencing this action map.
    /// </summary>
    public ShootActions @Shoot => new ShootActions(this);
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Move" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="MoveActions.AddCallbacks(IMoveActions)" />
    /// <seealso cref="MoveActions.RemoveCallbacks(IMoveActions)" />
    public interface IMoveActions
    {
        /// <summary>
        /// Method invoked when associated input action "New action" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnNewaction(InputAction.CallbackContext context);
    }
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Run" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="RunActions.AddCallbacks(IRunActions)" />
    /// <seealso cref="RunActions.RemoveCallbacks(IRunActions)" />
    public interface IRunActions
    {
        /// <summary>
        /// Method invoked when associated input action "New action" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnNewaction(InputAction.CallbackContext context);
    }
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Crouch" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="CrouchActions.AddCallbacks(ICrouchActions)" />
    /// <seealso cref="CrouchActions.RemoveCallbacks(ICrouchActions)" />
    public interface ICrouchActions
    {
        /// <summary>
        /// Method invoked when associated input action "New action" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnNewaction(InputAction.CallbackContext context);
    }
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Jump" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="JumpActions.AddCallbacks(IJumpActions)" />
    /// <seealso cref="JumpActions.RemoveCallbacks(IJumpActions)" />
    public interface IJumpActions
    {
        /// <summary>
        /// Method invoked when associated input action "New action" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnNewaction(InputAction.CallbackContext context);
    }
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Shoot" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="ShootActions.AddCallbacks(IShootActions)" />
    /// <seealso cref="ShootActions.RemoveCallbacks(IShootActions)" />
    public interface IShootActions
    {
        /// <summary>
        /// Method invoked when associated input action "New action" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnNewaction(InputAction.CallbackContext context);
    }
}
