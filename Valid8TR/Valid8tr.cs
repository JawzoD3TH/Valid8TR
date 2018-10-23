using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ISO_Classes;
using PhoneNumbers;

namespace Valid8TR
{
    public class Valid8tr
    {
        public string Validate(string input)
        {
            if (IsGoogleMaps(input))
                return "GoogleMaps";
            if (IsGPSCoords(input))
                return "GPS";
            if (IsImageUrl(input))
                return "Image";
            if (IsUrl(input))
                return "Link";
            if (IsDateTime(input))
                return "DateTime";
            if (IsEmail(input))
                return "Email";
            if (IsPhone(input))
                return "Phone";
            if (IsCreditCard(input))
                return "Card";

            return "Text";
        }

        public List<string> List(string input)
        {
            List<string> List = new List<string>();
            if (IsGoogleMaps(input))
                List.Add("GoogleMaps");
            if (IsGPSCoords(input))
                List.Add("GPS");
            if (IsUrl(input))
                List.Add("Link");
            if (IsDateTime(input))
                List.Add("DateTime");
            if (IsEmail(input))
                List.Add("Email");
            if (IsPhone(input))
                List.Add("Phone");
            if (IsCreditCard(input))
                List.Add("Card");
            if (List.Count == 0)
                List.Add("Text");

            return List;
        }

        public string StoryBoard(string input)
        {
            var sb = new System.Text.StringBuilder(input);
            var List = GetImageUrls(input);
            foreach (var i in List)
                sb.Replace(i, $"<img src=\"{i}\" alt=\"Image Failed To Load\" />");

            List.Clear();
            List = GetEmails(sb.ToString());
            foreach (var e in List)
                sb.Replace(e, $"<a href=\"mailto://{e}\">{e}</a>");

            var Numbers = GetPhoneNumbers(sb.ToString());
            foreach (var p in Numbers)
                sb.Replace(p.Key, $"<abbr title=\"Phone\">P:</abbr>{p.Value}");

            List.Clear();
            List = GetUrls(sb.ToString());
            foreach (var l in List)
                sb.Replace(l, $"<a href=\"{l}\">{l}</a>");

            return sb.ToString();
        }

        bool IsEmail(string input)
        {
            var attr = new EmailAddressAttribute();
            if (attr.IsValid(input))
                return true;

            return false;
        }

        List<string> GetEmails(string input)
        {
            List<string> List = new List<string>();
            var m = Regex.Matches(input, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");
            for (int i = 0; i < m.Count; i++)
                List.Add(m[i].Value);

            return List;
        }

        bool IsPhone(string input)
        {
            var attr = new PhoneAttribute();
            if (attr.IsValid(input))
                return true;

            return false;
        }

        Dictionary<string, string> GetPhoneNumbers(string input)
        {
            Dictionary<string, string> Numbers = new Dictionary<string, string>();
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var NumberCollection = phoneUtil.FindNumbers(input, null);
            foreach (var n in NumberCollection)
                Numbers.Add(n.RawString, phoneUtil.Format(n.Number, PhoneNumberFormat.INTERNATIONAL));

            return Numbers;
        }

        bool IsUrl(string input)
        {
            var attr = new UrlAttribute();
            if (attr.IsValid(input))
                return true;

            return false;
        }

        List<string> GetUrls(string input)
        {
            List<string> List = new List<string>();
            var m = Regex.Matches(input, @"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?");
            for (int i = 0; i < m.Count; i++)
                List.Add(m[i].Value);

            return List;
        }

        bool IsCreditCard(string input)
        {
            var attr = new CreditCardAttribute();
            if (attr.IsValid(input))
                return true;

            return false;
        }

        bool IsDateTime(string input)
        {
            DateTime tDT;
            return DateTime.TryParse(input, out tDT);
        }

        bool IsGPSCoords(string input)
        {
            Coordinate tCoord = new Coordinate();
            try
            {
                tCoord.ParseIsoString(input);
                return true;
            }
            catch { return false; }
            //return Regex.IsMatch(input, @"([SN])\s(\d+)\s(\d+(?:\.\d+)?)\s([EW])\s(\d+)\s(\d+(?:\.\d*)?)");
        }

        bool IsGoogleMaps(string input)
        {
            if (Regex.IsMatch(input, @"(?<lat>(-?(90|(\d|[1-8]\d)(\.\d{1,6}){0,1})))\,{1}(?<long>(-?(180|(\d|\d\d|1[0-7]\d)(\.\d{1,6}){0,1})))"))
                return true;

            return Regex.IsMatch(input, @"\-?(90|[0-8]?[0-9]\.[0-9]{0,6})\,\-?(180|(1[0-7][0-9]|[0-9]{0,2})\.[0-9]{0,6})");
        }

        bool IsImageUrl(string input)
        {
            if (Regex.IsMatch(input, @"(https?:)?//?[^\'<>]+?\.(jpg|jpeg|gif|png)"))
                return true;

            return false;
        }

        List<string> GetImageUrls(string input)
        {
            List<string> List = new List<string>();
            var m = Regex.Matches(input, @"(https?:)?//?[^\'<>]+?\.(jpg|jpeg|gif|png)");
            for (int i = 0; i < m.Count; i++)
                List.Add(m[i].Value);

            return List;
        }
    }
}