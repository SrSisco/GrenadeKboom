using System.ComponentModel;
using Exiled.API.Interfaces;

namespace GrenadeKboom
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("Are debugging messages shown?")]
        public bool Debug { get; set; } = false;
    }
}