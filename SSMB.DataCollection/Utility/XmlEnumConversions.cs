namespace UpdateItemListTask
{
    using SSMB.Domain;

    public static class XmlEnumConversions
    {
        public static ItemType ItemTypeFromString(string str)
        {
            switch (str)
            {
                case "WEAPON": return ItemType.Weapon;
                case "augreset": return ItemType.AugmenterReset;
                case "auratweak": return ItemType.AuraTweak;
                case "BASE": return ItemType.Base;
                case "beacon": return ItemType.Beacon;
                case "COMMODITY": return ItemType.Commodity;
                case "cop": return ItemType.Cop;
                case "spiritvessel": return ItemType.EscapePod;
                case "escapepod": return ItemType.EscapePod;
                case "fighterbay": return ItemType.FighterBay;
                case "fighter": return ItemType.Fighter;
                case "PILLBOX": return ItemType.Drone;
                case "MISSION": return ItemType.Mission;
                case "storybox": return ItemType.StoryBox;
                case "auraprojector": return ItemType.FieldGenerator;
                case "generator": return ItemType.Generator;
                case "holoprojector": return ItemType.HoloProjector;
                case "HULL": return ItemType.Ship;
                case "inkbomb": return ItemType.InkBomb;
                case "itemcookbook": return ItemType.ItemCookBook;
                case "AUGMENTER": return ItemType.Augmenter;
                case "microwarp": return ItemType.Microwarper;
                case "translocator": return ItemType.Translocator;
                case "missilebay": return ItemType.MissileBay;
                case "fighterrecall": return ItemType.FighterRecall;
                case "destructionator": return ItemType.Destructionator;
                case "fieldamplifier": return ItemType.FieldAmplifier;
                case "ambushinatrix": return ItemType.FieldAmplifier;
                case "sniperanalyzer": return ItemType.SniperAnalyzer;
                case "seertargettracker": return ItemType.TargetTracker;
                case "targettracker": return ItemType.TargetTracker;
                case "insultor": return ItemType.Insultor;
                case "GPS": return ItemType.GPS;
                case "oracle": return ItemType.Oracle;
                case "blackbox": return ItemType.BlackBox;
                case "epiphany": return ItemType.Epiphany;
                case "selfdestruct": return ItemType.SelfDestruct;
                case "census": return ItemType.Census;
                case "echo": return ItemType.Echo;
                case "shipscanner": return ItemType.ShipScanner;
                case "shieldhealer": return ItemType.ShieldHealer;
                case "energyboost": return ItemType.EnergyBoost;
                case "womb": return ItemType.Womb;
                case "capacitor": return ItemType.Capacitor;
                case "overloader": return ItemType.Overloader;
                case "massimpactor": return ItemType.MassImpactor;
                case "tractorthruster": return ItemType.Thruster;
                case "thruster": return ItemType.Thruster;
                case "neurotweak": return ItemType.Neurotweak;
                case "TRACTOR": return ItemType.Tractor;
                case "repairkit": return ItemType.RepairKit;
                case "demokit": return ItemType.DemolitionKit;
                case "planetscanner": return ItemType.PlanetScanner;
                case "galscanner": return ItemType.GalaxyScanner;
                case "creditscanner": return ItemType.CreditScanner;
                case "basescanner": return ItemType.BaseScanner;
                case "shieldcharger": return ItemType.ShieldCharger;
                case "ShipTechUpper": return ItemType.ShipTechUpgrade;
                case "solarpanel": return ItemType.SolarPanel;
                case "soundprojector": return ItemType.SoundProjector;
                case "itemsteleporter": return ItemType.ItemsTeleporter;
                case "augreseter": return ItemType.AugmenterReseter;
                case "shipformer": return ItemType.ShipFormer;
                case "itemnestersis": return ItemType.SuperItem;
                case "superitem": return ItemType.SuperItem;
                case "transwarp": return ItemType.Transwarp;
                case "ENERGY": return ItemType.Energy;
                case "ENGINE": return ItemType.Engine;
                case "RADAR": return ItemType.Radar;
                case "SHIELD": return ItemType.Shield;
                case "parasitemuncher": return ItemType.Exterminator;
                case "cargobay": return ItemType.CargoBay;
                case "CLOAK": return ItemType.Cloak;
                case "combatbot": return ItemType.CombatBot;
                case "tradebot": return ItemType.TradeBot;
                case "dronecontroller": return ItemType.DroneController;
                case "SCOOP": return ItemType.Scoop;
                case "shipupgrade": return ItemType.ShipUpgrade;
                case "untangler": return ItemType.Untangler;
            }

            return ItemType.Augmenter;
        }

        public static Quality QualityFromString(string str)
        {
            switch (str)
            {
                case "common": return Quality.Common;
                case "uncommon": return Quality.Uncommon;
                case "rare": return Quality.Rare;
                case "exotic": return Quality.Exotic;
                case "artifact": return Quality.Artifact;
                case "junk": return Quality.Junk;
            }

            return Quality.Common;
        }
    }
}
