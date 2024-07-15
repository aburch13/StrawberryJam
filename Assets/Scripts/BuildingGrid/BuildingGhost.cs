using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    // Fields
    private Transform visual;
    private BuildingData data;

    // Start is called before the first frame update
    void Start()
    {
        RefreshVisual();

        BuildingGrid.Instance.OnSelectionChange += OnSelectionChange;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = BuildingGrid.Instance.CurrentCellWorldPos;
    }

    private void OnSelectionChange(object sender, EventArgs e)
    {
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        data = BuildingGrid.Instance.CurrentSelection;

        if (data != null)
        {
            visual = Instantiate(data.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
        }
    }
}
