using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(PlayerInput))]
public class SoundScript : MonoBehaviour
{
    [Inject(Id =  "FireSound")]
    AudioSource _fireSound;
    [Inject(Id = "HitSound")]
    AudioSource _hitSound;
    [Inject(Id = "ReloadSound")]
    AudioSource _reloadSound;

    public void OnShoot(InputAction.CallbackContext ctx) => _fireSound.Play();
    public void OnHit(InputAction.CallbackContext ctx) => _hitSound.Play();
    public void OnReload(InputAction.CallbackContext ctx) => _reloadSound.Play();
}
