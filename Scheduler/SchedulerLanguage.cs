using System;
using System.Collections.Generic;
using System.Globalization;

namespace Scheduler

{
    internal class SchedulerLanguage
    {
        private struct LanguageTag
        {
            internal string Tag;
            internal string Value;
        }
        private List<LanguageTag> _taglist = new List<LanguageTag>(); 
        private CultureInfo cultureinfo;
        private SupportedLanguagesEnum language;

        public SchedulerLanguage(SupportedLanguagesEnum Language)
        {
            language = Language;
            cultureinfo = new CultureInfo(Enum.GetName(typeof(SupportedLanguagesEnum), Language).ToString());

            // Carga tags lenguages
            LoadTags(Language);
        }
        private void LoadTags(SupportedLanguagesEnum language)
        {
            switch (language)
            {
                case SupportedLanguagesEnum.es_ES:
                    // Spanish Tags
                    _taglist.Add(new LanguageTag() { Tag = "occursone", Value = "Ocurre una sola vez" });
                    _taglist.Add(new LanguageTag() { Tag = "occursevery", Value = "Ocurre cada" });
                    _taglist.Add(new LanguageTag() { Tag = "occursonday", Value = "Ocurre el día" });
                    _taglist.Add(new LanguageTag() { Tag = "occursthe", Value = "Ocurre el" });
                    _taglist.Add(new LanguageTag() { Tag = "startingon", Value = "comenzando el" });
                    _taglist.Add(new LanguageTag() { Tag = "schedulewillbeusedon", Value = "La siguiente cita se producirá el" });
                    _taglist.Add(new LanguageTag() { Tag = "hours", Value = "hora(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "day", Value = "día" });
                    _taglist.Add(new LanguageTag() { Tag = "days", Value = "día(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "weeks", Value = "semana(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "months", Value = "mes(es)" });
                    _taglist.Add(new LanguageTag() { Tag = "between", Value = "entre" });
                    _taglist.Add(new LanguageTag() { Tag = "and", Value = "y" });
                    _taglist.Add(new LanguageTag() { Tag = "the", Value = "el" });
                    _taglist.Add(new LanguageTag() { Tag = "at", Value = "a las" });
                    _taglist.Add(new LanguageTag() { Tag = "on", Value = "el" });
                    _taglist.Add(new LanguageTag() { Tag = "ofevery", Value = "de cada" });
                    _taglist.Add(new LanguageTag() { Tag = "every", Value = "cada" });
                    _taglist.Add(new LanguageTag() { Tag = "first", Value = "primer" });
                    _taglist.Add(new LanguageTag() { Tag = "second", Value = "segundo" });
                    _taglist.Add(new LanguageTag() { Tag = "third", Value = "tercer" });
                    _taglist.Add(new LanguageTag() { Tag = "fourth", Value = "cuarto" });
                    _taglist.Add(new LanguageTag() { Tag = "last", Value = "ultimo" });
                    _taglist.Add(new LanguageTag() { Tag = "monday", Value = "lunes" });
                    _taglist.Add(new LanguageTag() { Tag = "tuesday", Value = "martes" });
                    _taglist.Add(new LanguageTag() { Tag = "wednesday", Value = "miercoles" });
                    _taglist.Add(new LanguageTag() { Tag = "thursday", Value = "jueves" });
                    _taglist.Add(new LanguageTag() { Tag = "friday", Value = "viernes" });
                    _taglist.Add(new LanguageTag() { Tag = "saturday", Value = "sabado" });
                    _taglist.Add(new LanguageTag() { Tag = "sunday", Value = "domingo" });
                    _taglist.Add(new LanguageTag() { Tag = "weekenddays", Value = "día del fin de semana" });
                    _taglist.Add(new LanguageTag() { Tag = "weekdays", Value = "día laboral" });
                    break;
                case SupportedLanguagesEnum.en_GB:
                case SupportedLanguagesEnum.en_US:
                    // English US Tags
                    _taglist.Add(new LanguageTag() { Tag = "occursone", Value = "Occurs One" });
                    _taglist.Add(new LanguageTag() { Tag = "occursevery", Value = "Occurs every" });
                    _taglist.Add(new LanguageTag() { Tag = "occursonday", Value = "Occurs on day" });
                    _taglist.Add(new LanguageTag() { Tag = "occursthe", Value = "Occurs the" });
                    _taglist.Add(new LanguageTag() { Tag = "startingon", Value = "starting on" });
                    _taglist.Add(new LanguageTag() { Tag = "schedulewillbeusedon", Value = "Schedule will be used on" });
                    _taglist.Add(new LanguageTag() { Tag = "hours", Value = "hour(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "day", Value = "day" });
                    _taglist.Add(new LanguageTag() { Tag = "days", Value = "day(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "weeks", Value = "week(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "months", Value = "month(s)" });
                    _taglist.Add(new LanguageTag() { Tag = "between", Value = "between" });
                    _taglist.Add(new LanguageTag() { Tag = "and", Value = "and" });
                    _taglist.Add(new LanguageTag() { Tag = "the", Value = "the" });
                    _taglist.Add(new LanguageTag() { Tag = "at", Value = "at" });
                    _taglist.Add(new LanguageTag() { Tag = "on", Value = "on" });
                    _taglist.Add(new LanguageTag() { Tag = "ofevery", Value = "of every" });
                    _taglist.Add(new LanguageTag() { Tag = "every", Value = "every" });
                    _taglist.Add(new LanguageTag() { Tag = "first", Value = "first" });
                    _taglist.Add(new LanguageTag() { Tag = "second", Value = "second" });
                    _taglist.Add(new LanguageTag() { Tag = "third", Value = "third" });
                    _taglist.Add(new LanguageTag() { Tag = "fourth", Value = "fourth" });
                    _taglist.Add(new LanguageTag() { Tag = "last", Value = "last" });
                    _taglist.Add(new LanguageTag() { Tag = "monday", Value = "monday" });
                    _taglist.Add(new LanguageTag() { Tag = "tuesday", Value = "tuesday" });
                    _taglist.Add(new LanguageTag() { Tag = "wednesday", Value = "wednesday" });
                    _taglist.Add(new LanguageTag() { Tag = "thursday", Value = "thursday" });
                    _taglist.Add(new LanguageTag() { Tag = "friday", Value = "friday" });
                    _taglist.Add(new LanguageTag() { Tag = "saturday", Value = "saturday" });
                    _taglist.Add(new LanguageTag() { Tag = "sunday", Value = "sunday" });
                    _taglist.Add(new LanguageTag() { Tag = "weekenddays", Value = "weekenddays" });
                    _taglist.Add(new LanguageTag() { Tag = "weekdays", Value = "weekdays" });
                    break;
            }

        }

        public int GetWeekOfYear(DateTime ActualDate,CalendarWeekRule  WeekRule,DayOfWeek  Day)
        {
            return  cultureinfo.Calendar.GetWeekOfYear(ActualDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public string GetLanguageDateTimeFormatted(DateTime fecha)
        {
            return fecha.ToString("d", cultureinfo) + ' ' + fecha.ToString("t", cultureinfo);
        }
        public string GetLanguageDateFormatted(DateTime fecha)
        {
            return fecha.ToString("d", cultureinfo);
        }
        public string GetLanguageTimeFormatted(DateTime fecha)
        {
            return fecha.ToString("t", cultureinfo);
        }
        public string TraslateTag(string text)
        {
            string traslated = "** "+text+" **";

            var tag = _taglist.Find(x => x.Tag.Equals(text));
            if (tag.Value != null)
                traslated = tag.Value;

            return traslated;
        }
    }
}

