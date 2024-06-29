using System;
using Exiled.API.Features;

namespace GrenadeKboom
{
    public class GrenadeKboom : Plugin<Config>
    {
        public override string Name => "GrenadeKboom";
        public override string Author => "srsisco";
        public override Version Version => new Version(1, 1, 0);

        public static GrenadeKboom Instance;
        public EventHandler _handlers;



        
        public override void OnEnabled()
        {
            Instance = this;
            _handlers = new EventHandler();
            
            Exiled.Events.Handlers.Player.ThrowingRequest += _handlers.OnThrowingRequest;
            Exiled.Events.Handlers.Player.Dying += _handlers.OnDying;

            Log.Info("Spies has been enabled.");
            base.OnEnabled();

        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ThrowingRequest -= _handlers.OnThrowingRequest;
            Exiled.Events.Handlers.Player.Dying -= _handlers.OnDying;

            _handlers = null;

            Instance = null;

            Log.Info("Spies has been disabled.");
            base.OnDisabled();
        }
    }
}