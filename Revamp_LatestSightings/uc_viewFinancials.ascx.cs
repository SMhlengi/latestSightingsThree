﻿using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Revamp_LatestSightings
{
    public partial class uc_viewFinancials : System.Web.UI.UserControl
    {

        // new code
        //private int year = DateTime.Now.Year;
        //private int month = DateTime.Now.Month;
        private string contributor = string.Empty;

        public string data = string.Empty;
        public string ykeyA = string.Empty;
        public string ykeyB = string.Empty;
        public string ykeyALabel = string.Empty;
        public string ykeyBLabel = string.Empty;
        public string data1 = string.Empty;
        public string data2 = string.Empty;
        public string data3 = string.Empty;
        public string m3Keys = string.Empty;
        public string m3Labels = string.Empty;
        public string m4Keys = string.Empty;
        public string m4Labels = string.Empty;
        // end of new code
        private string id;

        public string userId
        {
            get
            {
                if (String.IsNullOrEmpty(id) && Session["userid"] != null)
                {
                    id = Session["userid"].ToString();
                }
                return id;
            }
        }

        private List<LatestSightingsLibrary.Video> _videos;
        private List<LatestSightingsLibrary.Video> videos
        {
            get
            {
                if (_videos == null)
                {
                    _videos = LatestSightingsLibrary.Video.GetContributorVideos(userId);
                }

                return _videos;
            }
            set
            {
                _videos = value;
            }
        }

        public string currencyScripts = string.Empty;
        public int year = DateTime.Now.Year;
        public int month = DateTime.Now.Month;

        protected void Page_Load(object sender, EventArgs e)
        {
            FillVideos();
            SetCurrencies();
            FillStats();
            FileGraphs(year, month, userId);
        }

        private void FileGraphs(int year, int month, string userId)
        {
            //if (Request.QueryString["year"] != null)
            //{
            //    year = Convert.ToInt32(Request.QueryString["year"]);
            //}
            //if (Request.QueryString["month"] != null)
            //{
            //    month = Convert.ToInt32(Request.QueryString["month"]);
            //}

            contributor = userId;


            ykeyA = "a";
            ykeyB = "b";

            DateTime date = new DateTime(year, month, 1);

            ykeyALabel = date.ToString("MMMM yyyy");
            ykeyBLabel = date.AddMonths(-1).ToString("MMMM yyyy");

            List<YouTubeVideoAnalytic> days1 = YouTubeVideoAnalytic.GetVideoTotalsByContributorWithShare(date.Year, date.Month, contributor);
            List<YouTubeVideoAnalytic> days2 = YouTubeVideoAnalytic.GetVideoTotalsByContributorWithShare(date.AddMonths(-1).Year, date.AddMonths(-1).Month, contributor);
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                string dayId = i.ToString();
                string value1 = "0";
                string value2 = "0";
                string value3 = "0";
                string value4 = "0";

                YouTubeVideoAnalytic day1 = days1.FirstOrDefault(x => { return x.Day == i; });
                YouTubeVideoAnalytic day2 = days2.FirstOrDefault(x => { return x.Day == i; });
                if (day1 != null)
                {
                    value1 = day1.EstimatedEarning.ToString();
                    value2 = day2 != null ? day2.EstimatedEarning.ToString() : 0.ToString();
                    value3 = day1.Views.ToString();
                    value4 = day2 != null ? day2.Views.ToString() : 0.ToString();

                    if (!String.IsNullOrEmpty(data))
                        data += ",";

                    if (!String.IsNullOrEmpty(data1))
                        data1 += ",";

                    DateTime curDay = new DateTime(date.Year, date.Month, i);

                    data += "{ y: '" + curDay.ToString("yyyy-MM-dd") + "', a: " + Decimal.Parse(value1).ToString(CultureInfo.InvariantCulture) + ", b: " + Decimal.Parse(value2).ToString(CultureInfo.InvariantCulture) + " }";
                    data1 += "{ y: '" + curDay.ToString("yyyy-MM-dd") + "', a: " + value3 + ", b: " + value4 + " }";
                }
            }

            List<LatestSightingsLibrary.Video> videos = LatestSightingsLibrary.Video.GetContributorVideos(contributor);
            List<YouTubeVideoAnalytics> stats = LatestSightingsLibrary.Video.GetYouTubeVideoAnalyticsByContributor(year, month, contributor);
            Dictionary<int, List<YouTubeVideoAnalytic>> videoStats = new Dictionary<int, List<YouTubeVideoAnalytic>>();
            int totalDays = 0;

            if (stats != null && videos != null)
            {
                stats = stats.OrderByDescending(x => x.EstimatedEarning).ToList();
                int count = 1;
                foreach (YouTubeVideoAnalytics stat in stats)
                {
                    LatestSightingsLibrary.Video video = videos.FirstOrDefault(x => { return x.YoutubeId == stat.Id; });
                    if (video != null && count < 6)
                    {
                        List<YouTubeVideoAnalytic> videoDays = YouTubeVideoAnalytic.GetDaysByVideo(year, month, stat.Id);
                        if (videoDays != null && videoDays.Count > 0)
                        {
                            int videoLastDay = videoDays.OrderByDescending(x => x.Day).ToList()[0].Day;
                            if (videoLastDay > totalDays)
                                totalDays = videoLastDay;
                            if (!String.IsNullOrEmpty(m3Keys))
                                m3Keys += ",";
                            if (!String.IsNullOrEmpty(m4Keys))
                                m4Keys += ",";
                            if (!String.IsNullOrEmpty(m3Labels))
                                m3Labels += ",";
                            if (!String.IsNullOrEmpty(m4Labels))
                                m4Labels += ",";

                            m3Keys += "'" + (count - 1).ToString() + "'";
                            m4Keys += "'" + (count - 1).ToString() + "'";
                            m3Labels += "'" + Regex.Replace(video.Title, @"[^a-zA-Z0-9\s]", string.Empty) + "'";
                            m4Labels += "'" + Regex.Replace(video.Title, @"[^a-zA-Z0-9\s]", string.Empty) + "'";
                            videoStats.Add(count - 1, videoDays);
                            count++;
                        }
                    }
                }

                for (int i = 1; i <= totalDays; i++)
                {
                    DateTime curDay = new DateTime(date.Year, date.Month, i);
                    if (!String.IsNullOrEmpty(data2))
                        data2 += ",";
                    if (!String.IsNullOrEmpty(data3))
                        data3 += ",";

                    data2 += "{ y: '" + curDay.ToString("yyyy-MM-dd") + "'";
                    data3 += "{ y: '" + curDay.ToString("yyyy-MM-dd") + "'";

                    for (int n = 0; n < videoStats.Count; n++)
                    {
                        List<YouTubeVideoAnalytic> vids = videoStats[n];
                        YouTubeVideoAnalytic dayData = vids.FirstOrDefault(x => { return x.Day == i; });
                        string value1 = dayData != null ? dayData.EstimatedEarning.ToString() : "0";
                        string value2 = dayData != null ? dayData.Views.ToString() : "0";

                        data2 += ", " + n.ToString() + ": '" + Decimal.Parse(value1).ToString("G29") + "'";
                        data3 += ", " + n.ToString() + ": '" + value2 + "'";
                    }

                    data2 += " }";
                    data3 += " }";
                }

                data2 = data2.Replace("wer", "");
                data3 = data3.Replace("wer", "");
            }
        }

        private void FillStats()
        {
            Month mnth = Month.GetMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (mnth == null || (mnth != null && mnth.EstimatedEarnings <= 0))
            {
                mnth = Month.GetMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            }

            if (mnth != null)
            {
                year = mnth.YearNumber;
                month = mnth.MonthNumber;

                List<YouTubeVideoAnalytics> anals = LatestSightingsLibrary.Video.GetYouTubeVideoAnalyticsByContributor(year, month, userId);
                decimal totalEarnings = 0;
                long totalViews = 0;

                if (anals != null)
                {
                    foreach (YouTubeVideoAnalytics anal in anals)
                    {
                        if (anal.EstimatedEarning > 0)
                        {
                            totalEarnings += ApplyRevenueShare("", anal.Id, anal.EstimatedEarning);
                        }
                        if (anal.Views > 0)
                            totalViews += anal.Views;
                    }
                }

                ltlEarnings.Text = "$" + totalEarnings.ToString();
                ltlViews.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalViews);

                List<LatestSightingsLibrary.Video> videos = LatestSightingsLibrary.Video.GetContributorVideos(userId);

                List<YouTubeVideoAnalytics> stats = LatestSightingsLibrary.Video.GetYouTubeVideoAnalyticsByContributor(year, month, userId);
                if (stats != null && videos != null)
                {
                    stats = stats.OrderByDescending(x => x.EstimatedEarning).ToList();
                    StringBuilder sb = new StringBuilder();
                    int count = 1;
                    foreach (YouTubeVideoAnalytics stat in stats)
                    {
                        LatestSightingsLibrary.Video video = videos.FirstOrDefault(x => { return x.YoutubeId == stat.Id; });
                        if (video != null)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + count.ToString() + "</td>");
                            sb.Append("<td>" + video.Title + "</td>");
                            sb.Append("<td>$" + ApplyRevenueShare("", stat.Id, stat.EstimatedEarning) + "</td>");
                            sb.Append("</tr>");
                            count++;
                        }
                    }
                    ltlTop10Earnings.Text = sb.ToString();

                    stats = stats.OrderByDescending(x => x.Views).ToList();
                    sb = new StringBuilder();
                    count = 1;
                    foreach (YouTubeVideoAnalytics stat in stats)
                    {
                        LatestSightingsLibrary.Video video = videos.FirstOrDefault(x => { return x.YoutubeId == stat.Id; });
                        if (video != null)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + count.ToString() + "</td>");
                            sb.Append("<td>" + video.Title + "</td>");
                            sb.Append("<td>" + string.Format(CultureInfo.InvariantCulture, "{0:N0}", stat.Views) + "</td>");
                            sb.Append("</tr>");
                            count++;
                        }
                    }
                    ltlTop10Views.Text = sb.ToString();
                }
            }
        }

        private void FillVideos()
        {
            List<LatestSightingsLibrary.Video> contVideos = LatestSightingsLibrary.Video.GetContributorVideos(userId);
            if (contVideos != null && contVideos.Count > 0)
            {
                tableRow.Visible = true;
                alert.Visible = false;
            }
            else
            {
                tableRow.Visible = false;
                alert.Visible = true;
            }
        }



        private void SetCurrencies()
        {
            List<Currency> currencies = Currency.GetCurrencies();
            if (currencies != null)
            {
                int count = 1;
                StringBuilder sb = new StringBuilder();
                foreach (Currency currency in currencies)
                {
                    currencyScripts += "{ \"data\": \"Currency" + count.ToString() + "\", orderable: false },";
                    sb.Append("<th style=\"background-color:#554337; color: #FFFFFF; border-bottom: 0px;\">" + currency.Description + "</th>" + Environment.NewLine);
                    count++;
                }
                ltlCurrencies.Text = sb.ToString();
            }
        }

        private decimal RoundtoTwo(decimal value)
        {
            return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private decimal ApplyRevenueShare(string videoId, string youtubeId, decimal value)
        {
            decimal newValue = 0;
            string revenueShare = string.Empty;
            if (videos != null && videos.Count > 0)
            {
                LatestSightingsLibrary.Video vid = null;
                if (!String.IsNullOrEmpty(youtubeId))
                {
                    vid = videos.FirstOrDefault(x => { return x.YoutubeId == youtubeId; });
                }
                else
                {
                    vid = videos.FirstOrDefault(x => { return x.Id == videoId; });
                }
                if (vid != null)
                {
                    newValue = LatestSightingsLibrary.Financial.ApplyRevenueShare(value, vid.RevenueShare);
                    newValue = RoundtoTwo(newValue);
                }
            }

            return newValue;
        }
    }
}