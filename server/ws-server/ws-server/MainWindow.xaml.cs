using System;
using System.Windows;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace ws_server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocketServer wsServer;
        private WebSocketHandler wsHandler;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void initialize_Click(object sender, RoutedEventArgs e)
        {
            stopWebSocketServer();
            startWebSocketServer();
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            wsServer.WebSocketServices.Broadcast("{\"result\": true, \"date\": \"" + DateTime.Now.ToLongDateString() + "\"}");
        }

        private void startWebSocketServer()
        {
            // TODO add authentication
            wsServer = new WebSocketServer(9790);

            wsHandler = new WebSocketHandler();
            //wsServer.AddWebSocketService<WebSocketHandler>("/ws-test", () => wsHandler);
            wsServer.AddWebSocketService<WebSocketHandler>("/ws-test");
            wsServer.Start();
            btn_send.IsEnabled = true;
        }

        private void stopWebSocketServer()
        {
            if (wsServer != null)
            {
                wsServer.Stop();
            }
            btn_send.IsEnabled = false;
        }
    }

    public class WebSocketHandler : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "status")
            {
                Send("{\"result\": true, \"message\": \"Server working\"}");
            }
            else
            {
                Send("{\"result\": false, \"message\": \"Unknown request\"}");
            }
        }
    }
}
