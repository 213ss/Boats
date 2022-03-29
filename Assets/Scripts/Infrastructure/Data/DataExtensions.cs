using UnityEngine;

namespace Infrastructure.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVector3Data(this Vector3 vector) => 
            new Vector3Data(vector.x, vector.y, vector.z);


        public static string ToJson(this object obj) => JsonUtility.ToJson(obj);
        
        public static T ToDesirialize<T>(this string json) => 
            JsonUtility.FromJson<T>(json);
    }
}