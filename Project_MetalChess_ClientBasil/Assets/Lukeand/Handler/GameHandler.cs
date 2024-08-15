using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    //THIS EXISTSS TO GATHER ALL THE ASSETS TOGETHER

    public static GameHandler instance;

    public ObserverHandler _observer { get; private set; }

    public SoundHandler _sound {  get; private set; }

    public SceneLoaderHandler _sceneLoader { get; private set; }


    public TableHandler _table { get; private set; }

    public SinglePlayerHandler _singlePlayer { get; private set; }

    public PoolHandler_Main _pool_Main { get; private set; }


    public PlayerInputHandler _playerInput { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {

            //this is going to passa anyref it might require.]
            GameHandler.instance.UpdatePlayers(player_1, player_2);
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        _observer = GetComponent<ObserverHandler>();
        _table = GetComponent<TableHandler>();
        _sound = GetComponent<SoundHandler>();
        _singlePlayer = GetComponent<SinglePlayerHandler>();
        _pool_Main = GetComponent<PoolHandler_Main>();
        _sceneLoader = GetComponent<SceneLoaderHandler>();
        _playerInput = GetComponent<PlayerInputHandler>();

        DOTween.Init();
    }


    private void Start()
    {
        
        if (startSinglePlayer)
        {
            _singlePlayer.StartSinglePlayer();
        }

        _observer.eventAMagneticCollided += OnMagnetCollided;
    }

    void OnMagnetCollided(Magnetic magnet)
    {
        //return it to the original position.
        Debug.Log("one");

        magnet.ReturnToOriginalPosition();

    }


    [Separator("DEBUG")]
    [SerializeField] bool startSinglePlayer;


    [Separator("PLAYERS")]
    [SerializeField] Player player_1;
    [SerializeField] Player player_2;

    public Player Player_1 { get {  return player_1; } }
    public Player Player_2 { get { return player_2; } }

    public void UpdatePlayers(Player player_1, Player player_2)
    {

        Debug.Log("received new players " + gameObject.name);
        this.player_1 = player_1;
        this.player_2 = player_2;


        
    }


}


//how will we organize this?
//we can do it all in the same scene?
//single player - Spawn the first surface, put the pieces in the surface, and count the timer; change the ui of position
//local coop - 
//multiplayer - 