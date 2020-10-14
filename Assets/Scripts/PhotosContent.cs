using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosContent : MonoBehaviour
{
    // The photo button prefab
    public GameObject Prefab;

    // The rect transform of this photos container
    private RectTransform RectTransform;

    // Auto-layout script for the photo buttons
    private GridLayoutGroup GridLayoutGroup;

    // Keep list of all instantiated photo buttons
    private List<GameObject> InstantiatedPrefabs;

    // The photo detail component of the photo detail panel
    private PhotoDetail PhotoDetail;

    // Delegate to open the photo detail from menu manager
    [HideInInspector]
    public delegate void PhotoDetailDelegate();
    private PhotoDetailDelegate OpenPhotoDetailDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign photo detail component from menu manager
    public void SetupPhotoDetail(PhotoDetail photoDetail)
    {
        this.PhotoDetail = photoDetail;
    }

    // Assign open photo detail delegate from menu manager
    public void SetupOpenPhotoDetailDelegate(PhotoDetailDelegate callback)
    {
        this.OpenPhotoDetailDelegate = callback;
    }

}
