using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using System;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using System.Text;


public class Database : MonoBehaviour
{

    private static string username = "dbadmin";
    private static string password = "ssqV5I33EARa2qBR";
    private static string _realmAppId = "prototype-yozkn";
    private static string MONGO_URI = "mongodb+srv://"+username+":"+password+"@cluster0.xm3l59x.mongodb.net/?retryWrites=true&w=majority";
    private static string DATABASE_NAME = "491B";
    private static MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<GameDataModel> userCollection;
    private GameDataModel user;

    static public Database Instance;

    public bool isLoggedIn = false;


    async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private async void Start()
    {
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        userCollection = db.GetCollection<GameDataModel>("user_accounts");
        // GameDataModel e = new GameDataModel();
        // // e.name = "hope";
        // // userCollection.InsertOne(e);
        // List<GameDataModel> userModelList = userCollection.Find(user => true).ToList();
        // GameDataModel[] userAsap= userModelList.ToArray();
        // foreach(GameDataModel asap in userAsap)
        // {
        //     Debug.Log(asap.name);
        // }
        // signup("test@test.com","test","test");
        // login("test@test.com","test");
        // setPosition(new Vector3(1,2,3));
        // Debug.Log(getPosition());

        
    }

    
    public bool signup(string email, string password, string username)
    {
        // check if email is already in use
        IMongoCollection<GameDataModel> userCollection = db.GetCollection<GameDataModel>("user_accounts");
        var query = userCollection.Find(x => x.Email == email).FirstOrDefault();
        password = HashPassword(password);
        if (query != null)
        {
            Debug.Log("Email already in use");
            return false;
        }
        else{
            GameDataModel e = new GameDataModel();
            e.Email = email;
            e.Password = password;
            e.UserId = username;
            e.Name = username;
            userCollection.InsertOne(e);
            isLoggedIn = true;
            return true;
        }
    }

    public bool login(string email, string password)
    {
        // IMongoCollection<GameDataModel> userCollection = db.GetCollection<GameDataModel>("user_accounts");
        var query = userCollection.Find(x => x.Email == email).FirstOrDefault();
        password = HashPassword(password);

        if (query != null)
        {
            if (query.Password == password )
            {
                Debug.Log("Login Successful");
                user = query;
                isLoggedIn = true;
                return true;
            }
            else
            {
                Debug.Log("Incorrect Password");
                return false;
            }
        }
        else
        {
            Debug.Log("Email not found");
            return false;
        }
    }

    public Vector3 getPosition()
    {
        var query = userCollection.Find(x => x.Email == user.Email).FirstOrDefault();
        if (query != null)
        {
            return new Vector3(query.PositionX, query.PositionY, query.PositionZ);
        }
        else
        {
            Debug.Log("Email not found");
            return new Vector3(0,0,0);
        }
        
    }

    public async void setPosition(Vector3 position)
    {
        var query = userCollection.Find(x => x.Email == user.Email).FirstOrDefault();
        if (query != null)
        {
            var update = Builders<GameDataModel>.Update.Set("PositionX", position.x).Set("PositionY", position.y).Set("PositionZ", position.z);
            userCollection.UpdateOne(x => x.Email == user.Email, update);
        }
        else
        {
            Debug.Log("Email not found");
        }
    }

    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the password string to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hashed bytes to a hexadecimal string
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}