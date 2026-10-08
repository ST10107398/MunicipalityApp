using System;
using System.Collections.Generic;

namespace MunicipalityApp
{
    // Event category enum for type safety
    public enum EventCategory
    {
        Community,
        Sports,
        Cultural,
        Educational,
        Health,
        Environment,
        PublicSafety,
        Entertainment,
        Government,
        Other
    }

    // Event class
    public class LocalEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EventCategory Category { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string ContactInfo { get; set; }
        public bool IsAnnouncement { get; set; }
        public DateTime PostedDate { get; set; }
        public int Priority { get; set; } // Higher = more important
        public bool IsFree { get; set; }
        public decimal TicketPrice { get; set; }

        public override string ToString()
        {
            return $"[{Category}] {Title} - {EventDate:yyyy-MM-dd}";
        }
    }

    // Search history for recommendations
    public class SearchRecord
    {
        public string SearchTerm { get; set; }
        public EventCategory? Category { get; set; }
        public DateTime SearchDate { get; set; }
    }

    // Recommendation score
    public class EventRecommendation
    {
        public LocalEvent Event { get; set; }
        public double Score { get; set; }
        public string Reason { get; set; }
    }
}