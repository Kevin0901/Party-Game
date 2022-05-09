using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TowerLevel
{
    public GameObject visualization;
    public float fireRate;
    public float damage;
    public int MaxHealth, CurHealth;
}

public class TowerData : MonoBehaviour
{
    [Header("往上的圖")]
    [SerializeField] private Sprite up;
    [Header("往下的圖")]
    [SerializeField] private Sprite down;
    private SpriteRenderer spriteRenderer;
    private health health;
    public ResourceAmount[] CostArray;
    public List<TowerLevel> levels;
    private TowerLevel currentLevel;
    private Animator animator;
    private void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
        SetAnimeIdle();
        health = this.GetComponentInChildren<health>();

    }
    public void SetAnimeIdle()
    {
        if (this.gameObject.tag == "red")
        {
            animator.SetBool("RedIdle", true);
            spriteRenderer.sprite = up;
        }
        else
        {
            animator.SetBool("BlueIdle", true);
            spriteRenderer.sprite = down;
        }
    }
    public TowerLevel CurrentLevel
    {
        //2
        get
        {
            return currentLevel;
        }
        //3
        set
        {
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf(currentLevel);
            GameObject levelVisualization = levels[currentLevelIndex].visualization;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                    {
                        levels[i].visualization.SetActive(true);
                    }
                    else
                    {
                        levels[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }
    private void Update()
    {
        CurrentLevel.CurHealth = health.curH;
    }
    void OnEnable()
    {
        CurrentLevel = levels[0];
        health.maxH = CurrentLevel.MaxHealth;
    }

    public TowerLevel getNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            return null;
        }
    }

    public void increaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
        }
    }

    public ResourceAmount[] returnCost()
    {
        if (CostArray != null)
        {
            return CostArray;
        }
        else
        {
            return null;
        }

    }

}
