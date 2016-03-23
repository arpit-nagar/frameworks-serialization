namespace Tavisca.Frameworks.Serialization.Configuration
{
    public enum SerializerType
    {
        RootDefaultOrNone = 0,
        Binary = 1,
        ProtoBuf = 2,
        XmlSerializer = 3,
        DataContractSerializer = 4,
        DataContractJsonSerializer = 5,
        NewtonsoftJsonNetSerializer = 6
    }
}
