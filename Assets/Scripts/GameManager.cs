using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Game Manager Manages everything
public class GameManager : MonoBehaviour
{
    private bool isPrimaryPlayersTurn = true;

    [SerializeField]
    private GameObject Dataset;

    [SerializeField] private GameObject evolutionVFX;
    [SerializeField] private GameObject healthVFX;
    [SerializeField] private GameObject manaVFX;

    private GameObject[] primaryPlayerCharacters;
    private GameObject[] secondaryPlayerCharacters;
    private JSONReader dataset;

    [SerializeField] private TextMeshProUGUI playerDetailBar;
    [SerializeField] private TextMeshProUGUI a1;
    [SerializeField] private TextMeshProUGUI a2;
    [SerializeField] private TextMeshProUGUI a3;
    [SerializeField] private TextMeshProUGUI i1;
    [SerializeField] private TextMeshProUGUI i2;
    [SerializeField] private TextMeshProUGUI i3;

    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject attackMenu;
    [SerializeField] private GameObject itemMenu;

    private Animator player1Animator;
    private Animator player2Animator;

    public void togglePlayer()
    {
        isPrimaryPlayersTurn = !isPrimaryPlayersTurn;
        
        //set battle menu to default
        battleMenu.SetActive(true);
        attackMenu.SetActive(false);
        itemMenu.SetActive(false);

        updateBattleMenu();
    }

    public void Start()
    {
        dataset = Dataset.GetComponent<JSONReader>();
        primaryPlayerCharacters = new GameObject[] { };
        secondaryPlayerCharacters = new GameObject[] { };
        updateBattleMenu();
    }

