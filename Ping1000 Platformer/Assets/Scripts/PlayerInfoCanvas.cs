using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoCanvas : MonoBehaviour
{
    [Tooltip("The parent transform of the health unit icons.")]
    public Transform healthUnitParent;
    public GameObject healthUnitPrefab;
    public Transform ammoUnitParent;
    public GameObject ammoUnitPrefab;

    Stack<GameObject> healthUnits;
    Stack<GameObject> ammoUnits;

    public static PlayerInfoCanvas instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        healthUnits = new Stack<GameObject>();
        ammoUnits = new Stack<GameObject>();

        EricCharacterMovement player = FindObjectOfType<EricCharacterMovement>();
        if (player != null) {
            RegenGun playerGun = (RegenGun)player._gun;
            for (int i = 0; i < playerGun.maxAmmo; i++) {
                AddAmmo();
            }
            for (int i = 0; i < player.lives; i++) {
                AddHealthUnit();
            }
        }
    }

    public static void AddHealthUnit(int amount = 1) {
        for (int i = 0; i < amount; i++) {
            GameObject newUnit = Instantiate(Resources.Load<GameObject>("Prefabs/Health Unit"),
                instance.healthUnitParent);
            instance.healthUnits.Push(newUnit);
        }
    }

    public static void RemoveHealthUnit(int amount = 1) {
        for (int i = 0; i < amount; i++) {
            Destroy(instance.healthUnits.Pop());
        }
    }

    public static void AddAmmo(int amount = 1) {
        for (int i = 0; i < amount; i++) {
            GameObject newUnit = Instantiate(Resources.Load<GameObject>("Prefabs/Ammo Unit"),
                instance.ammoUnitParent);
            instance.ammoUnits.Push(newUnit);
        }
    }

    public static void RemoveAmmo(int amount = 1) {
        for (int i = 0; i < amount; i++) {
            Destroy(instance.ammoUnits.Pop());
        }
    }
}
