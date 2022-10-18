using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour {

    [SerializeField] private Transform pfCan1;
    [SerializeField] private Transform pfCan2;
    [SerializeField] private Transform pfCan3;
    [SerializeField] private Transform canSpawnPosition;

    [SerializeField] private ButtonWorldUI buttonCan1;
    [SerializeField] private ButtonWorldUI buttonCan2;
    [SerializeField] private ButtonWorldUI buttonCan3;

    private void Awake() {
        buttonCan1.GetComponent<Image>().color = Color.red;
        buttonCan2.GetComponent<Image>().color = Color.blue;
        buttonCan3.GetComponent<Image>().color = Color.green;

        buttonCan1.OnPointerExit += (object sender, EventArgs e) => { buttonCan1.GetComponent<Image>().color = Color.red; };
        buttonCan1.OnPointerEnter += (object sender, EventArgs e) => { buttonCan1.GetComponent<Image>().color = Color.white; };
        buttonCan1.OnPointerDown += (object sender, EventArgs e) => { SpawnCan(pfCan1); };

        buttonCan2.OnPointerExit += (object sender, EventArgs e) => { buttonCan2.GetComponent<Image>().color = Color.red; };
        buttonCan2.OnPointerEnter += (object sender, EventArgs e) => { buttonCan2.GetComponent<Image>().color = Color.white; };
        buttonCan2.OnPointerDown += (object sender, EventArgs e) => { SpawnCan(pfCan2); };

        buttonCan3.OnPointerExit += (object sender, EventArgs e) => { buttonCan3.GetComponent<Image>().color = Color.red; };
        buttonCan3.OnPointerEnter += (object sender, EventArgs e) => { buttonCan3.GetComponent<Image>().color = Color.white; };
        buttonCan3.OnPointerDown += (object sender, EventArgs e) => { SpawnCan(pfCan3); };
    }

    private void SpawnCan(Transform prefab) {
        Transform canTransform = Instantiate(prefab, canSpawnPosition.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(70f, 110f)));
        canTransform.GetComponent<Rigidbody>().velocity = (Quaternion.Euler(UnityEngine.Random.Range(-5f, +5f), UnityEngine.Random.Range(-5f, +5f), UnityEngine.Random.Range(-5f, +5f)) * canSpawnPosition.forward) * UnityEngine.Random.Range(3f, +7f);
        canTransform.GetComponent<BoxCollider>().isTrigger = true;
        //FunctionTimer.Create(() => { canTransform.GetComponent<BoxCollider>().isTrigger = false; }, .05f);
    }

}
