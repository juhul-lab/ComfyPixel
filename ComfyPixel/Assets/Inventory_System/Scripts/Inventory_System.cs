﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory_System : MonoBehaviour {

    private Utility utility;
    private PlayerController playerController;

    //general interfaces for inv_system
    private Image inv_Holder = null;
    private List<Image> inv_options_holder = null;

    //individual stuff for general interfaces
    private List<Image> items_places = new List<Image>(12);
    public List<Image> items_places_sprites = new List<Image>(12);
    private List<Text> stats_sprites_texts = new List<Text>();
    private List<Image> eqp_slots = new List<Image>();
    private List<Image> eqp_slots_sprites = new List<Image>();
    public Sprite[] remember_sprites = new Sprite[12];
    private Sprite[] remember_eqp_sprites = new Sprite[4];
    private bool inventory_bool;
    private bool once = true;
    private Image test;
    public bool stats_bool;
    public enum item_type{permament, temporary};
    [SerializeField] private string current_item_text;
    
    public bool Inventory_System_bool = true;

    private void spawn_inv_menu()
    {
        playerController.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        playerController.canMove = false;

        //creating main-window for inventory
        inv_Holder = (Image)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 1, new string[] { "inventory_holder" }, new Vector2[] { new Vector2(1800f, 2000f) },
                                                           new Vector3[] { new Vector3(-556f, -1.2398e-05f) }, new Vector3[] { Vector3.zero });
        inv_Holder.sprite = Resources.Load<Sprite>("Settings_System_Graphics/Settings_Holder");


        //different windows you can choose between for inv_options
        inv_options_holder = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 3, new string[] { "stats_holder", "attacks_holder", "items_holder"}, 
                                                                   new Vector2[] { new Vector2(400f, 400f), new Vector2(600f, 400f), new Vector2(1067.3f, 800f)},
                                                                   new Vector3[] { new Vector3(-751f, -330f), new Vector3(-499f, -330f), new Vector3(-597.4f, -8)}, 
                                                                   new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, });

        //sprites in stats_holder-window
        List<Image> stats_sprites = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 2, new string[] { "att_stat", "def_stat" },
                                                                          new Vector2[] { new Vector2(200f, 200f), new Vector2(200f, 200f)},
                                                                          new Vector3[] { new Vector3(-791.2f, -294.1f), new Vector3(-791.2f, -377.2f) },
                                                                          new Vector3[] { Vector3.zero, Vector3.zero});
        foreach(Image i in stats_sprites)
        {
            if(stats_sprites.IndexOf(i) == 0)
            {
                i.sprite = Resources.Load<Sprite>("Inventory_System_Graphics/Damage_Stat");
            }
            else if(stats_sprites.IndexOf(i) == 1)
            {
                i.sprite = Resources.Load<Sprite>("Inventory_System_Graphics/Defense_Stat");
            }

            i.rectTransform.SetParent(inv_options_holder[0].rectTransform);
        }

        eqp_slots = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 4, new string[] { "player_hat", "player__weapon", "player_barmor", "player_shoes" },
                                                                          new Vector2[] { new Vector2(300f, 300f), new Vector2(300f, 300f), new Vector2(300f, 300f), new Vector2(300f, 300f) },
                                                                          new Vector3[] { new Vector3(-781f, 324.3f), new Vector3(-657f, 211.7f), new Vector3(-534f, 324.3f), new Vector3(-407f, 211.7f) },
                                                                          new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero });

        eqp_slots_sprites = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 4, new string[] { "player_hat_sprite", "player__weapon_sprite", "player_barmor_sprite", "player_shoes_sprite" },
                                                                          new Vector2[] { new Vector2(84.243f, 84.67f), new Vector2(84.243f, 84.67f), new Vector2(84.243f, 84.67f), new Vector2(84.243f, 84.67f) },
                                                                          new Vector3[] { new Vector3(-781f, 324.3f), new Vector3(-657f, 211.7f), new Vector3(-534f, 324.3f), new Vector3(-407f, 211.7f) },
                                                                          new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero });
        foreach(Image i in eqp_slots_sprites)
        {
            i.sprite = remember_eqp_sprites[eqp_slots_sprites.IndexOf(i)];
            i.rectTransform.SetParent(eqp_slots[eqp_slots_sprites.IndexOf(i)].rectTransform);
            switch (eqp_slots_sprites.IndexOf(i))
            {
                case 0:
                    i.rectTransform.localPosition = new Vector3(-9.1f, 9.3f);
                    break;
                case 1:
                    i.rectTransform.localPosition = new Vector3(-9.1f, 9.2f);
                    break;
                case 2:
                    i.rectTransform.localPosition = new Vector3(-8.9f, 8.9f);
                    break;
                case 3:
                    i.rectTransform.localPosition = new Vector3(-9f, 9.1f);
                    break;
            }
        }

        foreach (Image i in eqp_slots)
        {
            i.sprite = Resources.Load<Sprite>("Inventory_System_Graphics/eqp_not_chosen");
            i.rectTransform.SetParent(inv_Holder.rectTransform);
        }

        //texts for sprites
        stats_sprites_texts = (List<Text>)utility.create_ui_object(new GameObject().AddComponent<Text>(), new System.Type[] { typeof(Text) }, 2, new string[] { "att_stat_text", "def_stat_text" },
                                                                              new Vector2[] { new Vector2(70f, 70f), new Vector2(70f, 70f) },
                                                                              new Vector3[] { new Vector3(-719.5f, -296.9f), new Vector3(-719.5f, -377.2f) },
                                                                              new Vector3[] { Vector3.zero, Vector3.zero });

        foreach(Text i in stats_sprites_texts)
        {
            i.font = Resources.Load<Font>("Dialogue_System_Graphics/dpcomic");
            i.fontSize = 40;
            i.color = Color.black;
            i.alignment = TextAnchor.MiddleCenter;
            if(stats_sprites_texts.IndexOf(i) == 0)
            {
                i.rectTransform.SetParent(stats_sprites[0].rectTransform);
                i.text = (playerController.player_stats[0] + playerController.added_player_stats[0]).ToString();
            }
            else if(stats_sprites_texts.IndexOf(i) == 1)
            {
                i.rectTransform.SetParent(stats_sprites[1].rectTransform);
                i.text = (playerController.player_stats[1] + playerController.added_player_stats[1]).ToString();
            }
        }

        foreach (Image i in inv_options_holder)
        {
            i.sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen");
            i.rectTransform.SetParent(inv_Holder.rectTransform);
        }

        items_places = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 12, new string[] { "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", "items_place", },
                                                                              new Vector2[] { new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f), new Vector2(200f, 200f),  new Vector2(200f, 200f) },
                                                                              new Vector3[] { new Vector3(-757f, 87f), new Vector3(-757f+104f, 87f), new Vector3(-757f + 104f*2, 87f), new Vector3(-757f + 104f*3, 87f), new Vector3(-757f, 87f-106f), new Vector3(-757f + 104f, 87f-106f), new Vector3(-757f + 104f * 2, 87f-106f), new Vector3(-757f + 104f * 3, 87f-106), new Vector3(-757f, 87f-106*2), new Vector3(-757f+104f, 87f-106*2), new Vector3(-757f + 104f*2, 87f-106*2), new Vector3(-757f + 104f*3, 87f-106*2),},
                                                                              new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, });
        foreach (Image i in items_places)
        {
            i.sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen");
            i.rectTransform.SetParent(inv_options_holder[2].rectTransform);
        }

        items_places_sprites = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 12, new string[] { "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite", "items_place_sprite" },
                                                                     new Vector2[] { new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f), new Vector2(82.12f, 82.142f) },
                                                                     new Vector3[] { new Vector3(-757.9969f, 83.97806f), new Vector3(-757.9969f+ 103.9969f, 83.97806f), new Vector3(-757.9969f+103.9969f*2, 83.97806f), new Vector3(-757.9969f+103.9969f*3, 83.97806f), new Vector3(-757.9969f, 83.97806f-106.07806f), new Vector3(-757.9969f + 103.9969f, 83.97806f - 106.07806f), new Vector3(-757.9969f + 103.9969f * 2, 83.97806f - 106.07806f), new Vector3(-757.9969f + 103.9969f * 3, 83.97806f - 106.07806f), new Vector3(-757.9969f, 83.97806f - 106.07806f*2), new Vector3(-757.9969f + 103.9969f, 83.97806f - 106.07806f*2), new Vector3(-757.9969f + 103.9969f * 2, 83.97806f - 106.07806f*2), new Vector3(-757.9969f + 103.9969f * 3, 83.97806f - 106.07806f*2), },
                                                                     new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero });

        foreach (Image i in items_places_sprites)
        {
            i.rectTransform.SetParent(items_places[items_places_sprites.IndexOf(i)].rectTransform);
            i.sprite = remember_sprites[items_places_sprites.IndexOf(i)];
        }

        inv_Holder.rectTransform.position += new Vector3(100f, 0f, 0f);
        inv_options_holder.AddRange(eqp_slots);
    }

    public void add_normal_item(Sprite item_sprite)
    {
        for(int x=0; x < remember_sprites.Length+1; x++)
        {
            if (remember_sprites[x] == null)
            {
                remember_sprites[x] = item_sprite;
                try
                {
                    items_places_sprites[x].sprite = item_sprite;
                }
                catch
                {

                }
                break;
            }            
        }
    }

    public void close_inv_menu()
    {
        //catching error when function called when inv_holder not existing in scene
        try
        {
            Destroy(GameObject.Find(inv_Holder.name));
        }
        catch
        {

        }
        once = true;
        inventory_bool = false;
        inv_Holder = null;
        playerController.canMove = true;
        utility.to_change = -1; //to choose between different option-systems
        FindObjectOfType<NPC_Dialogue_System>().NPC_Dialogue_System_bool = true;
    }

    private void use_inv_men()
    {
        if (Inventory_System_bool)
        {
            //closing and starting inv_system
            if (Input.GetKeyDown(KeyCode.D) && inv_Holder == null)
            {
                spawn_inv_menu();
            }
            else if (Input.GetKeyDown(KeyCode.F) && inv_Holder != null && !inventory_bool && !stats_bool && GameObject.Find("item_options") == null)
            {
                
                close_inv_menu();
            }
        }

        if (inv_Holder != null && GameObject.Find("cover_game") == null)
        {
            foreach(Text i in stats_sprites_texts)
            {
                if (stats_sprites_texts.IndexOf(i) == 0)
                {
                    i.text = (playerController.player_stats[0] + playerController.added_player_stats[0]).ToString();
                }
                else if (stats_sprites_texts.IndexOf(i) == 1)
                {
                    i.text = (playerController.player_stats[1] + playerController.added_player_stats[1]).ToString();
                }
            }

            FindObjectOfType<NPC_Dialogue_System>().NPC_Dialogue_System_bool = false;

            //choosing between inv_options
            if (!inventory_bool && !stats_bool && GameObject.Find("item_options") == null)
            {
                utility.choose_buttons(inv_options_holder.ToArray(), new Sprite[] { Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"), },
                       new Sprite[] { Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_not_chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_not_chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_not_chosen"), Resources.Load<Sprite>("Inventory_System_Graphics/eqp_not_chosen"), },
                       6, "ver");


                if((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    if (test != null)
                    {
                        GameObject.Find(test.name).GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().rectTransform.localPosition += new Vector3(0f, 6f);
                    }

                    foreach (Image i in inv_options_holder)
                    {
                        if (i.sprite == Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"))
                        {
                            test = i;
                            i.rectTransform.GetChild(0).GetComponent<Image>().rectTransform.localPosition -= new Vector3(0f, 6);
                        }
                    }

                    if (inv_options_holder[2].sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                    {
                        test = null;
                    }
                }
            }

            //moving specific elements in windows when chosen/not chosen
            if(inv_options_holder[0].sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
            {
                GameObject.Find("att_stat").GetComponent<Image>().rectTransform.localPosition = new Vector3(-40.20007f, 22.49997f);
                GameObject.Find("def_stat").GetComponent<Image>().rectTransform.localPosition = new Vector3(-40.20007f, -60.6f);

                Image text_inv_holder = null;
                Text stats_text = null;

                if (Input.GetKeyDown(KeyCode.D) && !stats_bool)
                {
                    utility.can_scroll = true;
                    utility.letter = 0;
                    utility.current_line = 0;
                    stats_bool = true;
                    text_inv_holder = (Image)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 1, new string[] { "stats_text_holder" }, new Vector2[] { new Vector2(830f, 650f) },
                                                           new Vector3[] { new Vector3(41f, -351.7f) }, new Vector3[] { Vector3.zero });
                    text_inv_holder.sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen");
                    stats_text = (Text)utility.create_ui_object(new GameObject().AddComponent<Text>(), new System.Type[] { typeof(Text) }, 1, new string[] { "stats_text" }, new Vector2[] { new Vector2(325f, 230f) },
                                                           new Vector3[] { new Vector3(41f, -360.9f) }, new Vector3[] { Vector3.zero });
                    stats_text.rectTransform.SetParent(text_inv_holder.rectTransform);
                    stats_text.fontSize = 45;
                    stats_text.font = Resources.Load<Font>("Dialogue_System_Graphics/dpcomic");
                    stats_text.color = Color.black;
                }

                if (stats_bool)
                {
                    if (Input.GetKeyDown(KeyCode.E) && GameObject.Find("stats_text").GetComponent<Text>().text == ("You have " + playerController.player_stats[0] + " Attack points (+" + playerController.added_player_stats[0] + ") and " + playerController.player_stats[1] + " Defense points (+" + playerController.added_player_stats[1] + ")."))
                    {
                        Destroy(GameObject.Find("stats_text_holder"));
                        stats_bool = false;
                        utility.can_scroll = true;
                        utility.letter = 0;
                        utility.current_line = 0;
                    }
                    utility.run_text(new List<string> { "You have " + playerController.player_stats[0] + " Attack points (+" + playerController.added_player_stats[0] + ") and "  + playerController.player_stats[1] + " Defense points (+" + playerController.added_player_stats[1] + ")." }, GameObject.Find("stats_text").GetComponent<Text>(), 0.03f);
                }
            }
            else if(inv_options_holder[1].sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
            {
                if (!once)
                {
                    foreach (Image i in items_places)
                    {
                        i.rectTransform.localPosition += new Vector3(0, 22f, 0);
                    }
                    once = true;
                }

                GameObject.Find("att_stat").GetComponent<Image>().rectTransform.localPosition = new Vector3(-40.20007f, 35.89996f);
                GameObject.Find("def_stat").GetComponent<Image>().rectTransform.localPosition = new Vector3(-40.20007f, -47.20001f);
            }
            else if(inv_options_holder[2].sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
            {
                Image item_options = null;
                bool test = true;

                if (once)
                {
                    foreach (Image i in items_places)
                    {
                        i.rectTransform.localPosition += new Vector3(0, -22f, 0);
                    }
                    once = false;
                }

                if (Input.GetKeyDown(KeyCode.D) && !inventory_bool)
                {
                    items_places[0].sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen");
                    items_places_sprites[0].rectTransform.localPosition -= new Vector3(0, 5.878058f, 0);
                    items_places_sprites[0].tag = "inv_item_chosen";
                    utility.to_change = 0;
                    test = false;
                }

                foreach(Image i in items_places)
                {
                    if(i.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                    {
                        inventory_bool = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.F) && GameObject.Find("item_options") == null)
                {
                    utility.to_change = 2;
                    inventory_bool = false;
                    foreach (Image i in items_places)
                    {
                        if (i.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                        {
                            i.sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen");
                            i.rectTransform.GetChild(0).localPosition += new Vector3(0, 5.878058f, 0);
                        }
                    }
                }

                if (inventory_bool)
                {
                    if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        foreach(Image i in items_places)
                        {
                            if (i.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                            {
                                i.rectTransform.GetChild(0).tag = "prev_inv_item_chosen";
                            }
                        }
                    }

                    if(GameObject.Find("item_options") == null)
                    {
                        utility.choose_buttons(items_places.ToArray(), new Sprite[] { Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen") },
                                     new Sprite[] { Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen"), Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen") },
                                     11, "ver");
                    }

                    foreach(Image i in items_places_sprites)
                    {
                        if (i.rectTransform.parent.GetComponent<Image>().sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                        {
                            if (Input.GetMouseButton(0))
                            {
                                //calculate distance of every ui-object to mouse-position, only able to move item_place_sprites&&certain near distance, if mouse-click released return to certain near distance item_sprites<->if not in distance, return to og item_sprite 
                                i.rectTransform.position = Input.mousePosition;
                            }
                        }
                        if(i.tag == "prev_inv_item_chosen")
                        {
                            foreach(Image o in items_places)
                            {
                                if(o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                {
                                    o.tag = "inv_item_chosen";
                                    o.rectTransform.GetChild(0).localPosition -= new Vector3(0, 5.878058f, 0);
                                }
                            }

                            i.rectTransform.localPosition += new Vector3(0, 5.878058f, 0);
                            i.tag = "Untagged";
                        }
                        if(i.rectTransform.parent.GetComponent<Image>().sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen") && i.sprite != null &&  Input.GetKeyDown(KeyCode.D) && test)
                        {
                            if(GameObject.Find("item_options") == null)
                            {
                                utility.multiple_to_change.Add(utility.to_change);
                                utility.to_change = -1;
                                item_options = (Image)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 1, new string[] { "item_options" }, new Vector2[] { new Vector2(830f, 650f) },
                                               new Vector3[] { new Vector3(41f, -351.7f) }, new Vector3[] { Vector3.zero });
                                item_options.sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen");
                                item_options.rectTransform.SetParent(GameObject.Find("inventory_holder").transform);
                            }
                        }
                    }
                }

                if(GameObject.Find("item_options") != null)
                {
                    Image options = GameObject.Find("item_options").GetComponent<Image>();
                    List<Image> options_buttons = null;

                    if(GameObject.Find("options1") != null)
                    {
                        utility.choose_buttons(new Image[] { GameObject.Find("options1").GetComponent<Image>(), GameObject.Find("options2").GetComponent<Image>() }, new Sprite[] { Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"), Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen") },
                                                  new Sprite[] { Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Not_Chosen"), Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Not_Chosen") }, 1, "ver");
                    }

                    if(GameObject.Find("options_text") == null)
                    {
                        Text options_text = (Text)utility.create_ui_object(new GameObject().AddComponent<Text>(), new System.Type[] { typeof(Text) }, 1, new string[] { "options_text" }, new Vector2[] { new Vector2(325f, 147.64f) },
                                                                           new Vector3[] { new Vector3(35f, -318f) }, new Vector3[] { Vector3.zero });
                        options_text.rectTransform.SetParent(options.rectTransform);
                        options_text.fontSize = 35;
                        options_text.font = Resources.Load<Font>("Dialogue_System_Graphics/dpcomic");
                        options_text.color = Color.black;

                        options_buttons = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 2, new string[] { "options1", "options2" }, new Vector2[] { new Vector2(1000f, 1000f), new Vector2(1000, 1000) },
                                                                                new Vector3[] { new Vector3(-11f, -400f), new Vector3(156f, -400f) }, new Vector3[] { Vector3.zero, Vector3.zero });
                        foreach(Image i in items_places)
                        {
                            if(i.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                            {
                                switch (i.rectTransform.GetChild(0).GetComponent<Image>().sprite.name)
                                {
                                    case "Test_Item":
                                        options_text.text = "This Item gives +1 Attack when used.";
                                        break;
                                    case "Test_Item_head":
                                        options_text.text = "This item gives +1 Attack when equipped";
                                        break;
                                    case "eqp_chosen":
                                        options_text.text = "This item will do nothing, but it can be equipped.";
                                        break;
                                }
                            }
                        }

                        if(GameObject.Find("options2_text") == null)
                        {
                            List<Text> options_buttons_texts = (List<Text>)utility.create_ui_object(new GameObject().AddComponent<Text>(), new System.Type[] { typeof(Text) }, 2, new string[] { "options1_text", "options2_text" }, new Vector2[] { new Vector2(89.83f, 50.1f), new Vector2(89.83f, 50.1f) },
                                                                new Vector3[] { new Vector3(-46f, -425.1703f), new Vector3(121f, -425.1703f) }, new Vector3[] { Vector3.zero, Vector3.zero });
                            foreach (Text i in options_buttons_texts)
                            {
                                if (options_buttons_texts.IndexOf(i) == 0)
                                {
                                    i.text = "USE";
                                    i.rectTransform.SetParent(GameObject.Find("options1").GetComponent<Image>().rectTransform);
                                }
                                else if (options_buttons_texts.IndexOf(i) == 1)
                                {
                                    i.text = "TRASH";
                                    i.rectTransform.SetParent(GameObject.Find("options2").GetComponent<Image>().rectTransform);
                                }
                                i.alignment = TextAnchor.MiddleCenter;
                                i.fontSize = 25;
                                i.font = Resources.Load<Font>("Dialogue_System_Graphics/dpcomic");
                                i.color = Color.black;
                            }
                        }

                        foreach (Image i in options_buttons)
                        {
                            i.sprite = Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Not_Chosen");
                            i.rectTransform.SetParent(options.rectTransform);
                        }
                    }

                    if(GameObject.Find("options1") != null)
                    {
                        if (GameObject.Find("options1").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                        {
                            GameObject.Find("options1_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-34.99997f, -25.17029f - 12f, 0);
                            GameObject.Find("options2_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-35.00003f, -25.17029f, 0);
                        }
                        else if (GameObject.Find("options2").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                        {
                            GameObject.Find("options2_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-35.00003f, -25.17029f - 12f, 0);
                            GameObject.Find("options1_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-34.99997f, -25.17029f, 0);
                        }

                        foreach(Image i in items_places)
                        {
                            if(i.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                            {
                                switch (i.rectTransform.GetChild(0).GetComponent<Image>().sprite.name)
                                {
                                    case "Test_Item":
                                        if (Input.GetKeyDown(KeyCode.D) && GameObject.Find("options1").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                                        {
                                            playerController.added_player_stats[0] += 1;
                                            foreach(Image o in items_places)
                                            {
                                                if (o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                                {
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                                    remember_sprites[items_places.IndexOf(o)] = null;
                                                }
                                            }
                                            Destroy(GameObject.Find("item_options"));
                                            utility.to_change = utility.multiple_to_change[0];
                                            utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                                        }
                                        else if(Input.GetKeyDown(KeyCode.D) && GameObject.Find("options2").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                                        {
                                            foreach (Image o in items_places)
                                            {
                                                if (o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                                {
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                                    remember_sprites[items_places.IndexOf(o)] = null;
                                                }
                                            }
                                            Destroy(GameObject.Find("item_options"));
                                            utility.to_change = utility.multiple_to_change[0];
                                            utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                                        }
                                        break;
                                    case "Test_Item_head":
                                        GameObject.Find("options1_text").GetComponent<Text>().text = "EQUIP";
                                        if (Input.GetKeyDown(KeyCode.D) && GameObject.Find("options1").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                                        {
                                            Sprite prev_sprite = null;
                                            if(eqp_slots_sprites[0] != null)
                                            {
                                                prev_sprite = eqp_slots_sprites[0].sprite;
                                            }
                                            remember_eqp_sprites[0] = i.rectTransform.GetChild(0).GetComponent<Image>().sprite;
                                            eqp_slots_sprites[0].sprite = i.rectTransform.GetChild(0).GetComponent<Image>().sprite;
                                            playerController.added_player_stats[0] += 1;
                                            foreach (Image o in items_places)
                                            {
                                                if (o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                                {
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                                    remember_sprites[items_places.IndexOf(o)] = null;
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = prev_sprite;
                                                    remember_sprites[items_places.IndexOf(o)] = prev_sprite;
                                                    if(prev_sprite != null)
                                                    {
                                                        switch (prev_sprite.name)
                                                        {
                                                            case "Test_Item_head":
                                                                playerController.added_player_stats[0] -= 1;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            Destroy(GameObject.Find("item_options"));
                                            utility.to_change = utility.multiple_to_change[0];
                                            utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                                        }
                                        else if (Input.GetKeyDown(KeyCode.D) && GameObject.Find("options2").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                                        {
                                            foreach (Image o in items_places)
                                            {
                                                if (o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                                {
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                                    remember_sprites[items_places.IndexOf(o)] = null;
                                                }
                                            }
                                            Destroy(GameObject.Find("item_options"));
                                            utility.to_change = utility.multiple_to_change[0];
                                            utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                                        }
                                        break;
                                    case "eqp_chosen":
                                        GameObject.Find("options1_text").GetComponent<Text>().text = "EQUIP"; 
                                        if (Input.GetKeyDown(KeyCode.D) && GameObject.Find("options1").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                                        {
                                            Sprite prev_sprite = null;
                                            if (eqp_slots_sprites[0] != null)
                                            {
                                                prev_sprite = eqp_slots_sprites[0].sprite;
                                            }
                                            remember_eqp_sprites[0] = i.rectTransform.GetChild(0).GetComponent<Image>().sprite;
                                            eqp_slots_sprites[0].sprite = i.rectTransform.GetChild(0).GetComponent<Image>().sprite;
                                            foreach (Image o in items_places)
                                            {
                                                if (o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                                {
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                                    remember_sprites[items_places.IndexOf(o)] = null;
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = prev_sprite;
                                                    remember_sprites[items_places.IndexOf(o)] = prev_sprite;
                                                    if(prev_sprite != null)
                                                    {
                                                        switch (prev_sprite.name)
                                                        {
                                                            case "Test_Item_head":
                                                                playerController.added_player_stats[0] -= 1;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            Destroy(GameObject.Find("item_options"));
                                            utility.to_change = utility.multiple_to_change[0];
                                            utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                                        }
                                        else if (Input.GetKeyDown(KeyCode.D) && GameObject.Find("options2").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                                        {
                                            foreach (Image o in items_places)
                                            {
                                                if (o.sprite == Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Chosen"))
                                                {
                                                    o.GetComponent<Image>().rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                                    remember_sprites[items_places.IndexOf(o)] = null;
                                                }
                                            }
                                            Destroy(GameObject.Find("item_options"));
                                            utility.to_change = utility.multiple_to_change[0];
                                            utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                                        }
                                        break;
                                }
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Destroy(GameObject.Find("item_options"));
                        utility.to_change = utility.multiple_to_change[0];
                        utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                    }
                }

                GameObject.Find("att_stat").GetComponent<Image>().rectTransform.localPosition = new Vector3(-40.20007f, 35.89996f);
                GameObject.Find("def_stat").GetComponent<Image>().rectTransform.localPosition = new Vector3(-40.20007f, -47.20001f);
            }
            else if (GameObject.Find("item_options") != null)
            {
                utility.choose_buttons(new Image[] { GameObject.Find("options1").GetComponent<Image>(), GameObject.Find("options2").GetComponent<Image>() }, new Sprite[] { Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"), Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen") },
                                          new Sprite[] { Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Not_Chosen"), Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Not_Chosen") }, 1, "ver");

                if (GameObject.Find("options1").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                {
                    GameObject.Find("options1_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-34.99997f, -25.17029f - 12f, 0);
                    GameObject.Find("options2_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-35.00003f, -25.17029f, 0);
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        foreach (Image i in eqp_slots)
                        {
                            if (i.sprite == Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"))
                            {
                                try
                                {
                                    add_normal_item(Resources.Load<Sprite>("Inventory_System_Graphics/" + i.rectTransform.GetChild(0).GetComponent<Image>().sprite.name));
                                    switch (i.rectTransform.GetChild(0).GetComponent<Image>().sprite.name)
                                    {
                                        case "Test_Item_head":
                                            playerController.added_player_stats[0] -= 1;
                                            break;
                                    }
                                    i.rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                    remember_eqp_sprites[eqp_slots.IndexOf(i)] = null;
                                    Destroy(GameObject.Find("item_options"));
                                    utility.to_change = utility.multiple_to_change[0];
                                    utility.multiple_to_change.Remove(utility.to_change);
                                }
                                catch
                                {
                                    GameObject.Find("options_text").GetComponent<Text>().text = "Your inventory is full.";
                                }
                            }
                        }
                    }
                }
                else if (GameObject.Find("options2").GetComponent<Image>().sprite == Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Chosen"))
                {
                    GameObject.Find("options2_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-35.00003f, -25.17029f - 12f, 0);
                    GameObject.Find("options1_text").GetComponent<Text>().rectTransform.localPosition = new Vector3(-34.99997f, -25.17029f, 0);
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        foreach (Image i in eqp_slots)
                        {
                            if (i.sprite == Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"))
                            {
                                switch (i.rectTransform.GetChild(0).GetComponent<Image>().sprite.name)
                                {
                                    case "Test_Item_head":
                                        playerController.added_player_stats[0] -= 1;
                                        break;
                                }
                                i.rectTransform.GetChild(0).GetComponent<Image>().sprite = null;
                                remember_eqp_sprites[eqp_slots.IndexOf(i)] = null;
                                Destroy(GameObject.Find("item_options"));
                                utility.to_change = utility.multiple_to_change[0];
                                utility.multiple_to_change.Remove(utility.to_change);
                            }
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    Destroy(GameObject.Find("item_options"));
                    utility.to_change = utility.multiple_to_change[0];
                    utility.multiple_to_change.Remove(utility.multiple_to_change[0]);
                }
            }
            foreach(Image i in inv_options_holder)
            {
                if(i.sprite == Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen"))
                {
                    if (!once)
                    {
                        foreach (Image o in items_places)
                        {
                            o.rectTransform.localPosition += new Vector3(0, 22f, 0);
                        }
                        once = true;
                    }
                }
                foreach(Image o in eqp_slots)
                {
                    if(o.sprite == Resources.Load<Sprite>("Inventory_System_Graphics/eqp_chosen") && o.rectTransform.GetChild(0).GetComponent<Image>().sprite != null && Input.GetKeyDown(KeyCode.D))
                    {
                        if (GameObject.Find("item_options") == null)
                        {
                            utility.multiple_to_change.Add(utility.to_change);
                            utility.to_change = -1;
                            Image eqp_options = (Image)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 1, new string[] { "item_options" }, new Vector2[] { new Vector2(830f, 650f) },
                                            new Vector3[] { new Vector3(41f, -351.7f) }, new Vector3[] { Vector3.zero });
                            eqp_options.sprite = Resources.Load<Sprite>("Settings_System_Graphics/GSettings_Not_Chosen");
                            eqp_options.rectTransform.SetParent(GameObject.Find("inventory_holder").transform);
                            Text options_text = (Text)utility.create_ui_object(new GameObject().AddComponent<Text>(), new System.Type[] { typeof(Text) }, 1, new string[] { "options_text" }, new Vector2[] { new Vector2(325f, 147.64f) },
                                                        new Vector3[] { new Vector3(35f, -318f) }, new Vector3[] { Vector3.zero });
                            options_text.rectTransform.SetParent(eqp_options.rectTransform);
                            options_text.fontSize = 35;
                            options_text.font = Resources.Load<Font>("Dialogue_System_Graphics/dpcomic");
                            options_text.color = Color.black;

                            switch (o.rectTransform.GetChild(0).GetComponent<Image>().sprite.name)
                            {
                                case "Test_Item_head":
                                    GameObject.Find("options_text").GetComponent<Text>().text = "This item gives +1 Attack when equipped";
                                    break;
                                case "eqp_chosen":
                                    GameObject.Find("options_text").GetComponent<Text>().text = "This item gives does not do anything, but it can be equipped.";
                                    break;
                            }

                            List<Image> options_buttons = (List<Image>)utility.create_ui_object(new GameObject().AddComponent<Image>(), new System.Type[] { typeof(Image) }, 2, new string[] { "options1", "options2" }, new Vector2[] { new Vector2(1000f, 1000f), new Vector2(1000, 1000) },
                                                                                    new Vector3[] { new Vector3(-11f, -400f), new Vector3(156f, -400f) }, new Vector3[] { Vector3.zero, Vector3.zero });
                            foreach (Image k in options_buttons)
                            {
                                k.sprite = Resources.Load<Sprite>("Dialogue_System_Graphics/Dialogue_Option_Not_Chosen");
                                k.rectTransform.SetParent(eqp_options.rectTransform);
                            }
                            List<Text> options_buttons_texts = (List<Text>)utility.create_ui_object(new GameObject().AddComponent<Text>(), new System.Type[] { typeof(Text) }, 2, new string[] { "options1_text", "options2_text" }, new Vector2[] { new Vector2(89.83f, 50.1f), new Vector2(89.83f, 50.1f) },
                                                                new Vector3[] { new Vector3(-46f, -425.1703f), new Vector3(121f, -425.1703f) }, new Vector3[] { Vector3.zero, Vector3.zero });
                            foreach (Text u in options_buttons_texts)
                            {
                                if (options_buttons_texts.IndexOf(u) == 0)
                                {
                                    u.text = "UNEQUIP";
                                    u.rectTransform.SetParent(GameObject.Find("options1").GetComponent<Image>().rectTransform);
                                }
                                else if (options_buttons_texts.IndexOf(u) == 1)
                                {
                                    u.text = "TRASH";
                                    u.rectTransform.SetParent(GameObject.Find("options2").GetComponent<Image>().rectTransform);
                                }
                                u.alignment = TextAnchor.MiddleCenter;
                                u.fontSize = 25;
                                u.font = Resources.Load<Font>("Dialogue_System_Graphics/dpcomic");
                                u.color = Color.black;
                            }
                        }                   
                    }
                }
            }
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