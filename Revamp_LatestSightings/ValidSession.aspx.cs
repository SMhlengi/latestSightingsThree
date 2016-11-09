using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Linq;
using System.Data.SqlClient;

namespace Revamp_LatestSightings
{
    public partial class ValidSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserData user = new UserData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SqlConnection conn = new SqlConnection();
                SqlCommand query = new SqlCommand();
                SqlDataReader data = null;
                Person userDetails = null;
                userDetails = DataLayer.GetUserDetails(conn, query, HttpContext.Current.Session["userid"].ToString(), data, userDetails);
                user.sessionIsValid = true;
                user.firstName = userDetails.FirstName;
                user.lastName = userDetails.LastName;
                user.email = userDetails.Email;

            }
            else
            {
                user.sessionIsValid = false;
            }

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(user, Formatting.Indented)) ;
        }
    }
}