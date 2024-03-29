using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
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
        public static Dictionary<Player, ExplosiveGrenade> GranadaLanzada = new Dictionary<Player, ExplosiveGrenade>();
        public void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
            if (ev.Throwable is ExplosiveGrenade grenadex)
            {
                GranadaLanzada[ev.Player] = grenadex;
            }

            if (ev.RequestType == ThrowRequest.BeginThrow  && ev.Throwable is ExplosiveGrenade)
            {
                Timing.RunCoroutine(GrenadeTime(ev.Player),$"GrenadeTime{ev.Player.NetId}");
            }
            if(ev.RequestType == ThrowRequest.CancelThrow && ev.Throwable is ExplosiveGrenade)
            {
                GranadaActiva[ev.Player].Destroy();
                Timing.KillCoroutines($"GrenadeTime{ev.Player.NetId}");
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


        
        
        public static IEnumerator<float> GrenadeTime(Player player)
        {
            var fuse = 5f;
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 5f;
            ExplosionGrenadeProjectile granadaActiva = grenade.SpawnActive(new Vector3(player.Position.x,player.Position.y +2,player.Position.z));
            granadaActiva.PreviousOwner = player;
            granadaActiva.Scale = new Vector3(0.1f,0.1f,0.1f) ;
            granadaActiva.Rigidbody.useGravity = false;
            GranadaActiva[player] = granadaActiva;
            for (;;)
            {
                
                CosaQueExplota[player] = fuse;
                fuse -= 0.1f;
                Log.Debug(fuse);
                granadaActiva.Position = player.Position;
                if (fuse <= 0)
                {
                    GranadaLanzada[player].Destroy();
                    yield break;
                }
                yield return Timing.WaitForSeconds(0.1f);
            }

            
        }
    }
}