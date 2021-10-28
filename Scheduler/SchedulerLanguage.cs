using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Scheduler

{
    internal class SchedulerLanguage
    {
        Hashtable _taglist = new Hashtable();
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
                    _taglist.Add("occursone", "Ocurre una sola vez" );
                    _taglist.Add( "occursevery", "Ocurre cada" );
                    _taglist.Add( "occursonday", "Ocurre el día" );
                    _taglist.Add( "occursthe", "Ocurre el" );
                    _taglist.Add( "startingon", "comenzando el" );
                    _taglist.Add( "schedulewillbeusedon", "La siguiente cita se producirá el" );
                    _taglist.Add( "hours", "hora(s)" );
                    _taglist.Add( "day", "día" );
                    _taglist.Add( "days", "día(s)" );
                    _taglist.Add( "weeks", "semana(s)" );
                    _taglist.Add( "months", "mes(es)" );
                    _taglist.Add( "between", "entre" );
                    _taglist.Add( "and", "y" );
                    _taglist.Add( "the", "el" );
                    _taglist.Add( "at", "a las" );
                    _taglist.Add( "on", "el" );
                    _taglist.Add( "ofevery", "de cada" );
                    _taglist.Add( "every", "cada" );
                    _taglist.Add( "first", "primer" );
                    _taglist.Add( "second", "segundo" );
                    _taglist.Add( "third", "tercer" );
                    _taglist.Add( "fourth", "cuarto" );
                    _taglist.Add( "last", "ultimo" );
                    _taglist.Add( "monday", "lunes" );
                    _taglist.Add( "tuesday", "martes" );
                    _taglist.Add( "wednesday", "miercoles" );
                    _taglist.Add( "thursday", "jueves" );
                    _taglist.Add( "friday", "viernes" );
                    _taglist.Add( "saturday", "sabado" );
                    _taglist.Add( "sunday", "domingo" );
                    _taglist.Add( "weekenddays", "día del fin de semana" );
                    _taglist.Add( "weekdays", "día laboral" );
                    break;
                case SupportedLanguagesEnum.en_GB:
                case SupportedLanguagesEnum.en_US:
                    // English US Tags
                    _taglist.Add( "occursone", "Occurs One" );
                    _taglist.Add( "occursevery", "Occurs every" );
                    _taglist.Add( "occursonday", "Occurs on day" );
                    _taglist.Add( "occursthe", "Occurs the" );
                    _taglist.Add( "startingon", "starting on" );
                    _taglist.Add( "schedulewillbeusedon", "Schedule will be used on" );
                    _taglist.Add( "hours", "hour(s)" );
                    _taglist.Add( "day", "day" );
                    _taglist.Add( "days", "day(s)" );
                    _taglist.Add( "weeks", "week(s)" );
                    _taglist.Add( "months", "month(s)" );
                    _taglist.Add( "between", "between" );
                    _taglist.Add( "and", "and" );
                    _taglist.Add( "the", "the" );
                    _taglist.Add( "at", "at" );
                    _taglist.Add( "on", "on" );
                    _taglist.Add( "ofevery", "of every" );
                    _taglist.Add( "every", "every" );
                    _taglist.Add( "first", "first" );
                    _taglist.Add( "second", "second" );
                    _taglist.Add( "third", "third" );
                    _taglist.Add( "fourth", "fourth" );
                    _taglist.Add( "last", "last" );
                    _taglist.Add( "monday", "monday" );
                    _taglist.Add( "tuesday", "tuesday" );
                    _taglist.Add( "wednesday", "wednesday" );
                    _taglist.Add( "thursday", "thursday" );
                    _taglist.Add( "friday", "friday" );
                    _taglist.Add( "saturday", "saturday" );
                    _taglist.Add( "sunday", "sunday" );
                    _taglist.Add( "weekenddays", "weekenddays" );
                    _taglist.Add( "weekdays", "weekdays" );
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
            string traslated;

            try
            {
                traslated = _taglist[text].ToString();
            }
            catch (Exception e)
            {
                traslated = "** " + text + " **";
            }

            return traslated;
        }
    }
}

