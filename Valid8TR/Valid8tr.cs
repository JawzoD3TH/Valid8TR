using ISO_Classes;
using PhoneNumbers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Valid8TR
{
    public class Valid8tr
    {
        const string GoogleMaps = nameof(GoogleMaps);
        const string GPS = nameof(GPS);
        const string Image = nameof(Image);
        const string Link = nameof(Link);
        const string DateTime = nameof(DateTime);
        const string Email = nameof(Email);
        const string Phone = nameof(Phone);
        const string Card = nameof(Card);
        const string Text = nameof(Text);

        public string Validate(string input)
        {
            if (IsGoogleMaps(input))
                return GoogleMaps;

            if (IsGPSCoords(input))
                return GPS;

            if (IsImageUrl(input))
                return Image;

            if (IsUrl(input))
                return Link;

            if (IsDateTime(input))
                return DateTime;

            if (IsEmail(input))
                return Email;

            if (IsCreditCard(input))
                return Card;

            if (IsPhone(input))
                return Phone;

            return Text;
        }

        public IEnumerable<string> List(string input)
        {
            bool added = false;
            if (IsGoogleMaps(input))
            {
                added = true;
                yield return GoogleMaps;
            }

            if (IsGPSCoords(input))
            {
                added = true;
                yield return GPS;
            }

            if (IsImageUrl(input))
            {
                added = true;
                yield return Image;
            }

            if (IsUrl(input))
            {
                added = true;
                yield return Link;
            }

            if (IsDateTime(input))
            {
                added = true;
                yield return DateTime;
            }

            if (IsEmail(input))
            {
                added = true;
                yield return Email;
            }

            if (IsCreditCard(input))
            {
                added = true;
                yield return Card;
            }

            if (IsPhone(input))
            {
                added = true;
                yield return Phone;
            }

            if (!added)
                yield return Text;

        }

        public string StoryBoard(string input)
        {
            var sb = new System.Text.StringBuilder(input);

            foreach (var i in GetImageUrls(input))
                sb.Replace(i, $"<img src=\"{i}\" alt=\"Image Failed To Load\" />");

            foreach (var e in GetEmails(sb.ToString()))
                sb.Replace(e, $"<a href=\"mailto://{e}\">{e}</a>");

            foreach (var p in GetPhoneNumbers(sb.ToString()))
                sb.Replace(p.Key, $"<abbr title=\"Phone\">P:</abbr>{p.Value}");

            foreach (var l in GetUrls(sb.ToString()))
                sb.Replace(l, $"<a href=\"{l}\">{l}</a>");

            return sb.ToString();
        }

        //return Regex.IsMatch(input, @"([SN])\s(\d+)\s(\d+(?:\.\d+)?)\s([EW])\s(\d+)\s(\d+(?:\.\d*)?)");
        bool IsGPSCoords(string input) => new Coordinate().ParseIsoString(input);
        bool IsEmail(string input) => new EmailAddressAttribute().IsValid(input);
        bool IsPhone(string input) => new PhoneAttribute().IsValid(input);
        bool IsUrl(string input) => new UrlAttribute().IsValid(input);
        bool IsCreditCard(string input) => new CreditCardAttribute().IsValid(input);
        bool IsDateTime(string input) => System.DateTime.TryParse(input, out _);
        bool IsImageUrl(string input) => Regex.IsMatch(input, @"(https?:)?//?[^\'<>]+?\.(jpg|jpeg|gif|png)");

        IEnumerable<string> GetEmails(string input)
        {
            var m = Regex.Matches(input, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");
            for (int i = 0; i < m.Count; i++)
                yield return m[i].Value;

        }

        IEnumerable<string> GetUrls(string input)
        {
            var m = Regex.Matches(input, @"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?");
            for (int i = 0; i < m.Count; i++)
                yield return m[i].Value;

        }

        Dictionary<string, string> GetPhoneNumbers(string input)
        {
            Dictionary<string, string> Numbers = new Dictionary<string, string>();
            var phoneUtil = PhoneNumberUtil.GetInstance();
            foreach (var n in phoneUtil.FindNumbers(input, null))
                Numbers.Add(n.RawString, phoneUtil.Format(n.Number, PhoneNumberFormat.INTERNATIONAL));

            return Numbers;
        }

        bool IsGoogleMaps(string input)
        {
            if (Regex.IsMatch(input, @"(?<lat>(-?(90|(\d|[1-8]\d)(\.\d{1,6}){0,1})))\,{1}(?<long>(-?(180|(\d|\d\d|1[0-7]\d)(\.\d{1,6}){0,1})))"))
                return true;

            return Regex.IsMatch(input, @"\-?(90|[0-8]?[0-9]\.[0-9]{0,6})\,\-?(180|(1[0-7][0-9]|[0-9]{0,2})\.[0-9]{0,6})");
        }

        IEnumerable<string> GetImageUrls(string input)
        {
            var m = Regex.Matches(input, @"(https?:)?//?[^\'<>]+?\.(jpg|jpeg|gif|png)");
            for (int i = 0; i < m.Count; i++)
                yield return m[i].Value;

        }
    }
}