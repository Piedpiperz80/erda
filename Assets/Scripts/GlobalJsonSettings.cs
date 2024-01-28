using Newtonsoft.Json;
using UnityEngine;

public class GlobalJsonSettings : MonoBehaviour
{
    void Awake()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }
}
