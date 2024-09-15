using System.Runtime.Serialization;

namespace Nova_Engine.Models;

[DataContract]
public class Vector2 (float x, float y)
{
    [DataMember]
    public float X { get; set; } = x;
    
    [DataMember]
    public float Y { get; set; } = y;

}