using System;

using Xamarin.Forms;

namespace Mandrilkalender
{
	public class VideoPage : ContentPage
	{
		MandrilContentView videoPlayer;
		//WebView webView;
		//Gate gate;

		public VideoPage(Gate g)
		{
			BackgroundColor = Color.Black;
			//Padding = new Thickness(0, 0, 0, 0);

			//gate = g;
			//webView = new WebView()
			//{
			//	HorizontalOptions = LayoutOptions.Center,
			//	VerticalOptions = LayoutOptions.Center,
			//};

			//var layout = new StackLayout()
			//{
			//	Padding = new Thickness(0, 0, 0, 0),
			//	HorizontalOptions = LayoutOptions.Fill,
			//	VerticalOptions = LayoutOptions.Fill,
			//	BackgroundColor = Color.Black
			//};
			//layout.Children.Add(webView);

			//Content = layout;
			videoPlayer = new MandrilContentView(g)
			{
				WidthRequest = App.ScreenWidth / 2,
				HeightRequest = App.ScreenHeight / 2,
			};

			Content = new StackLayout
			{
				Children = {
					videoPlayer
				}
			};
		}

		//private HtmlWebViewSource CreateHtmlDom(Gate g, double width, double height)
		//{
		//	//width = "{width}" height = "{height}"
		//	var htmlSource = new HtmlWebViewSource();
		//	htmlSource.Html = $"<html><body style=\"margin: 0;\"><iframe style=\"border: 0;\"id=\"player\"  src=\"https://www.youtube.com/embed/{g.VideoId}?enablejsapi=1\" frameborder=\"0\" allowfullscreen></iframe>" +
		//		"<script type=\"text/javascript\">" +
		//		"  var tag = document.createElement('script');" +
		//		"  tag.id = 'iframe-demo';" +
		//		"  tag.src = 'https://www.youtube.com/iframe_api';" +
		//		"  var firstScriptTag = document.getElementsByTagName('script')[0];" +
		//		"  firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);" +
		//		"" +
		//		"  function onYouTubeIframeAPIReady() {" +
		//		"    var player;" +
		//		"    player = new YT.Player('player', {" +
		//		$"      videoId: '{g.VideoId}'," +
		//		"      playerVars: { 'autoplay': 1, 'controls': 0 }," +
		//		"      events: {" +
		//		"        'onReady': onPlayerReady" +
		//		"      }" +
		//		"    });" +
		//		"  }" +
		//		"  function onPlayerReady(event) {" +
		//		"    event.target.playVideo();" +
		//		"  }" +
		//		"</script>" +
		//		"</body></html>";
		//	return htmlSource;
		//}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//need to change the size of the ContentView for Landscape Orientation
			//This enables fullscreen capabilities in the Custom Renderer
			if (width > height)
			{
				//Landscape Orientation
				videoPlayer.WidthRequest = App.ScreenWidth;
				videoPlayer.HeightRequest = App.ScreenHeight;
			}
			else if (width < height)
			{
				//Portrait Orientation
				videoPlayer.WidthRequest = App.ScreenWidth / 2;
				videoPlayer.HeightRequest = App.ScreenHeight / 2;
			}

			base.LayoutChildren(x, y, width, height);
		}

		//protected override void LayoutChildren(double x, double y, double width, double height)
		//{
		//	//need to change the size of the ContentView for Landscape Orientation
		//	//This enables fullscreen capabilities in the Custom Renderer
		//	if (width > height)
		//	{
		//		//Landscape Orientation
		//		webView.WidthRequest = App.ScreenWidth;
		//		webView.HeightRequest = App.ScreenHeight;
		//		webView.Source = CreateHtmlDom(gate, webView.WidthRequest, webView.HeightRequest);
		//	}
		//	else if (width < height)
		//	{
		//		//Portrait Orientation
		//		webView.WidthRequest = App.ScreenWidth;
		//		webView.HeightRequest = App.ScreenHeight/2;
		//		webView.Source = CreateHtmlDom(gate, webView.WidthRequest, webView.HeightRequest);
		//	}

		//	base.LayoutChildren(x, y, width, height);
		//}
	}
}

