using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stations.Data;

using Stations.DataProcessor.Dto.Import;
using Stations.Models;
using Stations.Models.Enums;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedStations = JsonConvert.DeserializeObject<StationDto[]>(jsonString);

            var validStations = new List<Station>();

            foreach (var dto in deserializedStations)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                if (dto.Town == null)
                {
                    dto.Town = dto.Name;
                }

                var alreadyExists = validStations.Any(s => s.Name == dto.Name);

                if (alreadyExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var station = Mapper.Map<Station>(dto);
                validStations.Add(station);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }
            context.Stations.AddRange(validStations);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedClass = JsonConvert.DeserializeObject<SeatingClassDto[]>(jsonString);

            var validClasses = new List<SeatingClass>();

            foreach (var dto in deserializedClass)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var classAlreadyExists = validClasses
                    .Any(c => c.Name == dto.Name || c.Abbreviation == dto.Abbreviation);

                if (classAlreadyExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatingClass = Mapper.Map<SeatingClass>(dto);

                validClasses.Add(seatingClass);
                sb.AppendLine(String.Format(SuccessMessage, seatingClass.Name));
            }

            context.SeatingClasses.AddRange(validClasses);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedTrains = JsonConvert.DeserializeObject<TrainDto[]>(jsonString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var validTrains = new List<Train>();

            foreach (var dto in deserializedTrains)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var trainAleadyExists = validTrains.Any(t => t.TrainNumber == dto.TrainNumber);
                if (trainAleadyExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatsAreValid = dto.Seats.All(IsValid);
                if (!seatsAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatingClassesAreValid = dto.Seats.All(s => context.SeatingClasses
                    .Any(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation));
                if (!seatingClassesAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var type = Enum.Parse<TrainType>(dto.Type);

                var trainSeats = dto.Seats.Select(s => new TrainSeat
                {
                    SeatingClass = context.SeatingClasses.SingleOrDefault(sc => sc.Name == s.Name &&
                                                                   sc.Abbreviation == s.Abbreviation),
                    Quantity = s.Quantity.Value
                })
                .ToArray();

                var train = new Train()
                {
                    TrainNumber = dto.TrainNumber,
                    Type = type,
                    TrainSeats = trainSeats
                };

                validTrains.Add(train);
                sb.AppendLine(String.Format(SuccessMessage, dto.TrainNumber));
            }
            context.Trains.AddRange(validTrains);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedTrips = JsonConvert.DeserializeObject<TripDto[]>(jsonString, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var validTrips = new List<Trip>();

            foreach (var dto in deserializedTrips)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var train = context.Trains.SingleOrDefault(t => t.TrainNumber == dto.Train);
                var originStation = context.Stations.SingleOrDefault(s => s.Name == dto.OriginStation);
                var destStation = context.Stations.SingleOrDefault(s => s.Name == dto.DestinationStation);

                if (train == null || originStation == null || destStation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var depTime = DateTime.ParseExact(dto.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var arrivleTime = DateTime.ParseExact(dto.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                TimeSpan timeDiff;
                if (dto.TimeDifference != null)
                {
                    timeDiff = TimeSpan.ParseExact(dto.TimeDifference, "hh\\:mm", CultureInfo.InvariantCulture);
                }

                if (depTime > arrivleTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var status = Enum.Parse<TripStatus>(dto.Status);

                var trip = new Trip
                {
                    Train = train,
                    OriginStation = originStation,
                    DestinationStation = destStation,
                    DepartureTime = depTime,
                    ArrivalTime = arrivleTime,
                    Status = status,
                    TimeDifference = timeDiff
                };

                validTrips.Add(trip);
                sb.AppendLine($"Trip from {dto.OriginStation} to {dto.DestinationStation} imported.");
            }

            context.Trips.AddRange(validTrips);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
            var deserializedCard = (CardDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var validCards = new List<CustomerCard>();
            foreach (var dto in deserializedCard)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var cardType = Enum.TryParse<CardType>(dto.CardType, out var cardT) ? cardT : CardType.Normal;

                var card = new CustomerCard
                {
                    Name = dto.Name,
                    Type = cardType,
                    Age = dto.Age
                };

                validCards.Add(card);
                sb.AppendLine(String.Format(SuccessMessage, $"{card.Name}"));
            }

            context.Cards.AddRange(validCards);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            var deserializedTickets = (TicketDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var validTickets = new List<Ticket>();

            foreach (var dto in deserializedTickets)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var departureTime = DateTime.ParseExact(dto.Trip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var trip = context.Trips
                    .Include(t => t.OriginStation)
                    .Include(t => t.DestinationStation)
                    .Include(t => t.Train)
                    .ThenInclude(t => t.TrainSeats)
                    .SingleOrDefault(t => t.OriginStation.Name == dto.Trip.OriginStation &&
                                                    t.DestinationStation.Name == dto.Trip.DestinationStation &&
                                                    t.DepartureTime == departureTime);

                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;
                if (dto.Card != null)
                {
                    card = context.Cards.SingleOrDefault(c => c.Name == dto.Card.Name);

                    if (card == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                var seatingClassAbbriviation = dto.Seat.Substring(0, 2);
                var quantity = int.Parse(dto.Seat.Substring(2));

                var seatExists = trip.Train.TrainSeats
                    .SingleOrDefault(s => s.SeatingClass.Abbreviation == seatingClassAbbriviation &&
                         s.Quantity >= quantity);

                if (seatExists == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seat = dto.Seat;

                var ticket = new Ticket()
                {
                    Trip = trip,
                    CustomerCard = card,
                    Price = dto.Price,
                    SeatingPlace = seat
                };

                validTickets.Add(ticket);
                sb.AppendLine($"Ticket from {trip.OriginStation.Name} to {trip.DestinationStation.Name} departing at {trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} imported.");
            }

            context.Tickets.AddRange(validTickets);
            context.SaveChanges();
            return sb.ToString();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return isValid;
        }
    }
}