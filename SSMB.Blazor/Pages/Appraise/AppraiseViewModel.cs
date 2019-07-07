namespace SSMB.Blazor.Pages.Appraise
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using ViewServices;

    public class AppraiseViewModel : IAppraiseViewModel
    {
        private readonly IItemsService itemsService;
        public string AppraiseText { get; set; }

        private readonly Subject<Unit> appraisalCompleted = new Subject<Unit>();
        private ItemAppraisal[] appraisals;
        public IObservable<Unit> WhenAppraisalCompleted => this.appraisalCompleted.AsObservable();

        public ItemAppraisal[] Appraisals
        {
            get => this.appraisals;
            set
            {
                this.appraisals = value; 
                this.appraisalCompleted.OnNext(Unit.Default);
            }
        }

        public AppraiseViewModel(IItemsService itemsService)
        {
            this.itemsService = itemsService;
            this.AppraiseText = @"2	A Billion
12	Bunny Chocolatier
4	Chronoton Radiation
2	Dark Cell
2	Dark Jungle Key
3	Dark Scorch's Remains
1	Deep Jungle Key
6	Domesticated Space Rat
1	Easter Mention
14	Egg Chunk
3	Egg Hunting Ticket
4	Encrypted Transcoder Data Transmitter
251	Energy Globule
37	Gloopy Organic Webbing
5	Gold Dubloon
11	Goldenboy Coin Specialized Trade Goods
3	Goldenboy Nugget Specialized Trade Goods
2	Goldenboy Pyrite Specialized Trade Goods
1	Goldenboy Statue Specialized Trade Goods
2	Gunky Organic Webbing
7	Holy Rat Flesh
90	Honey
2	Indium
5	Infernal Cell
2	John's Token
7	Jungle Armada Crest
6	Kalthi Depths Wreckage
26	Kalthi Essence
103	Kalthi Manufacturing Parts
3	Lion Heart Key
146	Master Pirate Crest
1	Monochrome Hue Specialized Trade Goods
5	Monochrome Tint Specialized Trade Goods
8	NCC-Bulk Specialized Trade Goods
1	Ngorongoro key
138	Olduvai Gorge key
1	Oversized Rosburst Specialized Trade Goods
1	Oversized Zebucart Specialized Trade Goods
10	Pax Astralogica Specialized Trade Goods
6	Paxian Medallion
3	Peasants
2	Permission of the Prophet
25	Pirate Crest
1	Pirated Lyceum Research Module V
2	Pirated Lyceum Research Module VI
27	Primal Jungle Powercore
18	Primal Lion Powercore
24	Primal Panther Powercore
23	Primal Rhino Powercore
26	Primal Zebra Powercore
1	Pumpkin Shard
2	Pumpkins
269	Pyritic Radiation
52	Rations
1	Rhenium
13	Salt
28	Scorched Trader Badge
10	Scorched Trader Insignia
3	Scorched Trader Signet
46	Serengeti Nebula key
8	Shard Dust
1	Souls
49	Space Oats
1	Spoiled Rations
4	Stabilized Thalaron Particles
40	Sticky Organic Webbing
2	Trader Emblem
9	Trader Signet
7	Unclean Imperial Seal
9	Urk' Qokujiii Qa'ik
4	Urk' Ukukuu Qu
16	Urko Qu
10	Urko Quu
107	Veteran Pirate Crest
43	Wattage Specialized Trade Goods
145	Workers
1	Zaphragi Research Diploma
1	Zebucart Specialized Trade Goods
1	Zombies
2	Bindomite
66	Enriched Nuclear Material
333	Fool's Gold
60	Jelly Beans
2133	Metals
2754	Nanosilicon
2112	Nuclear Waste
75	Petroleum
6174	Phased Metals
19	Promethium
361	Quartz Crystals
2499	Silicon
139	Silver
6	Solar Prisms
66	Tin
92	Titanium
4	Traginium
3	Umbra
63	Vis
37	Warpwnium
2	Dementium
61	Laconia Sheet
52	Shrinkonium
65	Titanium Sheet
10	Anaconda Scales
1	Aqua Essence
1	Azure Essence
3	Carcass
1	Chrome Knobs
5	Conclusion Icicle
150	F Cell
23	Giant Goblets
32	Mad Scientist's Brain
33	Mad Scientist's Other Brain
2	Tanza Essence
29	Xeno Energy Remains
129	Xeno Shield Remains
21	Ace in a Pill I Crate
1	Alien Bacteria Canister
470	Alien Husk
2	Ambush Crate
33	Ammo Crate - Large AP Type III
2	Ammo Crate - Large Bunkerbuster Torpedo
3	Ammo Crate - Large Kinetic Type III
120	Ammo Crate - Large Paxian Missile
8	Ammo Crate - Large Surgical Strike Type III
70	Ammo Crate - Medium Paxian Missile
70	Ammo Crate - Small Paxian Missile
5	Ammo Crate - Tiger Missile
19	Ammo Crate - Zaphragi Cheetah Missile
2	Black Pearl Mobile Drone Crate
43	Crystallized Enriched Nuclear Material
19	Eggbox Crate
1	Enchromas Crate
16	Fractured Aveksaka Carapace
1	Hermes Delirium Tremens Crate
2	Hermes Power Punch Crate
1	Hot Pumpkin Crate
3	Jury Rigging of Hephaestus Crate
1	Kidd's Volatile Wrath Crate
1	Large Station Gear Token Crate
1	Lasting Resources Crate
4	Oilheart's Best Afterburner Crate
7	Pallet of Eggbox Crates
1	Promethium Matrix Crystal
1	Rage of Ares Crate
20	Shadow Shield Tweak Crate
1	Strength of Poseidon Crate
1	Tardis in a Box
68	Tektite
4	Wrath of Zeus Crate
2	XFH1231K Nano Factory
3193	Alien Husk Fragments
736	Aveksaka Plasma
1324	Irradiated Tumors
1	[Overclocked|Forceful|Buffered]Ares Sapper
1	Faranji Gatling Laser X
1	Paxian Purifier
1	Poseidon Current
4	Suicidal Tendency
1	[Miniaturized]Suicidal Tendency
1	[Reinforced]Suicidal Tendency
1	[Buffered]The Emperor's Drone Controller
1	[Dynamic|Evil|Buffered]Unt Faranji Gatling X
1	[Miniaturized|Composite|Dynamic|Radioactive]Unt Faranji Gatling X
1	[Composite|Scoped|Radioactive|Superconducting|Overclocked]Unt Faranji Gatling X
1	[Composite|Scoped|Forceful|Reinforced]Unt Faranji Gatling X
1	[Miniaturized|Buffered]Unt Faranji Gatling X
1	[Dynamic|Super Intelligent]Unt Faranji Gatling X
1	[Superconducting]Unt Faranji Gatling X
2	Ukukuu Qu
3	Urqa'ka
9	Uu Quu
1	[Buffered]Abstructor
1	Bear Maul
1	[Composite]Capital Armageddon Laser Core
1	[Scoped]Capital Armageddon Laser Core
2	Capital Armageddon Laser Core
1	[Miniaturized|Superconducting]Capital Armageddon Laser Prototype
1	[Intelligent]Capital Armageddon Laser Prototype
1	Capital Armageddon Laser Prototype
1	[Superconducting]Capital Armageddon Laser Prototype
1	Kikale Mzungu Warfare
1	[Miniaturized]Red Photon Asteroid Drill
1	[Composite|Radioactive|Evil|Forceful]Red Photon Asteroid Drill
1	[Evil]Harrier Talons
2	High Powered Knocker
1	[Scoped]Zebra Hooves Torpedoe
1	[Evil]Neutron Bomb
5	Neutron Bomb
1	[Dynamic]Neutron Bomb
1	[Extended|Reinforced]Neutron Bomb
1	[Miniaturized|Evil]Neutron Bomb
1	Prismatic Conversion
2	ZigZagger
1	Apprentice's Wand
1	[Evil]Poseidon Shield Transference
1	Poseidon Shield Transference
1	[Buffered]Poseidon Shield Transference
1	Eclipse Blockade
1	[Amorphous]Hephaestus Shield
1	Paxian Sovereign Explorer
4	Pretender Ares Shield
1	Titan Shield
1	Twilight Blockade
3	Zeus Shield
1	[Composite]Zeus Shield
1	[Docktastic|Overclocked]Zeus Shield
1	Mzungu Liquid
1	[Sleek|Gyroscopic|Super Intelligent]Ares War Horses
1	[Miniaturized|Amorphous|Sleek]Ares War Horses
1	[Composite]Ares War Horses
1	[Radioactive]Ares War Horses
1	[Composite|Docktastic|Rewired|Forceful]Ares War Horses
1	[Gyroscopic]Ares War Horses
1	[Sleek]Ares War Horses
1	Ayudhin Puccha
2	Hermes Engine
1	Oversized Moral Emphasis
3	Rhino Charge
1	Titan Engine
1	Zeus Engine
50	Bhu Dve Epiphany
50	Bhu Ekam Epiphany
150	Bhu Pajca Epiphany
50	Bhu Tra Epiphany
1	Black Box Report
3	Egg Hunting Grounds map and rules
1	Narwhal Charisma
2	Paxian Holocube II
2	Paxian Holocube III
2	Paxian Holocube IV
1	Salt Assault's Christmas Letter
1	Ship Dry-dock Module
1	Skilled Ricochet Device
1	Skilled Ricochet Device
1	Small Chihuahua Light Fighter Generator
3	Small Death of First Born Fighter Generator
26	Small Fly Fighter Generator
4	Small Locust Fighter Generator
5	The Crucible by Jon Zebedi
1	Exc. Electric Drone Controller
1	Witch's Broomstick
1	Commander's Ascension
1	Commander's Bodacious Bravado
4	Gamma Emitter
227	Advanced Blocker
95	Apollo's Rebirth
133	Apollo's Warmth
28	Backstab
8	Blocker
1	Candy Mints
2	Chocolate Crispe Cake
82	Double Sub Space Anchor
2	Face Hugger's Acid Blood
2	Gigantic Chocolate Egg
1	Insto Shield Tweak 2
3	Ion Caipirinha
2	Ion Rebirth
6	Ion Salvation
1	Kotonjata
2	Large Chocolate Egg
2	Medium Chocolate Egg
77	Mega Warmth
3	Oilheart's Afterburner
380	Oilheart's Best Afterburner
6	Oilheart's Better Afterburner
1	Power Doughnut
436	Power Punch GT
176	Power Punch Z
2	Quick Attack
12	Rapid Attack
2	Rich Christmas Pudding
5	Sacrificial Rebirth
74	Sacrificial Renewal
131	Sacrificial Salvation
2	Seasoned Pumpkin Seeds
1	Simple Blocker
2	Sitting Duck
1	Small Chocolate Egg
5	Small Power Punch
2	Sub Space Anchor
4	Super Shield Soother
60	Swifty-Spry Attack
2	Tiny Power Punch
1	Triple-Chocolate Candy Cane Kisses
15	Urchin Thorn
23	Vapid Attack
35	Warmalicious Warmth
26	Warmy Warmth
1	White Chocolate Candy Pretzels
1	Divine Hull Expansion
1	Apollo's Collector
13	Dark Intake
1	Lord's Solar Collector
1	Miniaturized Perilous Panel
1	Solar Intake
2	Energy Diffuser 2
1	Energy Diffuser Kali
5	Ultimate Hazard Dissipator
1	UrQa Uka Uzu
1	Liberty Shield Capacitor III
7	Ace Pest Control
1	Divine Overloader
1	Elephant Modifications
1	Nexus Power Cells
1	Overloader King
1	Overloader Rapid
1	Overloader Stable 4
1	Overloader Stable 5
1	Overloader Stable 6
1	Carcass Missile
1	Bulker Light Fighter
6	Chihuahua Light Fighter
7	Fly Fighter
2	Broadsword Class Launcher Mark V
1	Madman Fighter Bay
1	Primal Zebra Charger
2	Super Quartz Charger
2	Parasitic Commander
2	Parasitic Controller
2	Parasitic Director
15	Parasitic Governor
1	Electron Storm
37	Electron Tomb
1	Graviton Interruption
1	Black Box
2	Homing Beacon
1	Repair Rejigger Drone Device
1	Skilled Sniper Blind Device
1	Blue Photon 1000 Series Transwarp
1	Blue Photon Armada Transwarp
1	Blue Photon Prototype Transwarp
1	Blue Photon Prototype Transwarp
1	Blue Photon Prototype Transwarp
2	Armada Base Scanner
11	Armada Planet Scanner
6	Armada Shipscanner
1	Assayer's Scanner
1	EDVAC Intensive Prospecting Scanner
6	Inhumanly Advanced Resource Detector
1	Witch's Cackle
2	Electro Scoop
1	Maxi Vacuum Scoop
3	Scoop
1	Basic DNA Extraction Beam
1	[Buffered]Deepest Core Drilling Beam
1	Energy Transference Beam V
1	Hitchhiker Beta
4	Hitchhiker Gamma
1	Rhino Roar
1	[Composite|Dynamic|Buffered]Rhino Roar
1	Sensitive Drilling Beam
3	Thrak'hra's Denial
1	Achilles Recovery Augmenter Blueprint
1	Adamantium Extractor Blueprint
6	Adamantium Sheet Smelting Schema
1	Adamantiumized Thruster Blueprint
2	Advanced Admixium Blueprint
1	Armada Ambush Mine Blueprint
1	Armada Lifestream Blueprint
1	Capital Lipo Driver Blueprint
1	Combined Aspect of the Rhino Tablet
1	Combined Aspect of the Zebra Tablet
1	Compressed Mining Dampener Blueprint
4	Compressed Physical Dampener Blueprint
1	Good Recovery Augmenter Condenser
1	Hephaestus Machine Blueprint
2	Infinite Improbability Drive Blueprint
1	Lunarian Bank Blueprint
1	Lunarian Locator Blueprint
1	Lunarian Thruster Blueprint
4	Rovert Nanobotics Facility Blueprint
6	Rudimentary Admixium Blueprint
1	Selenium Blueprint
1	Solarian MagCannon Blueprint
2	Solarian Scanner Blueprint
1	Sup. Docking Augmenter Condenser
1	Twilight Blockade Blueprint
1	Undead Narwhal Energy Piece
4	Unidyne Core Drain Blueprint
6	Unidyne Core Dump Blueprint
1	Zeus Throne Blueprint
3	Poseidon Energy
1	[Composite|Extended]Pretender Ares Energy
1	Pretender Ares Energy
2	UrQa Broken Aku' Qa
1	Venusian Vitality
2	Zeus Energy
1	[Extended]Zeus Energy
1	[Miniaturized|Transcendental]Zeus Energy
1	Ayudhin Drsti
1	Hermes Radar
1	Poseidon Radar
1	[Radioactive|Sleek]Pretender Ares Radar
1	Pretender Ares Radar
1	Zeus Radar
4	Armada Ambush Drone
7	Armada Charge Drone
1	Armada Drone
1	Armada Inferno Drone
2	Arson Drone
1	Large Forcefield Generator
1	Urqa'ka Drone
3	Var Zazatamzu Katur
5	Ambrosia Drone
7	Qa Quuu Drone
3	Quu Drone
1	Achilles Assault Augmenter
1	Achilles Damage Augmenter
1	Aphrodite Augmenter
1	Aspect of the Rhino
1	Basic Capacity Augmenter
1	Basic Damage Augmenter
1	Basic Energy Augmenter
1	Basic Radar Augmenter
2	Basic Recovery Augmenter
1	Basic Speed Augmenter
1	Exc. Docking Augmenter
1	Good Docking Augmenter
1	Good Electric Augmenter
1	Green Augmenter
2	Hermes Swiftness Augmenter
1	Mad Scientist Augmenter
1	Minor Aggravation Augmenter
2	Minor Agility Augmenter
1	Minor Capacity Augmenter
2	Minor Damage Augmenter
1	Minor Energy Augmenter
1	Minor Fading Augmenter
1	Minor Radar Augmenter
2	Minor Speed Augmenter
1	Minor Targeting Augmenter
1	Minor Thrust Augmenter
1	Spirit of the Leopard
1	Spirit of the Monkey
2	Spirit of the Tiger
2	Std. Electric Augmenter
1	Std. Stealth Augmenter
1	Std. Traction Augmenter
1	Sup. Docking Augmenter
5	Allied Command
1	Blue Championship Racer Medal
24	Chocolate Egg
17	Earthforce Survivor
1	Keryx's Letter of Introduction
1	Micron Blue Course Key
1	Rumble Pirate Crest";
            this.OnAppraiseClick();
        }

        public void OnAppraiseClick()
        {
            var text = this.AppraiseText.Replace("\r", string.Empty);
            var lines = text.Split("\n");
            var items = new List<(string name, int count)>();
            foreach (var line in lines)
            {
                try
                {
                    var entry = line.Split("	");
                    var count = int.Parse(entry[0]);
                    var name = entry[1];
                    items.Add((name, count));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _ = Task.Run(async () => { this.Appraisals = await this.itemsService.GetAppraisal(items.ToArray()); });
        }
    }
}
