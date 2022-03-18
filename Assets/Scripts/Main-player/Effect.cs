using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    private bool PowerUpStatus = false;
    [Header("力量提升秒數")]
    [SerializeField] private float PowDuration;
    [Header("力量提升倍數")]
    [SerializeField] private float PowMagnification;

    private bool SpeedUpStatus = false;
    [Header("速度提升秒數")]
    [SerializeField] private float SpeedDuration;
    [Header("速度提升倍數")]
    [SerializeField] private float SpeedMagnification;

    private bool PotionHealStatus = false;
    [Header("回復單次秒數")]
    [SerializeField] private float PHPreSec;
    [Header("回復總秒數")]
    [SerializeField] private float PHDuration;
    [Header("回復趴數")]
    [SerializeField] private float PHPersen;

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
                orginspeed = this.GetComponent<PlayerMovement>().speed;
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
                orginspeed = this.GetComponent<PlayerMovement>().speed;
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
}
