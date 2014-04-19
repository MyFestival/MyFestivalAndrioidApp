using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyFestivalApp.Models
{
    #region Festival

    public class Festival
    {
        public int FestivalId { get; set; }

        public string FestivalName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual County FestivalCounty { get; set; }

        public virtual Town FestivalTown { get; set; }

        public virtual FestivalType FType { get; set; }

        public string Description { get; set; }

        public ICollection<Events> Events { get; set; }
        public IEnumerable<Events> EventsOrdered
        {
            get { return Events.OrderBy(e => e.EventsDate); }
        }
    }

    #endregion

    #region Events

    public class Events
    {
        public int ID { get; set; }

        public int FestivalID { get; set; }

        public string EventsName { get; set; }

        public DateTime EventsDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Location { get; set; }

        public virtual EventType EType { get; set; }

    }

    #endregion

    #region County

    public class County
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region Town

    public class Town
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region Festival Type

    public class FestivalType
    {
        public int ID { get; set; } 
        public string FType { get; set; }
    }

    #endregion

    #region EventType

    public class EventType
    {
        public int ID { get; set; }
        public string EType { get; set; }
    }

    #endregion
}