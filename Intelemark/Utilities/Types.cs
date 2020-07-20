using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Intelemark.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }

    public enum QuestionTypes : Int32
    {
        [Display(Name = "Open Answer")]
        OpenAnswer,

        [Display(Name = "Multiple Answer")]
        MultipleAnswer,

        [Display(Name = "Selectable Answer")]
        SelectableAnswer,

        [Display(Name = "Calendar Answer")]
        CalendarAnswer,

        [Display(Name = "Calendar Time Answer")]
        CalendarTimeAnswer,

        [Display(Name = "Time Answer")]
        TimeAnswer,
    }

    public enum FieldTypes : Int32
    {
        [Display(Name = "Text")]
        Text,

        [Display(Name = "Selectable Option")]
        SelectableOption,

        [Display(Name = "Date Option")]
        DateOption,

        [Display(Name = "Date Time Option")]
        DateTimeOption,

        [Display(Name = "Time Option")]
        TimeOption,

        //[Display(Name = "Multiple Options")]
        //MultipleOptions,
    }

    public enum EOCBehaviors : Int32
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "Call Back")]
        CallBack,
        [Display(Name = "Finalize Record")]
        FinalizeRecord,
        [Display(Name = "Set up Appointment")]
        SetUpAppointment,
        [Display(Name = "Finalize Company")]
        FinalizeCompany,

        [Display(Name = "Migrated")]
        Migrated = 999,
    }

    public enum Priorities : Int32
    {
        [Display(Name = "Low")]
        Low,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "High")]
        High,
    }

}