using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    private bool PowerUpStatus = false;
    [Header("力量提升秒數")]
    [SerializeField] private float PowDuration = 10;
    [Header("力量提升倍數")]
    [SerializeField] private float PowMagnification = 2;

    private bool SpeedUpStatus = false;
    [Header("速度提升秒數")]
    [SerializeField] private float SpeedDuration = 10;
    [Header("速度提升倍數")]
    [SerializeField] private float SpeedMagnification = 2;

    private bool PotionHealStatus = false;
    [Header("回復單次秒數")]
    [SerializeField] private float PHPreSec = 0.5f;
    [Header("回復總秒數")]
    [SerializeField] private float PHDuration = 3;
    [Header("回復總趴數")]
    [SerializeField] private float PHPersen = 30;

    private bool StoneStatus = false;
    [Header("石化秒數")]
    [SerializeField] private float StoneDuration = 3;

    private bool BurnStatus = false;
    [Header("燃燒扣血單次秒數")]
    [SerializeField] private float BurnPreSec = 0.5f;
    [Header("燃燒扣血總秒數")]
    [SerializeField] private float BurnDuration = 3;
    [Header("燃燒扣血總趴數")]
    [SerializeField] private float BurnPersen = 30;

    private bool InvincibleStatus = false;
    [Header("燃燒扣血單次秒數")]
    [SerializeField] private float InvincibleDuration = 5;
    [Header("隨機效果數")]
    [SerializeField] private int RandEffectTimes = 3;

    private bool ProjectileStatus = false;
    [Header("投射物無限秒數")]
    [SerializeField] private float ProjectileInfiniteDuration = 5;

    public IEnumerator RandGiveEffect()
    {
        int[] nonre = new int[RandEffectTimes];
        for (int i = 0; i < RandEffectTimes; i++)
        {
            nonre[i] = Random.Range(0, 5);

            for (int j = 0; j < i; j++)
            {
                while (nonre[j] == nonre[i])
                {
                    j = 0;
                    nonre[i] = Random.Range(0, 5);
                }
            }
        }
        for (int i = 0; i < RandEffectTimes; i++)
        {
            switch (nonre[i])
            {
                case 0:
                    StartCoroutine(PowerUPEffect());
                    break;
                case 1:
                    StartCoroutine(SpeedUpEffect());
                    break;
                case 2:
                    StartCoroutine(PotionHealEffect());
                    break;
                case 3:
                    StartCoroutine(StoneEffect());
                    break;
                case 4:
                    StartCoroutine(BurnEffect());
                    break;
                case 5:
                    StartCoroutine(InvincibleEffect());
                    break;
            }

        }
        yield return null;
    }

    public IEnumerator PowerUPEffect()
    {
        float orgindamage;
        if (!PowerUpStatus)
        {
            Debug.Log("powerup");
            PowerUpStatus = true;
            if (this.gameObject.layer == 10)
            {
                orgindamage = this.GetComponent<PlayerMovement>().attackDamage;
                this.GetComponent<PlayerMovement>().attackDamage = (int)(orgindamage * PowMagnification);
                Debug.Log(transform.parent.Find("EffectUI").gameObject);
                // this.transform.Find("EffectUI").transform.GetChild(0).transform.Find("Power").gameObject.SetActive(true);
                yield return new WaitForSeconds(PowDuration);
                this.GetComponent<PlayerMovement>().attackDamage = (int)orgindamage;
                PowerUpStatus = false;
            }
            else
            {
                orgindamage = this.GetComponent<monsterMove>().attackDamage;
                this.GetComponent<monsterMove>().attackDamage = (int)(orgindamage * PowMagnification);
                yield return new WaitForSeconds(PowDuration);
                this.GetComponent<monsterMove>().attackDamage = (int)orgindamage;
                PowerUpStatus = false;
            }
        }
    }

    public IEnumerator SpeedUpEffect()
    {
        float orginspeed;
        if (!SpeedUpStatus)
        {
            Debug.Log("speedup");
            SpeedUpStatus = true;
            if (this.gameObject.layer == 10)
            {
                orginspeed = this.GetComponent<PlayerMovement>().orginspeed;
                this.GetComponent<PlayerMovement>().speed = orginspeed * SpeedMagnification;
                yield return new WaitForSeconds(SpeedDuration);
                this.GetComponent<PlayerMovement>().speed = orginspeed;
                SpeedUpStatus = false;
            }
            else
            {
                orginspeed = this.GetComponent<monsterMove>().speed;
                this.GetComponent<monsterMove>().speed = orginspeed * SpeedMagnification;
                yield return new WaitForSeconds(SpeedDuration);
                this.GetComponent<monsterMove>().speed = orginspeed;
                SpeedUpStatus = false;
            }
        }
    }

    public IEnumerator PotionHealEffect()
    {
        float orginspeed;
        if (!PotionHealStatus)
        {
            Debug.Log("healup");
            PotionHealStatus = true;
            if (this.gameObject.layer == 10)
            {
                orginspeed = this.GetComponent<PlayerMovement>().orginspeed;
                health targetheal = this.GetComponentInChildren<health>();

                this.GetComponent<PlayerMovement>().speed = orginspeed / 2;
                float times = PHDuration / PHPreSec;
                for (int i = 0; i < times; i++)
                {

                    int reheal = (int)((targetheal.maxH * (PHPersen / 100)) / times);
                    if (targetheal.curH < targetheal.maxH)
                    {
                        if (targetheal.curH + reheal > targetheal.maxH)
                        {
                            targetheal.curH = targetheal.maxH;
                        }
                        else
                        {
                            targetheal.curH += reheal;
                        }
                    }
                    yield return new WaitForSeconds(PHPreSec);
                }
                this.GetComponent<PlayerMovement>().speed = orginspeed;
                PotionHealStatus = false;
            }
            else
            {
                orginspeed = this.GetComponent<monsterMove>().speed;
                health targetheal = this.GetComponentInChildren<health>();

                this.GetComponent<monsterMove>().speed = orginspeed / 2;
                float times = PHDuration / PHPreSec;
                for (int i = 0; i < times; i++)
                {
                    int reheal = (int)((targetheal.maxH * (PHPersen / 100)) / times);
                    if (targetheal.curH < targetheal.maxH)
                    {
                        if (targetheal.curH + reheal > targetheal.maxH)
                        {
                            targetheal.curH = targetheal.maxH;
                        }
                        else
                        {
                            targetheal.curH += reheal;
                        }
                    }
                    yield return new WaitForSeconds(PHPreSec);
                }
                this.GetComponent<monsterMove>().speed = orginspeed;
                PotionHealStatus = false;
            }
        }
    }

    public IEnumerator StoneEffect()
    {
        float orginspeed;
        if (!StoneStatus)
        {
            Debug.Log("stone");
            StoneStatus = true;
            if (this.gameObject.layer == 10)
            {
                orginspeed = this.GetComponent<PlayerMovement>().orginspeed;
                this.GetComponent<PlayerMovement>().speed = 0;
                this.GetComponent<SpriteRenderer>().color = new Color32(89, 89, 89, 255);
                yield return new WaitForSeconds(StoneDuration);
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                this.GetComponent<PlayerMovement>().speed = orginspeed;
                StoneStatus = false;
            }
            else
            {
                orginspeed = this.GetComponent<monsterMove>().speed;
                this.GetComponent<monsterMove>().speed = 0;
                this.GetComponent<SpriteRenderer>().color = new Color32(89, 89, 89, 255);
                yield return new WaitForSeconds(StoneDuration);
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                this.GetComponent<monsterMove>().speed = orginspeed;
                StoneStatus = false;
            }
        }

    }

    public IEnumerator BurnEffect()
    {
        if (!BurnStatus)
        {
            Debug.Log("buring");
            BurnStatus = true;
            health targetheal = this.GetComponentInChildren<health>();
            float times = BurnDuration / BurnPreSec;
            for (int i = 0; i < times; i++)
            {
                if (!targetheal.iswudi)
                {
                    int reheal = (int)((targetheal.maxH * (BurnPersen / 100)) / times);
                    this.GetComponent<SpriteRenderer>().color = new Color32(255, 60, 60, 255);
                    targetheal.curH -= reheal;
                    yield return new WaitForSeconds(BurnPreSec);
                    this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                }
            }
            BurnStatus = false;
        }
    }

    public IEnumerator InvincibleEffect()
    {
        if (!InvincibleStatus)
        {
            Debug.Log("widi!");
            health targetstatus = this.GetComponentInChildren<health>();
            targetstatus.iswudi = true;
            yield return new WaitForSeconds(InvincibleDuration);
            targetstatus.iswudi = false;
        }
    }

    public IEnumerator ProjectileInfiniteEffect()
    {
        if (!ProjectileStatus && this.gameObject.layer == 10)
        {
            Debug.Log("infinite!");
            this.GetComponentInChildren<PlayerMovement>().ProjectileCost = 0;
            yield return new WaitForSeconds(ProjectileInfiniteDuration);
            this.GetComponentInChildren<PlayerMovement>().ProjectileCost = 1;

        }
    }
}
