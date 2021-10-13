using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        //this will delete a duplicate object when going to the portal and go back.

        if (GameManager.instance !=null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(HUD);
            Destroy(menu);
            return;
        }

        //this will delete save state.
        //PlayerPrefs.DeleteAll();
        instance = this;
        
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //DontDestroyOnLoad(gameObject);
    }

    //Resources

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //Refernces
    public Player player;

    //Public weapon weapon...
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject HUD;
    public GameObject menu;
    public Animator deathMenuAnim;

    //Logic
    public int pesos;
    public int experience;

    //Floating text
    public void ShowText(string msg, int fontSize, Color color
                            , Vector3 position, Vector3 motion,float duration)
    {
        floatingTextManager.Show(msg,fontSize,color,position,motion,duration);
    }

    //Upgrade weapon
    public bool tryUpgradeWeapon()
    {
        // is the weapon max
        if(weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

        if(pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    //HitpointBar
    public void OnHitPointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    //Experience System
    public int GetCurrentLevel()
    {
        // return valve
        int r = 0;
        int add = 0;

        //this will check the experience need to lvl up in the xpTable.
        //When the exp reach once of the xptable they will leveling up and reach a new
        //xptable bar.
        while(experience>=add)
        {
            add += xpTable[r];
            r++;

            if(r == xpTable.Count)//max level
            {
                return r;
            }
        }
        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public void GrantXp(int xp )
    {
        int currlevel = GetCurrentLevel();
        experience += xp;
        if (currlevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        
        player.OnLevelUp();
        OnHitPointChange();
    }
    //Save state
    /*
     * INT preferedSkin
     * Int pesos
     * Int experience
     * Int weaponLevel
     */
    //On Scene Loaded

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("spawnPoint").transform.position;
    }

    //Death Menu and Respawn.
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("hide");
        SceneManager.LoadScene("Main");
        player.Respawn();
    }


    //Save State
    public void SaveState()
    {
        Debug.Log("SaveState");

        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);

        //Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //0 |10|15|2

        //Change player skin
        //Pesos
        pesos = int.Parse(data[1]);

        //experience
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() !=1)

            player.SetLevel(GetCurrentLevel());
        //Change the level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        //player.transform.position = GameObject.Find("spawnPoint").transform.position;
        
    }

   
}
