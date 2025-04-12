using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UiAssetLoader : MonoBehaviour
{
    public string spriteName;
    public string bundleName;

    void Start()
    {
        //AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        //Sprite sprite = localAssetBundle.LoadAsset<Sprite>(spriteName);
        //GetComponent<Image>().sprite = sprite;

        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return;
        }

        GetComponent<Image>().sprite = localAssetBundle.LoadAsset<Sprite>(spriteName);
        localAssetBundle.Unload(false);
    }
}