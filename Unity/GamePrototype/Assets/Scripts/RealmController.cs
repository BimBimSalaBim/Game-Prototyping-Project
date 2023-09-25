using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using MongoDB.Bson;
using System.Linq;
using Realms.Sync;

public class RealmController : MonoBehaviour
{
    
    static private string email = "train@train.com";
    static private string password = "123456";
    static public RealmController Instance;

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;

    [SerializeField] private string _realmAppId = "prototype-yozkn";

    async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        // Realm.DeleteRealm(RealmConfiguration.DefaultConfiguration);
        if (_realm == null)
        {
            _realmApp = App.Create(new AppConfiguration(_realmAppId));
            if (_realmApp.CurrentUser == null)
            {
                //TODO: change to login with username and password
                // _realmUser = await _realmApp.LogInAsync(Credentials.Anonymous());
                try{
                _realmUser = await _realmApp.LogInAsync(Credentials.EmailPassword(email, password));
                // _realmUser = SignIn(email, password);

                _realm = Realm.GetInstance(new PartitionSyncConfiguration(_realmUser.Id,_realmUser));
                }
                catch (System.Exception e){
                    Debug.Log(e);
                    Debug.Log("User Not Found");
                }

            }
            else
            {
                _realmUser = _realmApp.CurrentUser;
                _realm = Realm.GetInstance(new PartitionSyncConfiguration(_realmUser.Id,_realmUser));
            }
            
            
        }
        
        
    }

    void OnDisable()
    {
        if (_realm != null)
        {
            _realm.Dispose();
        }
    }

    public async void SignIn(string email, string password){
        if (_realm == null)
        {
            _realmApp = App.Create(new AppConfiguration(_realmAppId));
            if (_realmApp.CurrentUser == null)
            {
                try{
                _realmUser = await _realmApp.LogInAsync(Credentials.EmailPassword(email, password));
                // _realmUser = SignIn(email, password);

                _realm = Realm.GetInstance(new PartitionSyncConfiguration(_realmUser.Id,_realmUser));
                }
                catch (System.Exception e){
                    Debug.Log(e);
                    Debug.Log("User Not Found");
                }

            }
            else
            {
                _realmUser = _realmApp.CurrentUser;
                _realm = Realm.GetInstance(new PartitionSyncConfiguration(_realmUser.Id,_realmUser));
            }
            
            
        }
    }

    public async void SignUp(string email, string password){
        try{
        await _realmApp.EmailPasswordAuth.RegisterUserAsync(email, password);
        _realmUser = await _realmApp.LogInAsync(Credentials.EmailPassword(email, password));
        Debug.Log("User Created");
        }
        catch{
            Debug.Log("User Already Exists");
        }

    }

    private GameDataModel GetOrCreateGameData(){

        GameDataModel gameDataModel = _realm.All<GameDataModel>().Where(d => d.UserId == _realmUser.Id).FirstOrDefault();
        // Debug.Log("user id = "+gameDataModel.GetPosition());
        if (gameDataModel == null)
        {
            _realm.Write(() =>
            {
                Debug.Log("create new game data");
                gameDataModel = _realm.Add(new GameDataModel
                {
                    UserId = _realmUser.Id,
                    Name = new string(_realmUser.Id.ToCharArray().Reverse().ToArray()),
                    PositionX = 1,
                    PositionY = 1,
                    PositionZ = 1,
                    
                });
            });
            Debug.Log("ID = "+gameDataModel.Id);
        }
        return gameDataModel;
    }

    public bool IsRealmReady() {
        return _realm != null;
    }

    public Vector3 GetPosition(){
        GameDataModel gameDataModel = GetOrCreateGameData();
        // Debug.Log("get position");
        Debug.Log(gameDataModel.PositionX + " " + gameDataModel.PositionY + " " + gameDataModel.PositionZ);
        return new Vector3(gameDataModel.PositionX, gameDataModel.PositionY, gameDataModel.PositionZ);
    }

    public void SetPosition(Vector3 position){
        GameDataModel gameDataModel = GetOrCreateGameData();
        Debug.Log("set position");
        Debug.Log(gameDataModel.PositionX + " " + gameDataModel.PositionY + " " + gameDataModel.PositionZ);
        _realm.Write(() =>
        {
            gameDataModel.PositionX = position.x;
            gameDataModel.PositionY = position.y;
            gameDataModel.PositionZ = position.z;
        });
    }

}
