using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Health EnemyHealth;

    public Image HealthBarImage;

    public Transform HealthBar;

    public bool HideFullHealthBar;

    public float ViewDistance;

    private void Start()
    {
        if (this.gameObject.GetComponent<Health>() != null)
        {
            EnemyHealth = this.gameObject.GetComponent<Health>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarImage.fillAmount = EnemyHealth.CurrentHealth / EnemyHealth.MaxHealth;

        HealthBar.LookAt(Camera.main.transform.position);

        if (HideFullHealthBar)
        {
            HealthBar.gameObject.SetActive(HealthBarImage.fillAmount != 1);
        }

        if (Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position) < ViewDistance)
        {
            HealthBar.gameObject.SetActive(true);
        }
        else
        {
            HealthBar.gameObject.SetActive(false);
        }

    }
}
