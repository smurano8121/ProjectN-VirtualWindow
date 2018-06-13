using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWindowUWP
{
    class HttpServer
    {
        private TcpListener tcp;

        public HttpServer(int port)
        {
            tcp = new TcpListener(IPAddress.Loopback, port);
            tcp.Start();
            AddAccepter();
        }

        private async void AddAccepter()
        {
            Debug.WriteLine("ServerOK.");
            while (true)
            {
                using (var tcpClient = await tcp.AcceptTcpClientAsync())
                using (var stream = tcpClient.GetStream())
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream))
                {
                    // 接続元を出力しておく
                    Debug.WriteLine(tcpClient.Client.RemoteEndPoint);

                    // ヘッダー部分を全部読む
                    string line;
                    do
                    {
                        line = await reader.ReadLineAsync();
                        // 読んだ行を出力しておく
                        Debug.WriteLine(line);
                    } while (!String.IsNullOrWhiteSpace(line));
                }
            }
        }
    }
}
