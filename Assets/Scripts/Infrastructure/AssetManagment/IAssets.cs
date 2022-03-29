using UnityEngine;

namespace Infrastructure.AssetManagment
{
    public interface IAssets
    {
        GameObject Instantiate(string filePath);
        GameObject Instantiate(string filePath, Vector3 at);
    }
}