using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    public class Osoba
    {
        public string? m_strPESEL { get; set; }
        public string? m_strName { get; set; }
        public string? m_strSecName { get; set; }
        public string? m_strSurname { get; set; }
        public DateTime? m_dtBirth { get; set; }
        public string? m_strPhone { get; set; }
        public string? m_strAddress { get; set; }
        public string? m_strCity { get; set; }
        public string? m_strPostal { get; set; }

        public Osoba()
        {
            m_strPESEL = "";
            m_strName = "";
            m_strSecName = "";
            m_strSurname = "";
            m_dtBirth = null;
            m_strPhone = "";
            m_strAddress = "";
            m_strCity = "";
            m_strPostal = "";
        }

        public bool ValidatePESEL(out DateTime birthDate)
        {
            birthDate = DateTime.MinValue;
            var pesel = m_strPESEL ?? "";
            if (!Regex.IsMatch(pesel, @"^\d{11}$")) return false;
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++) sum += weights[i] * (pesel[i] - '0');
            int check = (10 - (sum % 10)) % 10;
            if (check != pesel[10] - '0') return false;
            int year = int.Parse(pesel.Substring(0, 2));
            int month = int.Parse(pesel.Substring(2, 2));
            int day = int.Parse(pesel.Substring(4, 2));
            int century = month > 80 ? 1800
                        : month > 60 ? 2200
                        : month > 40 ? 2100
                        : month > 20 ? 2000
                        : 1900;
            month %= 20;
            try
            {
                birthDate = new DateTime(century + year, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void FormatFields()
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            m_strName = ti.ToTitleCase((m_strName ?? "").Trim().ToLower());
            m_strSecName = ti.ToTitleCase((m_strSecName ?? "").Trim().ToLower());
            m_strSurname = ti.ToTitleCase((m_strSurname ?? "").Trim().ToLower());
            m_strAddress = ti.ToTitleCase((m_strAddress ?? "").Trim().ToLower());
            if (!string.IsNullOrWhiteSpace(m_strPhone))
            {
                var digits = Regex.Replace(m_strPhone, @"\D", "");
                if (digits.Length == 9) digits = "48" + digits;
                m_strPhone = "+" + digits;
            }
        }
    }
}
