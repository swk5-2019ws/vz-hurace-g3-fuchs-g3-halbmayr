using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InsertScriptGenerator.Core
{
    class Program
    {
        private static string insertScriptPath = "..\\..\\..\\..\\..\\db\\insert_script.sql";
        private static List<string> insertScript = new List<string>();

        static void Main()
        {
            var random = new Random();

            var skiers = new List<Skier>();
            var sexes = new List<Sex>();
            var countries = new List<Country>();
            var races = new List<Race>();
            var venues = new List<Venue>();
            var raceTypes = new List<RaceType>();
            var startLists = new List<StartList>();
            var startPositions = new List<StartPosition>();
            var raceStates = new List<RaceState>();
            var seasons = new List<Season>();
            var seasonPlans = new List<SeasonPlan>();
            var raceDataList = new List<RaceData>();
            var timeMeasurements = new List<TimeMeasurement>();

            var skiersJson = JObject.Parse(File.ReadAllText("Resources/data.json")).Children().Children().Children();
            var racesJson = JObject.Parse(File.ReadAllText("Resources/races.json")).Children().Children().Children();

            var raceDescriptionArr = new string[]
            {
                "Sondre Norheim was the champion of the first downhill skiing competition, reportedly held in Oslo, Norway in 1868. Two to three decades later, the sport spread to the rest of Europe and the U.S. The first slalom ski competition occurred in Mürren, Switzerland in 1922.",
                "Norwegian legend Sondre Norheim first began the trend of skis with curved sides, bindings with stiff heel bands made of willow, and the slalom turn style.",
                "Skiing was an integral part of transportation in colder countries for thousands of years. In the late 19th century skiing converted from a method of transportation to a competitive and recreational sport.",
                "The Norwegian army held skill competitions involving skiing down slopes, around trees and obstacles while shooting. The birth of modern alpine skiing is often dated to the 1850s.",
                "The ancient origins of skiing can be traced back to prehistoric times in Russia, Finland, Sweden and Norway where varying sizes and shapes of wooden planks were preserved in peat bogs. Skis were first invented to cross wetlands and marshes in the winter when they froze over."
            };

            raceStates.Add(new RaceState()
            {
                Id = raceStates.Count,
                Label = "Abgeschlossen"
            });
            raceStates.Add(new RaceState()
            {
                Id = raceStates.Count,
                Label = "Disqualifiziert"
            });
            raceStates.Add(new RaceState()
            {
                Id = raceStates.Count,
                Label = "NichtAbgeschlossen"
            });
            raceStates.Add(new RaceState()
            {
                Id = raceStates.Count,
                Label = "Laufend"
            });
            raceStates.Add(new RaceState()
            {
                Id = raceStates.Count,
                Label = "Startbereit"
            });

            foreach (var currentSkier in skiersJson)
            {
                var currentCountryLabel = currentSkier.Value<string>("country");
                if (countries.Any(c => c.Name == currentCountryLabel))
                {
                    countries.Add(new Country()
                    {
                        Id = countries.Count,
                        Name = currentCountryLabel
                    });
                }
            }

            foreach (var currentRace in racesJson)
            {
                var currentPlace = currentRace.Value<JObject>("place");
                var currentCountryLabel = currentPlace.Value<string>("country");

                var currentVenueLabel = currentPlace.Value<string>("place");
                if (venues.Any(v => v.Name == currentVenueLabel))
                {
                    venues.Add(new Venue()
                    {
                        Id = venues.Count,
                        CountryId = countries.FirstOrDefault(c => c.Name == currentCountryLabel).Id,
                        Name = currentVenueLabel
                    });
                }

                var currentRaceType = currentRace.Value<string>("discipline");

                //localize english to german
                if (currentRaceType == "Giant Slalom")
                    currentRaceType = "Riesentorlauf";

                bool raceTypeIsNeeded = currentRaceType == "Slalom" || currentRaceType == "Riesentorlauf";

                if (currentRaceType != null && raceTypeIsNeeded)
                {
                    int nextRaceId = races.Count;

                    if (raceTypes.FirstOrDefault(rt => rt.Label == currentRaceType) == null)
                    {
                        raceTypes.Add(new RaceType()
                        {
                            Id = raceTypes.Count,
                            Label = currentRaceType
                        });
                    }

                    int firstStartListId = startLists.Count;
                    startLists.Add(new StartList()
                    {
                        Id = firstStartListId
                    });

                    int secondStartListId = startLists.Count;
                    startLists.Add(new StartList()
                    {
                        Id = secondStartListId
                    });

                    races.Add(new Race()
                    {
                        Id = nextRaceId,
                        Date = DateTime.Parse(currentRace.Value<string>("date")),
                        Description = raceDescriptionArr[random.Next(0, raceDescriptionArr.Length)],
                        NumberOfSensors = random.Next(4, 7),
                        FirstStartListId = firstStartListId,
                        SecondStartListId = secondStartListId,
                        RaceTypeId = raceTypes.FirstOrDefault(rt => rt.Label == currentRaceType).Id,
                        VenueId = venues.FirstOrDefault(v => v.Name == currentVenueLabel).Id
                    });
                }
            }

            foreach (var currentSkier in skiersJson)
            {
                var currentSexLabel = currentSkier.Value<string>("gender");
                
                //localize english to german
                if (currentSexLabel == "Female")
                    currentSexLabel = "Weiblich";
                else if (currentSexLabel == "Male")
                    currentSexLabel = "Männlich";
                
                if (sexes.FirstOrDefault(s => s.Label == currentSexLabel) == null)
                {
                    sexes.Add(new Sex()
                    {
                        Id = sexes.Count,
                        Label = currentSexLabel
                    });
                }

                var currentCountryLabel = currentSkier.Value<string>("country");

                var dobString = currentSkier.Value<string>("birthDate");
                if (!dobString.Contains('-'))
                {
                    dobString = "01-01-" + dobString;
                }

                skiers.Add(new Skier()
                {
                    Id = skiers.Count,
                    FirstName = currentSkier.Value<string>("firstName"),
                    LastName = currentSkier.Value<string>("lastName"),
                    DateOfBirth = DateTime.Parse(dobString),
                    ImageUrl = currentSkier.Value<string>("avatarUrl"),
                    SexId = sexes.FirstOrDefault(sex => sex.Label == currentSexLabel).Id,
                    CountryId = countries.FirstOrDefault(c => c.Name == currentCountryLabel).Id
                });
            }

            int startPositionId = 0;
            foreach (var currentStartList in startLists)
            {
                int positionCounter = 1;
                int participatingSkierCount = random.Next(11) + 45;
                foreach (var participatingSkier in skiers.OrderBy(s => random.Next()).Take(participatingSkierCount))
                {
                    startPositions.Add(new StartPosition()
                    {
                        Id = startPositionId++,
                        StartListId = currentStartList.Id,
                        SkierId = participatingSkier.Id,
                        Position = positionCounter++
                    });
                }
            }

            races.Select(r => r.Date.Year)
                .Distinct()
                .ToList()
                .ForEach(y => seasons.Add(new Season()
                {
                    Id = seasons.Count,
                    Name = $"Jährliche Saison {y}",
                    StartDate = new DateTime(y, 1, 1),
                    EndDate = new DateTime(y, 12, 31)
                }));

            int seasonPlanIdCounter = 0;
            foreach (var currentSeason in seasons)
            {
                foreach (var currentVenue in venues)
                {
                    seasonPlans.Add(new SeasonPlan()
                    {
                        Id = seasonPlanIdCounter++,
                        SeasonId = currentSeason.Id,
                        VenueId = currentVenue.Id
                    });
                }
            }

            foreach (var currentStartPosition in startPositions)
            {
                int currentRaceId = races
                    .FirstOrDefault(r => r.FirstStartListId == currentStartPosition.StartListId
                                      || r.SecondStartListId == currentStartPosition.StartListId)
                    .Id;

                int raceStateId;
                double raceStateIdentifier = random.NextDouble();
                if (0 <= raceStateIdentifier && raceStateIdentifier < 0.9)
                {
                    raceStateId = raceStates.FirstOrDefault(rs => rs.Label == "Abgeschlossen").Id;
                }
                else if (0.9 <= raceStateIdentifier && raceStateIdentifier < 0.95)
                {
                    raceStateId = raceStates.FirstOrDefault(rs => rs.Label == "Disqualifiziert").Id;
                }
                else if (0.95 <= raceStateIdentifier && raceStateIdentifier < 1.0)
                {
                    raceStateId = raceStates.FirstOrDefault(rs => rs.Label == "NichtAbgeschlossen").Id;
                }
                else
                {
                    throw new Exception($"{nameof(raceStateIdentifier)} out of bounds!");
                }

                int nextRaceDataId = raceDataList.Count;
                raceDataList.Add(new RaceData()
                {
                    Id = nextRaceDataId,
                    StartListId = currentStartPosition.StartListId,
                    SkierId = currentStartPosition.SkierId,
                    RaceStateId = raceStateId
                });

                for (int sensorId = 0; sensorId < races.FirstOrDefault(r => r.Id == currentRaceId).NumberOfSensors; sensorId++)
                {
                    timeMeasurements.Add(new TimeMeasurement()
                    {
                        Id = timeMeasurements.Count,
                        RaceDataId = nextRaceDataId,
                        SensorId = sensorId,
                        Measurement = Enumerable.Range(0, sensorId + 1)
                            .ToList()
                            .Select(i => random.Next(25,36))
                            .Sum() * 1000
                    });
                }
            }

            AddToInsertScript(startLists);
            AddToInsertScript(raceTypes);
            AddToInsertScript(sexes);
            AddToInsertScript(countries);
            AddToInsertScript(skiers);
            AddToInsertScript(startPositions);
            AddToInsertScript(seasons);
            AddToInsertScript(venues);
            AddToInsertScript(seasonPlans);
            AddToInsertScript(races);
            AddToInsertScript(raceStates);
            AddToInsertScript(raceDataList);
            AddToInsertScript(timeMeasurements);

            using (var streamWriter = new StreamWriter(insertScriptPath))
            {
                foreach (var line in insertScript)
                {
                    streamWriter.WriteLine(line);
                }
            }
        }

        static void AddToInsertScript<T>(List<T> collection, bool enableIdentityInsert = true)
        {
            bool idPropExisting = typeof(T).GetProperties().FirstOrDefault(p => p.Name == "Id") != null;

            insertScript.Add("------------------------------------------------------------------------");
            insertScript.Add($"-- {typeof(T).Name.ToUpper()} DATA");
            insertScript.Add("------------------------------------------------------------------------");
            if (idPropExisting)
            {
                insertScript.Add("-- activate identity insert");
                insertScript.Add($"SET IDENTITY_INSERT [Hurace].[{typeof(T).Name}] ON;");
            }
            insertScript.Add("-- insert data");
            collection.ForEach(item => insertScript.Add(item.ToString()));
            if (idPropExisting)
            {
                insertScript.Add("-- deactivate identity insert");
                insertScript.Add($"SET IDENTITY_INSERT [Hurace].[{typeof(T).Name}] OFF;");
            }
            insertScript.Add("-- commit changes");
            insertScript.Add("GO");
            insertScript.Add("------------------------------------------------------------------------");
            insertScript.Add("");
            insertScript.Add("");
        }
    }
}
