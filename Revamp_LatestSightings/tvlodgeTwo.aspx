<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tvlodgeTwo.aspx.cs" Inherits="Revamp_LatestSightings.tvlodgeTwo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>LS Sightings Board</title>
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/board.css?v=22")%>">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/font-awesome.min.css") %>">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/jquery.bxslider.css") %>">
    <script src="https://maps.googleapis.com/maps/api/js?v=3&key=AIzaSyB3O6AqwnZ3uHY0h6ulDrxlAWXXrs99VqI"></script>
    <script src="<%= ResolveUrl("~/js/jquery-2.1.1.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/js/tvLodgeTwo.js?v=47") %>"></script>

</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="lodgeName" value="<%= lodgeName %>" />
    <div id="aspectRatio" class="bd-wrapper">
        <!-- Header -->
        <div class="bd-hdr">
            <div class="logo-left">
                <img src="/images/LS_logo.png">
            </div>
            <div id="carousel">
              <div class="btn-bar">
                <div id="buttons"><a id="prev" href="#"><</a><a id="next" href="#">></a> </div>
              </div>
              <div id="slides">
                <ul class="textSliderContent">
                  <li class="slide">
                    <div class="quoteContainer">
                      <p class="quote-phrase"> I was literally BLOWN AWAY by Company A's work! They went above and beyond all of our expectations with design, usability. and branding.</p>
                    </div>
                  </li>
                  <li class="slide">
                    <div class="quoteContainer">
                      <p class="quote-phrase">I could not stop staring! Company A's Web Solutions are by far the most elegant solutions, you can't beat their quality and attention to detail!</p>
                    </div>
                  </li>
                  <li class="slide">
                    <div class="quoteContainer">
                      <p class="quote-phrase">Carl Fakeguy, was the most helpful designer I've ever hired. He listened to my ideas and advised against things that could negatively affect my CEO. </p>
                    </div>
                  </li>
                </ul>
              </div>
            </div>
            <div class="logo-right">
                <% if (!String.IsNullOrEmpty(lodoid)){ %>
                    <img src="<%= ConfigurationManager.AppSettings["lodgeImageFolderUrl"] %><%= lodoid%>">
                <%} %>
            </div>
        </div>
        <!-- Main Content -->
        <div class="bd-inner">
            <div class="leftcol">
                <!-- Video -->
                <div class="video">
                    <iframe width="420" height="220" src="https://www.youtube.com/embed/videoseries?list=PL_K2CjlrV2nD_V9wDQPSGAkIo5o5wzhYV&autoplay=1" frameborder="0" allowfullscreen></iframe>
                </div>
                <!-- Carousel -->
                <ul class="bxslider">
                </ul>
            </div>
            <div class="rightcol">
                <!-- Map -->
                <div class="map">
                    <!-- Map placeholder -->
                    <div id="map-canvas" style="width: 100%; height: 690px"></div>
                </div>
            </div>
        </div>
        <!-- Footer -->
        <div class="bd-footer">
            <div class="footer-txt-wrap">
                <div class="footer-txt">Download Latest Sightings now and share <strong>YOUR</strong> sightings. Remember to stream to
                    <% if (!String.IsNullOrEmpty(lodgeName)){%> 
                        <%= lodgeName %>
                    <%} %>
                    Lodge.</div>
            </div>
            <div class="stores">
                <img src="/images/qr_code.jpg">
            </div>
        </div>
    </div>


<script src="<%= ResolveUrl("~/js/jquery.bxslider.min.js") %>"></script>
</form>
</body>
</html>
