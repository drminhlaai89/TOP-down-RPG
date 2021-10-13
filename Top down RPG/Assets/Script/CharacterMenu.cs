using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    //logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    //characterSelection
    public void onArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            //if we went to far away
            if(currentCharacterSelection == GameManager.instance.playerSprites.Count)
            {
                currentCharacterSelection = 0;
            }
            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            //if we went to far away
            if (currentCharacterSelection < 0)
            {
                //it will push back in 0;
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }
            OnSelectionChanged();
        }
    }
    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    //Weapon Upgrade

    public void OnUpgradeClick()
    {
        if(GameManager.instance.tryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    //Update the character Information

    public void UpdateMenu()
    {
        //Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
        {
            upgradeCostText.text = "MAX";
        }
        else
        {
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }


        //Meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //xpBar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if(GameManager.instance.GetCurrentLevel()== GameManager.instance.xpTable.Count)
        {
            //display total xp
            xpText.text = GameManager.instance.experience.ToString() + "Total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel -1);
            int currLevelxp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelxp - prevLevelXp;

            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;
            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + "/" + diff;
        }

       
    }
}