    public void Update()
    {
        if (primaryPlayerCharacters.Length == 0 || secondaryPlayerCharacters.Length == 0)
        {
            // get 1st player's active characters.
            primaryPlayerCharacters = GameObject.FindGameObjectsWithTag("Player1");
            foreach (GameObject character in primaryPlayerCharacters)
            {
                if (!character.activeInHierarchy)
                {
                    Utilities.RemoveFromArray(primaryPlayerCharacters, character);
                }
            }
            
            // get 2nd player's active characters.
            secondaryPlayerCharacters = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject character in secondaryPlayerCharacters)
            {
                if (!character.activeInHierarchy)
                {
                    Utilities.RemoveFromArray(secondaryPlayerCharacters, character);
                }
            }
            updateBattleMenu();
        }
    }

    // TODO: limit changes to hp range
    // check for death and zero mana before using attacks or his chance
    public void affectPlayerWithAttack(string attackName)
    {
        Attack att1 = new Attack("tackle", 30, 50);
        Attack att2 = new Attack("jump attack", 50, 10);
        Attack att3 = new Attack("takedown", 100, 10);
        Attack[] attackArr = new Attack[] { att1, att2, att3 };

        // find which player
        // add effect or attack
        // check if dead or has enough exp to evolve
        // toggle player
        if (isPrimaryPlayersTurn)
        {
            ICharacter player = primaryPlayerCharacters[0].GetComponent<ICharacter>();
            string attackNo = attackName; // initial number of attack.
            attackName = getAttackName(attackName, player); // reinitialize attack name with correct attack name
            
            foreach (Attack attack in attackArr)
            {
                if (attack.name == attackName)
                {
                    Attack currentAttack = attack;
                    ICharacter opponent = secondaryPlayerCharacters[0].GetComponent<ICharacter>(); //TODO enable

                    // current character animation
                    // vfx
                    // opponent take damage animation
                    // update stats, current char mana, opponent health
                    // switch player

                    //get animators
                    player1Animator = primaryPlayerCharacters[0].GetComponent<Animator>();
                    StartCoroutine(playAnimationDependingOnAttackNumberCoroutine(player1Animator, attackNo));

                    opponent.Hitpoints = opponent.Hitpoints - currentAttack.damage;
                    if (opponent.Hitpoints == 0)
                    {
                        player2Animator = secondaryPlayerCharacters[0].GetComponent<Animator>();
                        GameObject tempPlayer = secondaryPlayerCharacters[0];
                        StartCoroutine(playDeathAnimationCoroutine(player2Animator, tempPlayer));
                    }

                    player.Mana = player.Mana - currentAttack.cost;
                    player.ExperiencePoints = player.ExperiencePoints - currentAttack.cost;
                    StartCoroutine(evolveCoroutine(player));
                }
            }
        } else
        {
            ICharacter player = secondaryPlayerCharacters[0].GetComponent<ICharacter>();
            string attackNo = attackName; // initial number of attack.
            attackName = getAttackName(attackName, player); // reinitialize attack name with correct attack name

            foreach (Attack attack in attackArr)
            {
                if (attack.name == attackName)
                {
                    Attack currentAttack = attack;
                    ICharacter opponent = primaryPlayerCharacters[0].GetComponent<ICharacter>(); //TODO enable

                    // current character animation
                    // vfx
                    // opponent take damage animation
                    // update stats, current char mana, opponent health
                    // switch player

                    //get animators
                    player2Animator = secondaryPlayerCharacters[0].GetComponent<Animator>();
                    StartCoroutine(playAnimationDependingOnAttackNumberCoroutine(player2Animator, attackNo));

                    opponent.Hitpoints = opponent.Hitpoints - currentAttack.damage;

                    if (opponent.Hitpoints == 0)
                    {
                        player1Animator = primaryPlayerCharacters[0].GetComponent<Animator>();
                        GameObject tempPlayer = primaryPlayerCharacters[0];
                        StartCoroutine(playDeathAnimationCoroutine(player1Animator, tempPlayer));
                    }
                    player.Mana = player.Mana - currentAttack.cost;
                }
            }
        }
        //change player
        togglePlayer();
    }

    IEnumerator evolveCoroutine(ICharacter player)
    {
        if (player.ExperiencePoints <= 0)
        {
            // TODO play effect

            yield return new WaitForSeconds(3);
            for (int i = 0; i < 5; i++) {
                yield return new WaitForSeconds(1);
                Instantiate(evolutionVFX, primaryPlayerCharacters[0].GetComponent<Transform>().position, primaryPlayerCharacters[0].GetComponent<Transform>().rotation);
            }

            GameObject parent = primaryPlayerCharacters[0].GetComponent<Transform>().parent.gameObject;
            GameObject p1chars = parent.FindObject("MegaMutant");

            GameObject evolution = p1chars;
            primaryPlayerCharacters[0].SetActive(false);
            evolution.SetActive(true);

            primaryPlayerCharacters[0] = evolution;
            player1Animator = evolution.GetComponent<Animator>();
        }
    }

    IEnumerator playDeathAnimationCoroutine(Animator animator, GameObject toBeDeleted)
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(6);
        toBeDeleted.SetActive(false);
        //Destroy(toBeDeleted);
    }

    IEnumerator playAnimationDependingOnAttackNumberCoroutine(Animator animator, string attackNo)
    {
        Debug.Log(animator);
        if (attackNo == "1")
            animator.SetBool("attack1", true);
        else if (attackNo == "2")
            animator.SetBool("attack2", true);
        else if (attackNo == "3")
            animator.SetBool("attack3", true);

        Animator enemyAnimator;
        if (isPrimaryPlayersTurn)
            enemyAnimator = secondaryPlayerCharacters[0].GetComponent<Animator>();
        else
            enemyAnimator = primaryPlayerCharacters[0].GetComponent<Animator>();

        enemyAnimator.SetBool("damaged", true);

        yield return new WaitForSeconds(4);
        animator.SetBool("attack1", false);
        animator.SetBool("attack2", false);
        animator.SetBool("attack3", false);
        enemyAnimator.SetBool("damaged", false);
    }

    string getAttackName(string attackNumber, ICharacter player)
    {
        string attackName = player.Attacks[int.Parse(attackNumber) - 1];
        return attackName;
    }

    string getItemName(string itemNumber, ICharacter player)
    {
        string itemName = player.Items[int.Parse(itemNumber) - 1];
        return itemName;
    }

    // TODO: limit changes to mana or hp range
    public void affectPlayerWithItem(string itemName)
    {
        Item item1 = new Item("health potion", "hp", "add", 20);
        Item item2 = new Item("mana potion", "mana", "add", 20);
        Item item3 = new Item("L.health", "hp", "add", 40);
        Item[] itemArr = new Item[] { item1, item2, item3 };

        // find which player
        // add effect to stat
        // toggle player
        if (isPrimaryPlayersTurn)
        {
            ICharacter player = primaryPlayerCharacters[0].GetComponent<ICharacter>();
            itemName = getItemName(itemName, player);
            foreach (Item item in itemArr)
            {
                if (item.name == itemName)
                {
                    Item currentItem = item;
                    // current character animation
                    // vfx
                    // opponent take damage animation
                    // update stats, current char mana, opponent health
                    // switch player
                    if (currentItem.stat == "hp")
                    {
                        player.Hitpoints = player.Hitpoints + currentItem.amount;
                        StartCoroutine(healthVFXCoroutine(primaryPlayerCharacters[0]));
                    } else if (currentItem.stat == "mana")
                    {
                        player.Mana = player.Mana + currentItem.amount;
                        StartCoroutine(manaVFXCoroutine(primaryPlayerCharacters[0]));
                    }
                }
            }
        }
        else
        {
            ICharacter player = secondaryPlayerCharacters[0].GetComponent<ICharacter>();
            itemName = getItemName(itemName, player);
            foreach (Item item in itemArr)
            {
                if (item.name == itemName)
                {
                    Debug.Log(itemName);
                    Item currentItem = item;
                    // current character animation
                    // vfx
                    // opponent take damage animation
                    // update stats, current char mana, opponent health
                    // switch player
                    if (currentItem.stat == "hp")
                    {
                        player.Hitpoints = player.Hitpoints + currentItem.amount;
                        StartCoroutine(healthVFXCoroutine(secondaryPlayerCharacters[0]));
                    }
                    else if (currentItem.stat == "mana")
                    {
                        player.Mana = player.Mana + currentItem.amount;
                        StartCoroutine(manaVFXCoroutine(secondaryPlayerCharacters[0]));
                    }
                }
            }
        }

        // change player
        togglePlayer();
    }

    IEnumerator healthVFXCoroutine(GameObject player)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(healthVFX, player.GetComponent<Transform>().position, player.GetComponent<Transform>().rotation);
        }
    }

    IEnumerator manaVFXCoroutine(GameObject player)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(manaVFX, player.GetComponent<Transform>().position, player.GetComponent<Transform>().rotation);
        }
    }


    public void updateBattleMenu()
    {
        if (primaryPlayerCharacters.Length != 0 && secondaryPlayerCharacters.Length != 0)
        {
            GameObject currentPlayer;
            currentPlayer = (isPrimaryPlayersTurn) ? primaryPlayerCharacters[0] : secondaryPlayerCharacters[0];
            //currentPlayer = primaryPlayerCharacters[0];

            ICharacter character = currentPlayer.GetComponent<ICharacter>();

            // set which players turn text.
            if (isPrimaryPlayersTurn)
            {
                playerDetailBar.text = "Player 1's Turn";
            }
            else
            {
                playerDetailBar.text = "Player 2's Turn";
            }
            a1.text = character.Attacks[0];
            a2.text = character.Attacks[1];
            a3.text = character.Attacks[2];

            i1.text = character.Items[0];
            i2.text = character.Items[1];
            i3.text = character.Items[2];
        }
    }

    public void exit()
    {
        Application.Quit();
    }

    /*
     * Game manager has reference to battle menu.
     * using isPrimaryPlayersTurn and that players object, it populates each button individually in a loop
     * shows the main menu first and hides the others
     * depending on the button clicks, menus are hidden and shown
     */

    // update battle menu UI
    public void selectFightMenu()
    {
        battleMenu.SetActive(false);
        attackMenu.SetActive(true);
        itemMenu.SetActive(false);
    }
    
    public void selectItemMenu()
    {
        battleMenu.SetActive(false);
        attackMenu.SetActive(false);
        itemMenu.SetActive(true);
    }

    public void goBack()
    {
        battleMenu.SetActive(true);
        attackMenu.SetActive(false);
        itemMenu.SetActive(false);
    }
}
