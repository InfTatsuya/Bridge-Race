using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    private Quaternion startRotation;
    private TextMeshPro textMesh;

    private void Awake()
    {
        startRotation = transform.rotation;
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        PlayerRef.onPlayerNameChanged += PlayerRef_onPlayerNameChanged;
    }

    private void PlayerRef_onPlayerNameChanged(object sender, System.EventArgs e)
    {
        textMesh.text = PlayerRef.PlayerName;
    }

    private void FixedUpdate()
    {
        transform.rotation = startRotation;
    }
}
