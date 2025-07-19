using System.Collections.Generic;

public interface IJsonStorage
{
    void SaveToJson();
    List<object> LoadFromJson();
    void InitializeSampleData();
}