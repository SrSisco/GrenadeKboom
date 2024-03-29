using System;
using Exiled.API.Features;

namespace GrenadeKboom
{
    public class GrenadeKboom : Plugin<Config>
    {
        public override string Name => "GrenadeKboom";
        public override string Author => "srsisco";
        public override Version Version => new Version(1, 0, 0);

        public static GrenadeKboom Instance;
        public EventHandler _handlers;



        
        public override void OnEnabled()
        {
            Instance = this;
            _handlers = new EventHandler();
            
            Exiled.Events.Handlers.Player.ThrowingRequest += _handlers.OnThrowingRequest;
            

            Log.Info("GrenadeKboom has been enabled.");
            base.OnEnabled();

        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ThrowingRequest -= _handlers.OnThrowingRequest;
            

            _handlers = null;

            Instance = null;

            Log.Info("GrenadeKboom has been disabled.");
            base.OnDisabled();
        }
    }
}