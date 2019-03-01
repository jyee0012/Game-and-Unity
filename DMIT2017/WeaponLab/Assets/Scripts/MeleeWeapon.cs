using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    float rotateSpeed = 100;
    bool animate = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {
        base.Use();
    }
    void AnimateSwing()
    {
        if (animate)
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.z > 40)
            {
                rotateSpeed *= -1;
            }
            if (transform.rotation.eulerAngles.z <= 0)
            {
                rotateSpeed *= -1;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                animate = false;
            }
        }
    }
}
