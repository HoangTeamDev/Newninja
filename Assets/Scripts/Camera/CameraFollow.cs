using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraFollow : Singleton<CameraFollow> {
    public GameObject _target;
    public Vector2[] _minBounds;
    public Vector2[] _maxBounds;
    public float _speed;
    public float _smoothTime = 0.3f;
    private Vector3 _velocity = Vector3.zero;
    public float yChar;
    public string minBoundsFileName = "minBounds.json";
    public string maxBoundsFileName = "maxBounds.json";

   
    [ContextMenu("SaveData")]
    public void SevaData()
    {
        SaveBoundsWithIndexToJsonFile(_minBounds, minBoundsFileName);
        SaveBoundsWithIndexToJsonFile(_maxBounds, maxBoundsFileName);
    }
    [ContextMenu("LoadData")]
    public void LoadData()
    {
        LoadBoundsFromJsonFile(minBoundsFileName, ref _minBounds);
        LoadBoundsFromJsonFile(maxBoundsFileName, ref _maxBounds);
    }
    void SaveBoundsWithIndexToJsonFile(Vector2[] boundsArray, string fileName)
    {    
        List<BoundWithIndex> boundsWithIndex = new List<BoundWithIndex>();
        for (int i = 0; i < boundsArray.Length; i++)
        {
            boundsWithIndex.Add(new BoundWithIndex(i, boundsArray[i]));
        }     
        string path = Path.Combine(Application.dataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BoundsWithIndexWrapper existingData = JsonUtility.FromJson<BoundsWithIndexWrapper>(json);

           
            foreach (var newElement in boundsWithIndex)
            {
                bool updated = false;
                for (int j = 0; j < existingData.boundsWithIndex.Count; j++)
                {
                    if (existingData.boundsWithIndex[j].index == newElement.index)
                    {
                       
                        existingData.boundsWithIndex[j].bound = newElement.bound;
                        updated = true;
                        break;
                    }
                }

                
                if (!updated)
                {
                    existingData.boundsWithIndex.Add(newElement);
                }
            }          
            json = JsonUtility.ToJson(existingData, true);
            File.WriteAllText(path, json);
        }
        else
        {          
            string json = JsonUtility.ToJson(new BoundsWithIndexWrapper(boundsWithIndex), true);
            File.WriteAllText(path, json);
        }

        Debug.Log("Saved data to " + path);
    }

    // Hàm đọc dữ liệu từ file JSON và gán vào mảng
    void LoadBoundsFromJsonFile(string fileName, ref Vector2[] boundsArray)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile!=null)
        {
            // Đọc chuỗi JSON từ file
            string json = jsonFile.text;
            Debug.Log(json);

            // Chuyển đổi JSON thành đối tượng BoundsWithIndexWrapper
            BoundsWithIndexWrapper wrapper = JsonUtility.FromJson<BoundsWithIndexWrapper>(json);

            // Gán lại mảng từ wrapper
            List<Vector2> boundsList = new List<Vector2>();
            foreach (var bound in wrapper.boundsWithIndex)
            {
                boundsList.Add(bound.bound);
            }
            boundsArray = boundsList.ToArray();
        }
        else
        {
            Debug.Log("Khong tim thay file");
        }
       
    }
    void Update() {

        if (_target == null) {
            if (GameController.HasInstance)
            {
                if (GameController.Instance._mainCharacter != null)
                {
                    _target = GameController.Instance._mainCharacter;
                }
            }
            
        }

        if (_target != null && _target.activeInHierarchy) {
            Vector3 targetPosition = _target.transform.position;
            targetPosition.x = Mathf.Clamp(targetPosition.x, _minBounds[GameController.Instance._idMap].x , _maxBounds[GameController.Instance._idMap].x );
            targetPosition.y = Mathf.Clamp(targetPosition.y, _minBounds[GameController.Instance._idMap].y, _maxBounds[GameController.Instance._idMap].y);
            targetPosition.z = -10;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y + yChar, targetPosition.z), ref _velocity, _smoothTime);
        }


    }
    public float xDeltaWithScreen;

    float targetWidth = 1100; // Chiều rộng tham chiếu
    float targetHeight = 600; // Chiều cao tham chiếu
    public float pixelsPerUnit = 100f;
   
    //public float tile =1;
    void CheckScreenAspect() {
        //yield return new WaitForSeconds(2f);

        float aspectRatio = (float)Screen.safeArea.width / Screen.safeArea.height;

        float validateAspect = (float)targetWidth / targetHeight;

        float tile = aspectRatio / validateAspect;
        Debug.Log("CheckScreenAspect :" + tile);

        ScreenToZoom();

        if (Camera.main.orthographicSize == 4) {
            xDeltaWithScreen = CalculateXDelta(tile);
        } else {
            xDeltaWithScreen = CalculateXDelta(tile) + .4f;
        }
    }

    public void ScreenToZoom() {

        int size = Screen.width * Screen.height;
        Debug.Log("ScreenToZoom :" + size);
        if (size >= 3500000) {
            Camera.main.orthographicSize = 3.6f;
        } else {
            Camera.main.orthographicSize = 4f;
        }

    }

    float CalculateXDelta(float tile) {
        if (tile < 0.8f)
            return 1.5f;
        if (tile < 0.9f)
            return 10 * (1f - tile) - .4f;
        if (tile < 1f)
            return 10 * (1f - tile) -.2f;
        if (tile == 1f)
            return 0f;
        if (tile > 1)
            return -10 * (tile - 1f) - .4f;
        if (tile > 1.3f)
            return -3f;
        //return -3f;

        return 0f;

    }

}
[System.Serializable]
public class BoundWithIndex
{
    public int index;
    public Vector2 bound;

    public BoundWithIndex(int index, Vector2 bound)
    {
        this.index = index;
        this.bound = bound;
    }
}

// Lớp bao bọc mảng để tiện đọc và ghi vào JSON
[System.Serializable]
public class BoundsWithIndexWrapper
{
    public List<BoundWithIndex> boundsWithIndex;

    public BoundsWithIndexWrapper(List<BoundWithIndex> boundsWithIndex)
    {
        this.boundsWithIndex = boundsWithIndex;
    }
}
