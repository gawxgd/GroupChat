using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GroupChat
{
    public class ConnectionClient
    {
        public static long id;
        public long Id { get; }
        public string Name { get; set; } = "Unauthorized";
        public TcpClient TcpClient { get; }
        public bool Connected => this.TcpClient.Connected;
        public NetworkStream NetworkStream { get; }
        public StreamReader Reader { get; }
        public StreamWriter Writer { get; }
        public CancellationTokenSource Cancellation { get; } = new CancellationTokenSource();
        public ConnectionClient(TcpClient tcpClient)
        {
            this.Id = Interlocked.Increment(ref ConnectionClient.id);
            this.TcpClient = tcpClient;
            this.NetworkStream = this.TcpClient.GetStream();
            this.Reader = new StreamReader((Stream)this.NetworkStream);
            this.Writer = new StreamWriter((Stream)this.NetworkStream);
        }
    }
}
