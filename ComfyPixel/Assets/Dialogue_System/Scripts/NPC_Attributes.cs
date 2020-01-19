﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPC_Attributes : MonoBehaviour {

    //NPC attributes, more will be added 
    public TextAsset Dialogue;
    public Sprite[] Sprites;

    //variabled for NPC's to handle dialogue
    private Image dialogue_box;
    private Image sprite_box;
    public Image choose_button;
    public Sprite chosen_sprite;
    public Sprite not_chosen_sprite;
    public int stop_scroll_line;
    public bool oneTime = true;
    private List<GameObject> buttons = new List<GameObject>();

    private PlayerController playerController;
    private Utility utility;
    private Dialogue_System dialogue_System;


    private void NPC_dialogue_choosing(string[] button_texts, int[] new_dialogue_attr_1, int[] new_dialogue_attr_2)
    {
        if (oneTime)
        {
            //instantiating buttons, always 2 since this is part of a NPC's dialogue (would get too complicated storywise)
            buttons = utility.spawn_Buttons(choose_button, 1,
                                            new Vector3[] { new Vector3(dialogue_box.rectTransform.anchoredPosition.x - 100f, dialogue_box.rectTransform.anchoredPosition.y),
                                                                new Vector3(dialogue_box.rectTransform.anchoredPosition.x + 100f, dialogue_box.rectTransform.anchoredPosition.y) },
                                            new Quaternion[] { new Quaternion(), new Quaternion() },
                                            new string[] { button_texts[0], button_texts[1] });

            //spawn buttons once and stop dialogue from scrolling
            oneTime = false;
            tag = "NPC_nottalk";
        }
        else if (!oneTime)
        {
            //depending on options chosen:
            if (Input.GetKeyDown(KeyCode.Return) && buttons[0].GetComponent<Image>().sprite == chosen_sprite)
            {
                //delete buttons and continue dialogue on line according to button chosen (also reset certain dialogue_system-attributes)
                foreach (GameObject i in buttons.ToArray())
                {
                    GameObject reference = buttons[System.Array.IndexOf(buttons.ToArray(), i)];
                    buttons.Remove(reference);
                    Destroy(reference);
                }
                stop_scroll_line = new_dialogue_attr_1[0];
                utility.current_line = new_dialogue_attr_1[1];
                utility.letter = 0;
                tag = "NPC_talkable";
                oneTime = true;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && buttons[1].GetComponent<Image>().sprite == chosen_sprite)
            {
                foreach (GameObject i in buttons.ToArray())
                {
                    GameObject reference = buttons[System.Array.IndexOf(buttons.ToArray(), i)];
                    buttons.Remove(reference);
                    Destroy(reference);
                }
                stop_scroll_line = new_dialogue_attr_2[0];
                utility.current_line = new_dialogue_attr_2[1];
                utility.letter = 0;
                tag = "NPC_talkable";
                oneTime = true;
            }
        }
        
        //give ability to choose between buttons, always vertically because this is part of the dialogue_system
        utility.choose_buttons(buttons.ToArray(), chosen_sprite, not_chosen_sprite, 1, "ver");
    }

    private void Update()
    {
        //keep adding changing sprites/other special events in this switch statement for the npc's that need it
        if(playerController.NPC_name == name)
        {
            switch (playerController.NPC_name)
            {
                //just for testing purposes
                case "NPC":
                    switch (utility.current_line)
                    {
                        case 0:
                            sprite_box.sprite = Sprites[0];
                            break;
                        case 1:
                            sprite_box.sprite = Sprites[1];
                            break;
                        case 3:
                            NPC_dialogue_choosing(new string[] { "option1", "option2" },
                                                  new int[] { 9, 4}, new int[] { 18, 16});
                            break;
                        case 8:
                            NPC_dialogue_choosing(new string[] { "option1", "option2" },
                                                  new int[] { 12, 9}, new int[] { 16, 12});
                            break;
                    }
                    break;
            }
        }
    }

    private void Awake()
    {
        dialogue_box = GameObject.Find("dialogue_box").GetComponent<Image>();
        sprite_box = GameObject.Find("sprite_box").GetComponent<Image>();

        utility = FindObjectOfType<Utility>();
        playerController = FindObjectOfType<PlayerController>();
    }
}