using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EU4Importer
{
	public class Program
	{
		private string[] Lines = new string[2] { "start = {		possible_policy = 1		diplomatic_reputation = 2	}		bonus = {		administrative_efficiency = 0.05	}		trigger = {		tag = HLR	}	free = yes		hlr_imperial_throne = {		legitimacy = 1.5 	}	hlr_kaiserliche_armee = {		land_morale = 0.15	}	hlr_imperial_diplomacy = {		improve_relation_modifier = 0.33	}	hlr_centralised_empire = {		global_tax_modifier = 0.2	}	hlr_roman_heritage = {		core_creation = -0.2	}	hlr_adopting_the_goosestep = {		discipline = 0.05	}	hlr_onwards_and_upwards = {		governing_capacity_modifier = 0.1	}", "start = {		legitimacy = 1		devotion = 1		republican_tradition = 0.3		infantry_power = 0.15	}		bonus = {		diplomatic_upkeep = 1	}		trigger = {		tag = GER	}	free = yes		ger_german_confederation = {		global_unrest = -2	}	ger_german_universities = {		technology_cost = -0.1	}	ger_reichsheer = {		discipline = 0.05	}	ger_new_hanseatics = {		trade_efficiency = 0.10	}	ger_junkers = {		army_tradition = 0.5	}	ger_reichstag = {		administrative_efficiency = 0.05	}	ger_eisen_und_kohle = {		global_trade_goods_size_modifier = 0.2	}" };
		private static Program program = new();
		public List<OuterEntity> OuterEntities = new();
		public static void Main()
		{
			program.Run();
		}

		private void Run()
		{
			OuterEntity outerEntity = new();
			outerEntity.Name = "HLR_ideas";
			outerEntity.UnprocessedInnerEntities.Add(Lines[0]);
			OuterEntities.Add(outerEntity);
			outerEntity = new();
			outerEntity.Name = "GER_ideas";
			outerEntity.UnprocessedInnerEntities.Add(Lines[1]);
			OuterEntities.Add(outerEntity);
			Process.ProcessInnerEntities(this);
			Test.PrintOuterEntities(this);
		}
	}

	public class Process
	{
		public static void ProcessInnerEntities(Program program)
		{
			foreach (OuterEntity outerEntity in program.OuterEntities)
			{
				foreach (string unprocessedInnerEntity in outerEntity.UnprocessedInnerEntities)
				{
					InnerEntity innerEntity = ProcessInnerEntity(unprocessedInnerEntity, new InnerEntity());
					outerEntity.InnerEntities.Add(innerEntity);
				}

				outerEntity.UnprocessedInnerEntities.Clear();
			}
		}

		private static InnerEntity ProcessInnerEntity(string UnprocessedInnerEntity, InnerEntity? ProcessedInnerEntity, bool WaitingForValue = false )
		{
			InnerEntity? parent = ProcessedInnerEntity;
			InnerEntity processedInnerEntity = new();
			int currentPos = 0;
			int lastPos = 0;
			int brackets = 0;
			bool inBrackets = false;
			bool waitingForValue = WaitingForValue;
			StringBuilder sb = new StringBuilder();
			foreach (char c in UnprocessedInnerEntity)
			{
				if (c == '{') //Brackets opening
				{
					lastPos = currentPos + 1; //NEW
					inBrackets = true;
					brackets++;
				}
				else if (c == '}') //Brackets closing
				{
					brackets--;
				}
				else if (c == '=') //Name behind, Value or NestedEntity in front
				{
					for (int i = lastPos; i <= currentPos; i++)
					{
						sb.Append(UnprocessedInnerEntity[i]);
					}

					processedInnerEntity.Name = sb.ToString();
					sb.Clear();
					lastPos = currentPos + 1;
					waitingForValue = true;
					if (inBrackets) //In a nested bracket, find InnerEntity
					{
						processedInnerEntity.InnerEntities.Add(ProcessInnerEntity(UnprocessedInnerEntity.Substring(currentPos + 1),processedInnerEntity, waitingForValue)); //Search everything in front
					}
				}
				else if (c < 47 | c > 122) //Not text NEW
				{
					if (waitingForValue) //After =
					{
						for (int i = lastPos; i <= currentPos; i++)
						{
							if (c > 47 & c < 122) //Text NEW
							{
								waitingForValue = false;
								sb.Append(UnprocessedInnerEntity[i]);
							}
						}

						if (!waitingForValue) //Has value
						{
							if(parent.Name!=null & parent.Value == null)//Parent needs value
                            {
								parent.Value = sb.ToString();//How do I go there??
								sb.Clear();
                            }
							processedInnerEntity.Value = sb.ToString();
							sb.Clear();
						}
					}
				}

				if (brackets == 0 && inBrackets) //Brackets closed
				{
					inBrackets = false;
					currentPos++;
					break;
				}

				currentPos++;
			}

			return processedInnerEntity;
		}
	}

	public class InnerEntity
	{
		public List<string> UnprocessedInnerEntities = new();
		public string? Name;
		public string? Value;
		public List<InnerEntity> InnerEntities = new();
		public void Clear()
		{
			this.Name = null;
			this.Value = null;
			this.InnerEntities = new();
			this.UnprocessedInnerEntities = new();
		}

		public override string ToString()
		{
			StringBuilder sb = new();
			sb.AppendLine(Name);
			if (InnerEntities.Count > 0)
			{
				foreach (InnerEntity entity in InnerEntities)
				{
					sb.Append(entity.ToString());
				}
			}
			else
			{
				sb.AppendLine(Value);
			}

			foreach (string String in UnprocessedInnerEntities)
			{
				sb.AppendLine(String);
			}

			return sb.ToString();
		}
	}

	public class OuterEntity
	{
		public List<string> UnprocessedInnerEntities = new();
		public string? Name = "";
		public List<InnerEntity> InnerEntities = new();
		public void Clear()
		{
			this.UnprocessedInnerEntities.Clear();
			this.InnerEntities.Clear();
			this.Name = null;
		}

		public override string ToString()
		{
			StringBuilder sb = new();
			sb.Append(Name);
			foreach (InnerEntity entity in InnerEntities)
			{
				sb.Append(entity.ToString());
			}

			foreach (string entity in UnprocessedInnerEntities)
			{
				sb.Append(entity);
			}

			return sb.ToString();
		}
	}

	public class Test
	{
		public static void PrintOuterEntities(Program program)
		{
			List<string> strings = new();
			foreach (OuterEntity outerEntity in program.OuterEntities)
			{
				strings.Add(outerEntity.ToString());
			}

			foreach (string String in strings)
			{
				Console.Write(String);
			}
		}
	}
}