using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BasicWeapon : PlayerWeaponBase
{
    [Serializable]
    public struct WeaponLevelHolder {
        [Range(1, 5)]
        public int level;
        public Transform[] shootPoints;
    }

    public WeaponLevelHolder[] weaponLevelHolders;

    private Dictionary<int, WeaponLevelHolder> weaponsHolderDictionary = new ();


    private void Start()
    {
        foreach(WeaponLevelHolder holder in weaponLevelHolders){
            weaponsHolderDictionary[holder.level] = holder;
        }
    }


    protected override void OnWeaponUse()
    {
        WeaponLevelHolder currentHolder = weaponsHolderDictionary[weaponLevel];
        foreach(Transform shootPoint in currentHolder.shootPoints){
            ProjectileEntity newProjectile = (ProjectileEntity) PoolableObject.Spawn(
                projectilePrefab,
                shootPoint.position,
                MainLevelManagement.instance.projectilesParent.transform
            );

            newProjectile.Init(ownerPlayer, shootPoint.forward, damage);
            newProjectile.transform.rotation = shootPoint.rotation;
        }
    }
}


public class BasicWeaponGizmos {
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    public static void DrawGizmos(BasicWeapon basicWeapon, GizmoType gizmoType){
        Gizmos.color = Color.green;
        foreach(BasicWeapon.WeaponLevelHolder holder in basicWeapon.weaponLevelHolders){
            foreach(Transform point in holder.shootPoints){
                Gizmos.DrawWireSphere(point.position, 0.2f);
                GizmoUtils.DrawArrow(point.position, point.forward, 1.0f, 25.0f);
            }
        }
    }
}
