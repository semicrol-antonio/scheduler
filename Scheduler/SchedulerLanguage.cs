using System;
using System.Globalization;

namespace Scheduler

{
    internal class SchedulerLanguage
    {
        private CultureInfo cultureinfo;
        private SupportedLanguagesEnum language;
        public SchedulerLanguage()
        {
            cultureinfo = new CultureInfo("es_ES");
        }
        public SchedulerLanguage(SupportedLanguagesEnum Language)
        {
            language = Language;
            cultureinfo = new CultureInfo(Enum.GetName(typeof(SupportedLanguagesEnum), Language).ToString());
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
        public string Traslate(string Text)
        {
            string traslated = "";
            switch (language)
            {
                case SupportedLanguagesEnum.en_GB:
                    traslated = TraslateEnglish(Text);
                    break;
                case SupportedLanguagesEnum.en_US:
                    traslated = TraslateEnglish(Text);
                    break;
                case SupportedLanguagesEnum.es_ES:
                    traslated = TraslateSpanish(Text);
                    break;
            }
            return traslated;
            
        }
        private string TraslateEnglish(string Text)
        {
            return Text;
        }
        private string TraslateSpanish(string Text)
        {
            Text = Text.Replace("Occurs", "Ocurre");
            Text = Text.Replace("One", "una sola vez");
            Text = Text.Replace("Occurs the", "Ocurre el");
            Text = Text.Replace("Occurs on day", "Ocurre el día");
            Text = Text.Replace("of every", "de cada");
            Text = Text.Replace("every", "cada");
            Text = Text.Replace("Every", "Cada");
            Text = Text.Replace("month(s)", "mes(es)");
            Text = Text.Replace("week(s)", "semana(s)");
            Text = Text.Replace(" day ", " dia ");
            Text = Text.Replace("day(s)", "dia(s)");
            Text = Text.Replace("hour(s)", "hora(s)");
            Text = Text.Replace("Schedule will be used on", "La siguiente cita se producirá el");
            Text = Text.Replace("starting on", "comenzando el");
            Text = Text.Replace("between", "entre");
            Text = Text.Replace(" and ", " y ");
            Text = Text.Replace(" at ", " a las ");
            Text = Text.Replace(" on ", " el ");
            Text = Text.Replace("monday", "lunes");
            Text = Text.Replace("tuesday", "martes");
            Text = Text.Replace("wednesday", "miercoles");
            Text = Text.Replace("thursday", "jueves");
            Text = Text.Replace("friday", "viernes");
            Text = Text.Replace("saturday", "sabado");
            Text = Text.Replace("sunday", "domingo");
            Text = Text.Replace("weekenddays", "dia del fin de semana");
            Text = Text.Replace("weekdays", "dia laboral");

            Text = Text.Replace(" the ", " el ");
            Text = Text.Replace("first", "primer");
            Text = Text.Replace("second", "segundo");
            Text = Text.Replace("third", "tercer");
            Text = Text.Replace("fourth", "cuarto");
            Text = Text.Replace("last", "ultimo");

            return Text;
        }
    }
}

