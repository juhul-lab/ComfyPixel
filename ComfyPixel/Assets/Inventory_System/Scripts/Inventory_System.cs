﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory_System : MonoBehaviour {

    private Utility utility;
    private PlayerController playerController;

    private Image inv_Holder = null;

    public bool Inventory_System_bool = true;

    private void spawn_inv_menu()
    {
        playerController.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        playerController.canMove = false;

        inv_Holder = (Image)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 1, new string[] { "inventory_holder" }, new Vector2[] { new Vector2(1800f, 2000f) },
                                                           new Vector3[] { new Vector3(-556f, -1.2398e-05f) }, new Vector3[] { Vector3.zero });
        inv_Holder.sprite = Resources.Load<Sprite>("Settings_System_Graphics/Settings_Holder");
    }

    public void close_inv_menu()
    {
        try
        {
            Destroy(GameObject.Find(inv_Holder.name));
        }
        catch
        {

        }
        inv_Holder = null;
        playerController.canMove = true;
        utility.to_change = -1;
        FindObjectOfType<NPC_Dialogue_System>().NPC_Dialogue_System_bool = true;
    }

    private void use_inv_men()
    {
        if (Inventory_System_bool)
        {
            if (Input.GetKeyDown(KeyCode.D) && inv_Holder == null)
            {
                spawn_inv_menu();
            }
            else if (Input.GetKeyDown(KeyCode.F) && inv_Holder != null)
            {
                close_inv_menu();
            }
        }

        if (inv_Holder != null)
        {
            FindObjectOfType<NPC_Dialogue_System>().NPC_Dialogue_System_bool = false;
        }
    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        utility = FindObjectOfType<Utility>();
    }

    private void Update()
    {
        use_inv_men();
    }
}