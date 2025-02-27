using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "SO/EnemyData", order = 1)]
public class EnemySO : ScriptableObject {
    /// <summary>
    /// Ảnh chết của quái
    /// </summary>
    public Sprite spriteDate;

    /// <summary>
    /// vị trí sinh bullet + lại bullet
    /// </summary>
    public Vector2 bulletSpawnPosition;
    public int bulletId;

    /// <summary>
    /// có đánh được không
    /// </summary>
    public bool canHit;

    public bool canMove;
    /// <summary>
    /// la quái bay : không check ground , có anim chêt riêng
    /// </summary>
    public bool isFly;

    /// <summary>
    /// quái không thể di chuyển : người rơm
    /// </summary>
    public bool isStatic;
    /// <summary>
    /// độ cao của arrow + text 
    /// </summary>
    public float y;

    /// <summary>
    /// Loại attack cho tấn công 1 , 2
    /// VD : Đánh gần, đánh xa
    /// </summary>
    public AttackType AttackOne = AttackType.Range;
    public AttackType AttackTwo = AttackType.Range;

    /// <summary>
    /// vfx cho attack của quái
    /// </summary>
    public RuntimeAnimatorController muzzle;
    public Vector2 muzzlePosition = Vector2.one;
    public float muzzelTime = .5f;

    /// <summary>
    /// Clip âm thanh cho attack
    /// </summary>
    public AudioClip rangeClip;
    public AudioClip meeleClip;
}

[System.Serializable]
public enum AttackType {
    None,
    Melee,
    Range,
}

