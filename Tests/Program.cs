using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EU4Importer;

namespace EU4Importer
{
	public class Program
	{
		private static string Input = "start = {possible_policy = 1		diplomatic_reputation = 2	}		bonus = {		administrative_efficiency = 0.05	}		trigger = {		tag = HLR	}	free = yes		hlr_imperial_throne = {		legitimacy = 1.5 	}	hlr_kaiserliche_armee = {		land_morale = 0.15	}	hlr_imperial_diplomacy = {		improve_relation_modifier = 0.33	}	hlr_centralised_empire = {		global_tax_modifier = 0.2	}	hlr_roman_heritage = {		core_creation = -0.2	}	hlr_adopting_the_goosestep = {		discipline = 0.05	}	hlr_onwards_and_upwards = {		governing_capacity_modifier = 0.1	} start = {		legitimacy = 1		devotion = 1		republican_tradition = 0.3		infantry_power = 0.15	}		bonus = {		diplomatic_upkeep = 1	}		trigger = {		tag = GER	}	free = yes		ger_german_confederation = {		global_unrest = -2	}	ger_german_universities = {		technology_cost = -0.1	}	ger_reichsheer = {		discipline = 0.05	}	ger_new_hanseatics = {		trade_efficiency = 0.10	}	ger_junkers = {		army_tradition = 0.5	}	ger_reichstag = {		administrative_efficiency = 0.05	}	ger_eisen_und_kohle = {		global_trade_goods_size_modifier = 0.2	}";
		private static string parentName;
		private static Stack<Entity> entities = new();
		private static Stack<List<Entity>> collections = new();
		private static int lastChar = 0;
		private static int currentChar = 0;
		private static bool running = true;
		private static void Main()
        {
			Entity entity = new();
			entity.Name = "Test";
			entity.OpenCollection = true;
			List<Entity> collection = new();
			collections.Push(collection);
			entities.Push(entity);
			while (running)
			{
				lookAtCharacters();
			}
        }
		private static void lookAtCharacters()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			switch (Input[currentChar])
			{
				case '=':
					Console.WriteLine("Found = ");
					newEntity();
					break;
				case '{':
					Console.WriteLine("Found { ");
					newCollection();
					break;
				case ' ':
					Console.WriteLine("Found space");
					checkValue();
					break;
				case '}':
					Console.WriteLine("Found }");
					endCollection();
					break;
			}
			currentChar++;
		}
		private static void newCollection()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("newCollection");
			Entity entity = entities.Pop();
			Console.WriteLine("entities.Pop");
			Entity parentEntity = entities.Pop();
			Console.WriteLine("parentEntity.Pop");
			parentEntity.OpenCollection = true;
			parentEntity.Entities.Add(entity);
			entities.Push(parentEntity);
			collections.Push(parentEntity.Entities);
			Console.WriteLine("Pushed collection");
		}
		private static string past()
        {
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("Past requested : ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(Input.Substring(lastChar, currentChar));
			return Input.Substring(lastChar,currentChar-1);
        }
		private static void newEntity()
        {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("newEntity");
			Entity entity = new();
			entity.Name = past();
			lastChar = currentChar +1 ;
			entity.NeedValue = true;
			entities.Push(entity);
			Console.WriteLine("Pushed Entity");
		}
		private static void checkValue()
        {
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("checkValue");
			if (entities.Peek().NeedValue)
            {
				Console.WriteLine("Needed value");
				if (pastContainsValue())
                {
					Console.WriteLine("Past contained value");
					Entity entity = entities.Pop();
					Console.WriteLine("entities.Pop");
					entity.Value = past();
					entity.NeedValue = false;
					entities.Push(entity);
					lastChar = currentChar + 1;
					Console.WriteLine("Pushed entity");
					if (inCollection())
                    {
						Console.WriteLine("inCollection");
						addCollection();
                    }
					Console.WriteLine("Value added");
				}
            }
			Console.WriteLine("Did not need value");

		}
		private static void endCollection()
        {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("endCollection");
			collections.Pop();
			Console.WriteLine("collections.Pop");
			Entity entity = entities.Pop();
			Console.WriteLine("entities.Pop");
			if (entity.Name == parentName)
            {
				Console.WriteLine("Stopped");
				running = false;
            }
            else
            {
				Console.WriteLine("Closed collection");
				entity.OpenCollection = false;
            }
			entities.Push(entity);
		}
		private static void addCollection()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("addCollection");
			List<Entity> collection = collections.Pop();
			Console.WriteLine("collections.Pop");
			Entity entity = entities.Pop();
			Console.WriteLine("entities.Pop");
			collection.Add(entity);
			Entity parentEntity = entities.Pop();
			Console.WriteLine("entities.Pop");
			parentEntity.Entities = collection;
			collections.Push(collection);
			entities.Push(parentEntity);
			Console.WriteLine("Added Collection");
		}
		private static bool pastContainsValue()
        {
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("pastContainsValue?");
			string check = past();
			for(int i =0;i<check.Length;i++)
            {
				if (check[i] > 47 & check[i] < 123)
				{
					Console.WriteLine("true : ");
					Console.WriteLine(check[i]);
					return true;
				}
			}
			Console.WriteLine("false");
			return false;
        }
		private static bool inCollection()
        {
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("inCollection?");
			Entity entity = entities.Pop();
			Console.WriteLine("entities.Pop");
			Entity ParentEntity = entities.Peek();
			entities.Push(entity);
            if (ParentEntity.OpenCollection)
            {
				Console.WriteLine("true");
				return true;
            }
            else
            {
				Console.WriteLine("false");
				return false;
			}
        }
	}


	public class Entity
    {
		public string Name;
		public string? Value;
		public List<Entity> Entities = new();
		public bool OpenCollection = false;
		public bool NeedValue = false;

	}
}