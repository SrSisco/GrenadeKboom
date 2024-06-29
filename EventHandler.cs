using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using UnityEngine;
using Utils;

namespace GrenadeKboom
{
    
    public class EventHandler
    {

        public static Dictionary<Player,float> CosaQueExplota = new Dictionary<Player, float>();
        public static Dictionary<Player, ExplosionGrenadeProjectile> GranadaActiva = new Dictionary<Player, ExplosionGrenadeProjectile>();
        public void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
            if (ev.RequestType == ThrowRequest.BeginThrow  && ev.Throwable is ExplosiveGrenade)
            {
                Timing.RunCoroutine(GrenadeTime(ev.Player),$"GrenadeTime{ev.Player.NetId}");
            }
            if(ev.RequestType == ThrowRequest.CancelThrow && ev.Throwable is ExplosiveGrenade)
            {
                Timing.KillCoroutines($"GrenadeTime{ev.Player.NetId}");
                GranadaActiva[ev.Player].Destroy();
            }

            if (ev.RequestType == ThrowRequest.WeakThrow || ev.RequestType == ThrowRequest.FullForceThrow)
            {
                if (ev.Throwable is ExplosiveGrenade grenade)
                {
                    Timing.KillCoroutines($"GrenadeTime{ev.Player.NetId}");
                    GranadaActiva[ev.Player].Destroy();
                    Log.Debug("Granada lanzada");
                    grenade.FuseTime = CosaQueExplota[ev.Player];
                    Log.Debug("Granada lanzada con tiempo de " + CosaQueExplota[ev.Player]);

                }
                
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Player.CurrentItem == null) return;
    
            if (ev.Player.CurrentItem.Type == ItemType.GrenadeHE)
            {
                ev.Player.CurrentItem.Destroy();
            }

            
        }

        
        
        public static IEnumerator<float> GrenadeTime(Player player)
        {
            var fuse = 5f;
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 5f;
            ExplosionGrenadeProjectile granadaActiva = grenade.SpawnActive(new Vector3(player.Position.x,player.Position.y +1,player.Position.z));
            granadaActiva.Weight = 0f;
            granadaActiva.Rigidbody.useGravity = false;
            granadaActiva.Scale = new Vector3(0.01f,0.01f,0.01f);
            granadaActiva.PreviousOwner = player;
            
            GranadaActiva[player] = granadaActiva;
            for (;;)
            {
                CosaQueExplota[player] = fuse;
                fuse -= 0.1f;
                Log.Debug(fuse);
                granadaActiva.Position = player.Position;
                if (fuse <= 0)
                {
                    yield break;
                }
                yield return Timing.WaitForSeconds(0.1f);
            }

            
        }
    }
}