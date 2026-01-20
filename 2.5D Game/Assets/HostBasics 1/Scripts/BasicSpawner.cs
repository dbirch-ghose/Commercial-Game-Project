using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using MoreMountains.Feel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;


public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Room Configuration")]
    [Tooltip("Enable to read room name from RoomConfig.txt on desktop")]
    public bool useRoomConfigFile = true;

    public Vector3 spawnPoint;
    private NetworkRunner _runner;
    private PlayerRef Possessor;
    public GameObject starter;
    public GameObject books;
    public GameObject introDialogue;
    public LuaChanger referencer;
    //public NetworkObject networkPlayerObject;
    private string _currentRoomName;
    public booksSpawner bookSpawner;
    public NetworkObject blockers;
    private referencer referenceBlock;

    private void Awake()
    {
        // Load the configured room name when the scene starts
        //if (useRoomConfigFile)
        //{
        //    _currentRoomName = RoomConfigReader.GetConfiguredRoomName();
        //}
        
        //else
        //{
        //    _currentRoomName = "TestRoom";
        //}
        _currentRoomName = UIController.roomCode;
        referenceBlock = FindFirstObjectByType<referencer>();
        referencer = FindFirstObjectByType<LuaChanger>();
        if (UIController.hosting) { StartGame(GameMode.Host); }
        else { StartGame(GameMode.Client); }
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            // Display the current room ID at the top
            string roomLabel = useRoomConfigFile ? $"Room ID: {_currentRoomName}" : $"Room ID: {_currentRoomName} (Default)";
            GUI.Label(new Rect(210, 10, 400, 30), roomLabel);

            //if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            //{
            //    StartGame(GameMode.Host);
            //    //starter.SetActive(true);

            //}

            //if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            //{
            //    StartGame(GameMode.Client);
            //    //starter.SetActive(true);

            //}
        }
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var runnerSimulatePhysics3D = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        runnerSimulatePhysics3D.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = _currentRoomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    [SerializeField]
    private NetworkPrefabRef _playerPrefab;

    [SerializeField]
    private NetworkPrefabRef _player2Prefab;// Character to spawn for a joining player

    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public List<NetworkObject> players = new List<NetworkObject>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {

            // Create a unique position for the player
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            Vector3 spawnPosition = new Vector3();
            spawnPosition = this.GetComponent<Transform>().position;

            if (_spawnedCharacters.Count == 0)
            {
                Possessor = player;
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
                players.Add(networkPlayerObject);

                //take this out for multiplayer
                Rpc_EnableIntroDialogue();

            }
            else
            {
                Possessor = player;
                NetworkObject networkPlayerObject = runner.Spawn(_player2Prefab, spawnPosition, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
                players.Add(networkPlayerObject);
                Rpc_EnableIntroDialogue();
                //bookSpawner.spawnBooks();
            }
            // Keep track of the player avatars for easy access

        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void Rpc_EnableIntroDialogue()
    {
        Rpc_EnableIntroDialogueBroadcast();
    }
    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    void Rpc_EnableIntroDialogueBroadcast()
    {
        introDialogue.gameObject.SetActive(true);
        referenceBlock.waitText.SetActive(false);
    }

    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_WMSpawnNow(NetworkObject fallen, NetworkPrefabRef enemyType, Vector3 spawnPosition) {
        _runner.Despawn(fallen);
        _runner.Spawn(enemyType, spawnPosition, Quaternion.identity, Possessor);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    private bool _mouseButton0;
    private bool _mouseButton1;

    private void Update()
    {
        _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
        _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        
        data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
        _mouseButton0 = false;
        data.buttons.Set(NetworkInputData.MOUSEBUTTON1, _mouseButton1);
        _mouseButton1 = false;
        
        input.Set(data);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestDestroy(NetworkObject target)
    {
        if (target == null)
            return;

        // Safety check
        if (!target.HasStateAuthority)
            return;

        RPC_Destroy(target);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Destroy(NetworkObject target)
    {
        _runner.Despawn(target);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestRespawn(NetworkPrefabRef target, PlayerRef player)
    {
        RPC_Respawn(target, player);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Respawn(NetworkPrefabRef target, PlayerRef player)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition = this.GetComponent<Transform>().position;
        _runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
    }
    public void RequestUnlock(int unlockId)
    {
        Rpc_RequestUnlock(unlockId);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void Rpc_RequestUnlock(int unlockId)
    {
        // Authority tells everyone (including itself)
        Rpc_ApplyUnlock(unlockId);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_ApplyUnlock(int unlockId)
    {
        referencer.RPC_luaChange(unlockId);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RequestKillBlocks()
    {
        // Authority tells everyone (including itself)
        Rpc_KillBlocks();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_KillBlocks()
    {
        if (blockers.HasInputAuthority)
        {
            blockers.gameObject.SetActive(false);
        }
        
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_spawnFlies()
    {
        //if (referenceBlock == null)
        //{
        //    Debug.Log("referencer is null");
        //}
        //Debug.Log("Running spawn flies");
        //if (referenceBlock.flyPrefab == null)
        //{
        //    Debug.Log("fly prefab is null");
        //}
        //if (referenceBlock.flyDrop == null)
        //{
        //    Debug.Log("flyDrop is null");
        //}
        RPC_ActualSpawnFlies();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ActualSpawnFlies()
    {
        _runner.Spawn(referenceBlock.flyPrefab, referenceBlock.flyDrop.position);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_requestMove(GameObject block, Vector3 move)
    {
        RPC_applyMove(block, move);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_applyMove(GameObject block, Vector3 move)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }
    
    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }
}