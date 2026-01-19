using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private Ball _prefabBall;

    [SerializeField]
    private PhysxBall _prefabPhysxBall;
    
    private Material _material;

    [Networked]
    private TickTimer delay { get; set; }
    
    [Networked]
    public bool spawnedProjectile { get; set; }

    [Networked]
    private NetworkButtons PreviousButtons { get; set; }

    private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;
    private ChangeDetector _changeDetector;

    private bool canPossess = false;
    private weakMind wm;
    private GameObject enemy;
    public NetworkObject thisDude;

    public Camera Camera;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _material = GetComponentInChildren<MeshRenderer>().material;
    }
    
    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        if (HasInputAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<CameraBehaviour>().target = transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;

            //if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            //{
            //    if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
            //    {
            //        delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            //        Runner.Spawn(_prefabBall, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority, (runner, o) => { o.GetComponent<Ball>().Init(); });
            //        spawnedProjectile = !spawnedProjectile;
            //    }
            //    else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
            //    {
            //        delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            //        Runner.Spawn(_prefabPhysxBall, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority, (runner, o) => { o.GetComponent<PhysxBall>().Init(10 * _forward); });
            //        spawnedProjectile = !spawnedProjectile;
            //    }
            //}

            if(HasStateAuthority && canPossess == true && Input.GetKeyDown(KeyCode.Space))
            {
                BasicSpawner BS = FindFirstObjectByType<BasicSpawner>();
                NetworkPrefabRef creatureType = wm.creatureType;
                Vector3 spawnPoint = enemy.transform.position;
                Destroy(enemy);
                //BS.WMSpawn(thisDude, creatureType, spawnPoint);
            }
        }
    }

    public void Update()
    {
        /*if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }*/
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the weakMind script
        enemy = other.gameObject;
        wm = other.gameObject.GetComponent<weakMind>();

        if (wm != null)
        {
            // The object has the weakMind script
            Debug.Log("Collided with an object that has weakMind!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        wm = other.gameObject.GetComponent<weakMind>();
        enemy = other.gameObject;

        if (wm != null)
        {
            // The object has the weakMind script
            Debug.Log("Collided with an object that has weakMind!");
            canPossess = true;
            Debug.Log("Can Possess!");

        }
        else
        {
            canPossess = false;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Can no longer possess");
        canPossess = false;
        wm = null;
    }

    private TMP_Text _messages;

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
       RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (_messages == null)
            _messages = FindObjectOfType<TMP_Text>();

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }
        
        _messages.text += message;
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    _material.color = Color.white;
                    break;
            }
        }
        
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }
}