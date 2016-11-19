using System;
using System.Collections.Generic;
using System.Linq;
using Realms;

namespace Mandrilkalender
{
	public class GateService
	{
		public const int MAX_GATES = 25;
		public const int MAX_ROWS_COLUMNS = 5;

		private Realm realm;

		public GateService()
		{
			var config = new RealmConfiguration()
			{
				SchemaVersion = 2,
				MigrationCallback = (migration, oldSchemaVersion) =>
				{
					var newGates = migration.NewRealm.All<Gate>();
					var oldGates = migration.OldRealm.All("Gate");

					for (var i = 0; i < newGates.Count(); i++)
					{
						var oldGate = oldGates.ElementAt(i);
						var newGate = newGates.ElementAt(i);
					
						// Handle value updates for new fields
					}
				}
			};
			realm = Realm.GetInstance(config);
		}

		public IQueryable<Gate> GetGates() 
		{
			var gates = realm.All<Gate>();
			if (gates.Count() > 0)
			{
				return gates;
			}

			return InitializeGates().AsQueryable();
		}

		private IList<Gate> InitializeGates()
		{
			var descriptions = new List<string>()
			{
				"Casper får besøg af Asger Debono, som lægger lækkerier ud på foderbrættet.",
				"Casper får besøg af Jørgen Ingemann, der får besøg af Julemanden som den første hvert år.",
				"Endnu engang besøg af Asger Debono der ved hvad der rører sig inden for halm.",
				"Casper får besøg af Chacha Jublenn der ved alt om post.",
				"Casper får besøg af Hr. Vellemand, der aldrig har fået lige det han ønskede sig.",
				"Casper får besøg af Cosy Joe, en af de 8000 vise mænd.",
				"Casper får besøg af Doby Slinger, som aldrig bliver fortalt noget.",
				"Casper får besøg af Thor Vitter Rynkemås, der fortæller hvordan man tilbereder en juleand.",
				"Et herligt gensyn med Thor Vitter Rynkemås, der denne gang viser, hvordan man fjerner nougatpletter.",
				"Casper får besøg af Stofa Donnington der har røven fuld af penge.",
				"Casper får besøg af Julian Tisange, som mener at de danske juletraditioner bør ændres.",
				"Stofa Donnington kommer igen på besøg, denne gang fortæller han om juletraditioner.",
				"Timothy Birthebæk kigger forbi, og fortæller om det at være overvægtig, spise for to, og synge i en trio med én anden.",
				"Julian kommer endnu gang på besøg for at snakke mere om ændring af juletraditioner.",
				"Casper får besøg af Miffer Hansepajuk, der fortæller om sin markedsføring af en ny adventskrans.",
				"Endnu et gensyn med Julian til en snak om juletraditioner.",
				"I dag er der historie oplæsning.",
				"Casper får besøg af Herbert Røffel, der fortæller om livet som kravlenisse.",
				"Casper fortæller, hvordan man kan forberede sig til jul.",
				"Casper fortæller om alle de dejlige ting, som man kan lave med vat.",
				"Casper fortæller glöggens historie.",
				"Casper fortæller om de nye øl, der kommer til jul næste år.",
				"Casper fortæller, hvordan man sender julekort.",
				"Casper får besøg af englen Fergie.",
				"Casper forsøger at oprette forbindelse til rumbukken Fyffy."
			};

			var videoIds = new List<string>()
			{
				"whh2aljch8w",
				"fX3vzQKRdps",
				"Cc3aivx_uNU",
				"oG8oFVE7Gpo",
				"WKO3FP-SLzY",
				"P5hCu2JTQcs",
				"XlYg3LWzzIo",
				"xNC5Le-ZPjo",
				"XUfNMgBBQzk",
				"ZWjsFCGFZVk",
				"JPxhyGNhOsY",
				"LWoOeqBRo1g",
				"3QA55AJjMJ0",
				"FWSucptTJTc",
				"j89fnmYings",
				"Cg-GvBaCSj0",
				"XcvuLTcxZ8g",
				"Q0AmcPbHqEA",
				"nqfmM-aK8vY",
				"i5TTySKyWxg",
				"jz59tiZTe1E",
				"We5Si8jn3eI",
				"0uARiZkTGSA",
				"8kMTRDXxBVM",
				"OH_c3VR3O6M"
			};

			var gates = new List<Gate>();

			realm.Write(() =>
			{
				for (var i = 1; i <= MAX_GATES; i++)
				{
					var gate = realm.CreateObject<Gate>();
					gate.Number = i;
					gate.Description = descriptions[i - 1];
					gate.VideoId = videoIds[i - 1];
					gates.Add(gate);
				}
				gates.Shuffle();

				var k = 0;
				for (var i = 0; i < MAX_ROWS_COLUMNS; i++)
				{
					for (var j = 0; j < MAX_ROWS_COLUMNS; j++)
					{
						gates[k].X = i;
						gates[k].Y = j;
						k++;
					}
				}
			});

			return gates;
		}

		public void OpenGate(Gate gate)
		{
			using (var trans = realm.BeginWrite())
			{
				gate.Opened = true;
				trans.Commit();
			}
		}

		public static string GetImageName(Gate gate)
		{
			var ch = 'a';

			var newCh = (char)(ch + (gate.Number - 1));

			return $"{newCh}.png";
		}
	}
}
