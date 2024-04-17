using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;

namespace BannerPlugin
{
    public class BannerPlugin : BasePlugin
    {
        public FakeConVar<bool> bannerEnabled = new("banner_enable", "Whether banner is enabled or not. Default: false", false);

        public override string ModuleName => "Banner Plugin";

        public override string ModuleVersion => "0.0.1";

        public override string ModuleAuthor => "JasonX- (https://github.com/p07575/)";
    
        public override string ModuleDescription => "A plugin for showing banner";
        
        public override void Load(bool hotReload)
        {
            Console.WriteLine("Banner Plugin Loaded");
            RegisterListeners();
        }

        public void showBanner(CCSPlayerController player, string text)
        {  
            player.PrintToCenterHtml(text);
            return;
        }

        public void showHtml()
        {
        	foreach (var player in Utilities.GetPlayers())
			{
				if (player != null && bannerEnabled.Value)
				{
					player.PrintToCenterHtml("<img src='https://ftp.jasonxiang.net/Bann.png'</img>");
				}
			}
        }

        [ConsoleCommand("css_b", "alias for !b")]
        [ConsoleCommand("css_banner", "alias for !banner")]
        [ConsoleCommand("sv_showbanner", "Shows a banner in players hud")]
        public void OnBanner(CCSPlayerController? player, CommandInfo command)
        {
            if (player != null && bannerEnabled.Value) {
                showBanner(player, "You are playing on Jason's Server");
                return;
            }

            Console.WriteLine("Banner command called.");
        }

		private void OnTick()
		{
			if (!bannerEnabled.Value) return;

            showHtml();
		}

        private HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
		{
            bannerEnabled.Value = true;
			return HookResult.Continue;
		}

        private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
        {
            bannerEnabled.Value = false;
			return HookResult.Continue;
        }

		public void RegisterListeners()
		{
            RegisterListener<Listeners.OnTick>(OnTick);

            RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
            RegisterEventHandler<EventRoundStart>(OnRoundStart);
            
            return ;
		}
    }
}