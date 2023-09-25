using Realms;
using MongoDB.Bson;
using System.Numerics;

public class GameDataModel : RealmObject
{

    [PrimaryKey]
    [MapTo("_id")]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [Required]
    [MapTo("user_id")]
    public string UserId { get; set; } = "gamertag";

    [MapTo("PositionX")]
    public float PositionX { get; set; } = 0;

    [MapTo("PositionY")]
    public float PositionY { get; set; } = 0;
    [MapTo("PositionZ")]
    public float PositionZ { get; set; } = 0;

    [MapTo("name")]
    public string Name { get; set; }

    public Vector3 GetPosition()
    {
        return new Vector3(PositionX, PositionY, PositionZ);
    }

}
