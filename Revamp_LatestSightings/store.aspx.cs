using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Revamp_LatestSightings
{
    public partial class store : System.Web.UI.Page
    {
        protected SqlConnection conn;
        protected SqlCommand query;
        protected SqlDataReader data;
        protected Dictionary<string, string> profile;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (userId != null)
            {
                profile = GetUserProfile();
                Session["username"] = "Howzit " + profile["firstname"];
            }
        }

        private Dictionary<string, string> GetUserProfile()
        {
            Dictionary<string, string> userProfile = new Dictionary<string, string>();
            conn = new SqlConnection();
            query = new SqlCommand();
            data = null;
            userProfile = DataLayer.GetUserProfile(userId, conn, query, data);
            return userProfile;
        }
    }
}