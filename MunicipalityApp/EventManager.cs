using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalityApp
{
    public class EventManager
    {
        // Sorted Dictionary: Events organized by date (optimized for date-based retrieval)
        public SortedDictionary<DateTime, List<LocalEvent>> EventsByDate { get; private set; }

        // Dictionary: Events indexed by ID for O(1) lookup
        public Dictionary<int, LocalEvent> EventsById { get; private set; }

        // Hash Set: Unique categories in use
        public HashSet<EventCategory> UsedCategories { get; private set; }

        // Hash Set: Unique dates with events
        public HashSet<DateTime> UniqueEventDates { get; private set; }

        // Dictionary: Events grouped by category
        public Dictionary<EventCategory, List<LocalEvent>> EventsByCategory { get; private set; }

        // Priority Queue: Important announcements (using SortedDictionary with priority)
        public SortedDictionary<int, Queue<LocalEvent>> AnnouncementPriorityQueue { get; private set; }

        // Stack: Recently viewed events (LIFO)
        public Stack<LocalEvent> RecentlyViewed { get; private set; }

        // Queue: Upcoming events in chronological order
        public Queue<LocalEvent> UpcomingEventsQueue { get; private set; }

        // Search history for recommendations
        public List<SearchRecord> SearchHistory { get; private set; }

        private int nextId = 1;

        public EventManager()
        {
            EventsByDate = new SortedDictionary<DateTime, List<LocalEvent>>();
            EventsById = new Dictionary<int, LocalEvent>();
            UsedCategories = new HashSet<EventCategory>();
            UniqueEventDates = new HashSet<DateTime>();
            EventsByCategory = new Dictionary<EventCategory, List<LocalEvent>>();
            AnnouncementPriorityQueue = new SortedDictionary<int, Queue<LocalEvent>>(
                Comparer<int>.Create((a, b) => b.CompareTo(a))); // Descending order
            RecentlyViewed = new Stack<LocalEvent>();
            UpcomingEventsQueue = new Queue<LocalEvent>();
            SearchHistory = new List<SearchRecord>();

            LoadSampleEvents();
        }

        private void LoadSampleEvents()
        {
            // Community Events
            AddEvent(new LocalEvent
            {
                Title = "Community Clean-Up Day",
                Description = "Join us for a community-wide clean-up drive. Gloves and bags provided.",
                Category = EventCategory.Environment,
                EventDate = DateTime.Now.AddDays(5),
                Location = "Central Park, Main Entrance",
                Organizer = "Green City Initiative",
                ContactInfo = "cleanup@municipality.gov",
                Priority = 3,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Annual Sports Day",
                Description = "Fun-filled sports activities for all ages. Register at the venue.",
                Category = EventCategory.Sports,
                EventDate = DateTime.Now.AddDays(10),
                Location = "Municipal Stadium",
                Organizer = "Sports Department",
                ContactInfo = "sports@municipality.gov",
                Priority = 2,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Local Art Exhibition",
                Description = "Featuring works from local artists. Refreshments will be served.",
                Category = EventCategory.Cultural,
                EventDate = DateTime.Now.AddDays(3),
                Location = "Community Arts Center",
                Organizer = "Arts Council",
                ContactInfo = "arts@municipality.gov",
                Priority = 2,
                IsFree = false,
                TicketPrice = 50
            });

            AddEvent(new LocalEvent
            {
                Title = "Free Health Screening",
                Description = "Free blood pressure, diabetes, and vision screening for all residents.",
                Category = EventCategory.Health,
                EventDate = DateTime.Now.AddDays(2),
                Location = "Community Health Center",
                Organizer = "Public Health Department",
                ContactInfo = "health@municipality.gov",
                Priority = 5, // High priority
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Town Hall Meeting",
                Description = "Discuss local issues with council members. Your voice matters!",
                Category = EventCategory.Government,
                EventDate = DateTime.Now.AddDays(7),
                Location = "Municipal Council Chambers",
                Organizer = "Municipal Council",
                ContactInfo = "council@municipality.gov",
                Priority = 5,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Kids Science Workshop",
                Description = "Interactive science experiments for children aged 6-12.",
                Category = EventCategory.Educational,
                EventDate = DateTime.Now.AddDays(14),
                Location = "Public Library, Conference Room",
                Organizer = "Education Department",
                ContactInfo = "education@municipality.gov",
                Priority = 3,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Night Market",
                Description = "Food, crafts, and live entertainment. Bring the whole family!",
                Category = EventCategory.Entertainment,
                EventDate = DateTime.Now.AddDays(1),
                Location = "Market Square",
                Organizer = "Local Business Association",
                ContactInfo = "market@municipality.gov",
                Priority = 3,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Neighborhood Watch Meeting",
                Description = "Monthly meeting to discuss community safety initiatives.",
                Category = EventCategory.PublicSafety,
                EventDate = DateTime.Now.AddDays(4),
                Location = "Community Hall, Room B",
                Organizer = "Neighborhood Watch",
                ContactInfo = "safety@municipality.gov",
                Priority = 4,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Recycling Workshop",
                Description = "Learn how to reduce, reuse, and recycle effectively.",
                Category = EventCategory.Environment,
                EventDate = DateTime.Now.AddDays(12),
                Location = "Environmental Center",
                Organizer = "Green City Initiative",
                ContactInfo = "recycle@municipality.gov",
                Priority = 2,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Cultural Dance Festival",
                Description = "Celebrate diversity with traditional dance performances.",
                Category = EventCategory.Cultural,
                EventDate = DateTime.Now.AddDays(20),
                Location = "City Amphitheater",
                Organizer = "Cultural Affairs",
                ContactInfo = "culture@municipality.gov",
                Priority = 3,
                IsFree = false,
                TicketPrice = 100
            });

            AddEvent(new LocalEvent
            {
                Title = "Water Conservation Announcement",
                Description = "Due to low reservoir levels, water restrictions are in effect.",
                Category = EventCategory.Government,
                EventDate = DateTime.Now,
                Location = "City-wide",
                Organizer = "Water Department",
                ContactInfo = "water@municipality.gov",
                Priority = 5,
                IsAnnouncement = true,
                IsFree = true
            });

            AddEvent(new LocalEvent
            {
                Title = "Road Maintenance Notice",
                Description = "Main Street will be closed for repairs from Monday to Friday.",
                Category = EventCategory.Government,
                EventDate = DateTime.Now.AddDays(1),
                Location = "Main Street",
                Organizer = "Roads Department",
                ContactInfo = "roads@municipality.gov",
                Priority = 4,
                IsAnnouncement = true,
                IsFree = true
            });
        }

        public void AddEvent(LocalEvent evt)
        {
            evt.Id = nextId++;

            // Add to ID dictionary
            EventsById[evt.Id] = evt;

            // Add to date-sorted dictionary
            DateTime dateKey = evt.EventDate.Date;
            if (!EventsByDate.ContainsKey(dateKey))
            {
                EventsByDate[dateKey] = new List<LocalEvent>();
            }
            EventsByDate[dateKey].Add(evt);

            // Add to category dictionary
            if (!EventsByCategory.ContainsKey(evt.Category))
            {
                EventsByCategory[evt.Category] = new List<LocalEvent>();
            }
            EventsByCategory[evt.Category].Add(evt);

            // Update sets
            UsedCategories.Add(evt.Category);
            UniqueEventDates.Add(dateKey);

            // Add to priority queue if announcement
            if (evt.IsAnnouncement || evt.Priority >= 4)
            {
                if (!AnnouncementPriorityQueue.ContainsKey(evt.Priority))
                {
                    AnnouncementPriorityQueue[evt.Priority] = new Queue<LocalEvent>();
                }
                AnnouncementPriorityQueue[evt.Priority].Enqueue(evt);
            }
        }

        // Get events sorted by date
        public List<LocalEvent> GetAllEventsSortedByDate()
        {
            var result = new List<LocalEvent>();
            foreach (var kvp in EventsByDate)
            {
                result.AddRange(kvp.Value.OrderBy(e => e.EventDate));
            }
            return result;
        }

        // Search events with filters
        public List<LocalEvent> SearchEvents(string searchTerm = null,
            EventCategory? category = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Record search for recommendations
            if (!string.IsNullOrWhiteSpace(searchTerm) || category.HasValue)
            {
                SearchHistory.Add(new SearchRecord
                {
                    SearchTerm = searchTerm?.ToLower() ?? "",
                    Category = category,
                    SearchDate = DateTime.Now
                });
            }

            IEnumerable<LocalEvent> query = EventsById.Values;

            // Filter by search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.ToLower();
                query = query.Where(e =>
                    e.Title.ToLower().Contains(term) ||
                    e.Description.ToLower().Contains(term) ||
                    e.Location.ToLower().Contains(term) ||
                    e.Organizer.ToLower().Contains(term));
            }

            // Filter by category
            if (category.HasValue)
            {
                query = query.Where(e => e.Category == category.Value);
            }

            // Filter by date range
            if (startDate.HasValue)
            {
                query = query.Where(e => e.EventDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.EventDate.Date <= endDate.Value.Date);
            }

            return query.OrderBy(e => e.EventDate).ToList();
        }

        // Get upcoming events as a queue
        public Queue<LocalEvent> GetUpcomingEventsQueue(int count = 10)
        {
            var upcoming = EventsById.Values
                .Where(e => e.EventDate >= DateTime.Now && !e.IsAnnouncement)
                .OrderBy(e => e.EventDate)
                .Take(count)
                .ToList();

            UpcomingEventsQueue = new Queue<LocalEvent>(upcoming);
            return UpcomingEventsQueue;
        }

        // Get high priority announcements
        public List<LocalEvent> GetHighPriorityAnnouncements(int count = 5)
        {
            var announcements = new List<LocalEvent>();

            foreach (var kvp in AnnouncementPriorityQueue)
            {
                foreach (var evt in kvp.Value)
                {
                    if (announcements.Count >= count) break;
                    announcements.Add(evt);
                }
                if (announcements.Count >= count) break;
            }

            // If not enough from priority queue, add other announcements
            if (announcements.Count < count)
            {
                var others = EventsById.Values
                    .Where(e => e.IsAnnouncement && !announcements.Contains(e))
                    .OrderByDescending(e => e.Priority)
                    .ThenByDescending(e => e.EventDate)
                    .Take(count - announcements.Count);
                announcements.AddRange(others);
            }

            return announcements;
        }

        // Recently viewed (Stack operations)
        public void AddToRecentlyViewed(LocalEvent evt)
        {
            // Remove if already in stack to avoid duplicates
            var tempList = RecentlyViewed.ToList();
            tempList.RemoveAll(e => e.Id == evt.Id);

            RecentlyViewed = new Stack<LocalEvent>(tempList.Reverse<LocalEvent>());
            RecentlyViewed.Push(evt);
        }

        public LocalEvent GetLastViewed()
        {
            return RecentlyViewed.Count > 0 ? RecentlyViewed.Peek() : null;
        }

        // RECOMMENDATION ENGINE
        public List<EventRecommendation> GetRecommendations(int count = 5)
        {
            var recommendations = new Dictionary<int, EventRecommendation>();

            // Strategy 1: Based on most searched categories
            var categoryFrequency = SearchHistory
                .Where(s => s.Category.HasValue)
                .GroupBy(s => s.Category.Value)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .ToList();

            foreach (var catGroup in categoryFrequency)
            {
                var categoryEvents = EventsById.Values
                    .Where(e => e.Category == catGroup.Key && e.EventDate >= DateTime.Now)
                    .OrderBy(e => e.EventDate)
                    .Take(3);

                foreach (var evt in categoryEvents)
                {
                    if (!recommendations.ContainsKey(evt.Id))
                    {
                        recommendations[evt.Id] = new EventRecommendation
                        {
                            Event = evt,
                            Score = catGroup.Count() * 2.0,
                            Reason = $"Popular in {evt.Category} (you searched this category {catGroup.Count()} times)"
                        };
                    }
                }
            }

            // Strategy 2: Based on search keywords
            var recentSearches = SearchHistory
                .Where(s => !string.IsNullOrWhiteSpace(s.SearchTerm))
                .OrderByDescending(s => s.SearchDate)
                .Take(5)
                .ToList();

            foreach (var search in recentSearches)
            {
                var matchingEvents = EventsById.Values
                    .Where(e => e.EventDate >= DateTime.Now &&
                        (e.Title.ToLower().Contains(search.SearchTerm) ||
                         e.Description.ToLower().Contains(search.SearchTerm)))
                    .Take(3);

                foreach (var evt in matchingEvents)
                {
                    if (!recommendations.ContainsKey(evt.Id))
                    {
                        recommendations[evt.Id] = new EventRecommendation
                        {
                            Event = evt,
                            Score = 3.0,
                            Reason = $"Related to your search: '{search.SearchTerm}'"
                        };
                    }
                    else
                    {
                        recommendations[evt.Id].Score += 1.5;
                    }
                }
            }

            // Strategy 3: High priority / important events
            var highPriority = EventsById.Values
                .Where(e => e.Priority >= 4 && e.EventDate >= DateTime.Now)
                .OrderByDescending(e => e.Priority)
                .Take(3);

            foreach (var evt in highPriority)
            {
                if (!recommendations.ContainsKey(evt.Id))
                {
                    recommendations[evt.Id] = new EventRecommendation
                    {
                        Event = evt,
                        Score = evt.Priority * 0.5,
                        Reason = "High priority event in your area"
                    };
                }
                else
                {
                    recommendations[evt.Id].Score += evt.Priority * 0.3;
                }
            }

            // Strategy 4: Recently viewed categories
            if (RecentlyViewed.Count > 0)
            {
                var recentCategories = RecentlyViewed
                    .Take(5)
                    .GroupBy(e => e.Category)
                    .OrderByDescending(g => g.Count())
                    .Take(2);

                foreach (var catGroup in recentCategories)
                {
                    var similarEvents = EventsById.Values
                        .Where(e => e.Category == catGroup.Key &&
                                    e.EventDate >= DateTime.Now &&
                                    !RecentlyViewed.Take(5).Any(r => r.Id == e.Id))
                        .Take(2);

                    foreach (var evt in similarEvents)
                    {
                        if (!recommendations.ContainsKey(evt.Id))
                        {
                            recommendations[evt.Id] = new EventRecommendation
                            {
                                Event = evt,
                                Score = 2.0,
                                Reason = $"Because you viewed {catGroup.Key} events"
                            };
                        }
                    }
                }
            }

            // Strategy 5: Soonest upcoming events (as fallback)
            var soonest = EventsById.Values
                .Where(e => e.EventDate >= DateTime.Now && !e.IsAnnouncement)
                .OrderBy(e => e.EventDate)
                .Take(5);

            foreach (var evt in soonest)
            {
                if (!recommendations.ContainsKey(evt.Id))
                {
                    recommendations[evt.Id] = new EventRecommendation
                    {
                        Event = evt,
                        Score = 1.0,
                        Reason = "Happening soon in your area"
                    };
                }
                else
                {
                    recommendations[evt.Id].Score += 0.5;
                }
            }

            // Return top N sorted by score
            return recommendations.Values
                .OrderByDescending(r => r.Score)
                .Take(count)
                .ToList();
        }

        // Get unique categories from HashSet
        public List<EventCategory> GetUniqueCategories()
        {
            return UsedCategories.ToList();
        }

        // Get unique dates from HashSet
        public List<DateTime> GetUniqueDates()
        {
            return UniqueEventDates.OrderBy(d => d).ToList();
        }
    }
}