using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagebleObject : MonoBehaviour
{
    public int lives;
    public int damage;
    public Wizard dieWizard;
    public Skull  dieSkull;
    public Angel  dieAngel;

    public void TakeDamage(int damage) {
        if (gameObject.tag == "Wizard") {
            Wizard.lives -= damage;
            dieWizard = GetComponent<Wizard>();
            dieWizard.Die(); 
        }
        if (gameObject.tag == "Enemy") {
            Skull.lives -= damage;
            dieSkull = GetComponent<Skull>();
            dieSkull.Die();
        }
        if (gameObject.tag == "Boss") {
            Angel.lives -= damage;
            dieAngel = GetComponent<Angel>();
            dieAngel.Die();
        }
    }
}
